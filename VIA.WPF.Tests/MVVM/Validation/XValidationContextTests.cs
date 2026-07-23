// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XValidationContextTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using VIA.WPF.MVVM;

namespace VIA.WPF.Tests.MVVM.Validation;

#region ### Class XValidationContextTests ###
/// <summary>
/// Tests the <see cref="XValidationContext" /> validation rule helpers.
/// </summary>
public sealed class XValidationContextTests
{
    #region ### Fields ###
    private static readonly string[] MessagePropertyNames = ["First", "Second"];

    private static readonly string[] RequiredPropertyNames = ["NullValue", "TextValue", "GuidValue", "CollectionValue"];

    private static readonly string[] LengthAndRangePropertyNames = ["Text", "Range", "GreaterThan", "GreaterThanOrEqual", "LessThan", "LessThanOrEqual"];

    private static readonly string[] DateRangePropertyNames = ["BeginDate", "EndDate"];
    #endregion

    #region ### Tests ###
    /// <summary>
    /// Verifies that messages are collected with severity, code and property assignment.
    /// </summary>
    [Fact]
    public void AddMessage_ShouldStoreSeverityCodeAndProperties()
    {
        object source = new();
        XValidationContext context = new(source);

        context.AddMessage(
            XValidationText.Text("Message"),
            XValidationSeverity.Warning,
            MessagePropertyNames,
            "CODE-1");

        XValidationError message = Assert.Single(context.Messages);
        Assert.Same(source, context.Source);
        Assert.Equal("Message", message.Message);
        Assert.Equal(XValidationSeverity.Warning, message.Severity);
        Assert.Equal("CODE-1", message.Code);
        Assert.Equal(MessagePropertyNames, message.PropertyNames);
    }

    /// <summary>
    /// Verifies that empty required values create validation errors.
    /// </summary>
    [Fact]
    public void Required_ShouldAddErrorsForEmptyValues()
    {
        XValidationContext context = new(new object());
        XValidationText text = XValidationText.Text("Required");
        string[] emptyCollection = [];

        context.Required(null, "NullValue", text);
        context.Required("   ", "TextValue", text);
        context.Required(Guid.Empty, "GuidValue", text);
        context.Required(emptyCollection, "CollectionValue", text);

        IEnumerable<string> actualPropertyNames = context.Messages.Select(message => message.PropertyNames.Single());

        Assert.Equal(4, context.Messages.Count);
        Assert.All(context.Messages, message => Assert.Equal(XValidationSeverity.Error, message.Severity));
        Assert.Equal(
            RequiredPropertyNames.AsEnumerable(),
            actualPropertyNames);
    }

    /// <summary>
    /// Verifies that scalar default values are not treated as empty by Required.
    /// </summary>
    [Fact]
    public void Required_ShouldNotTreatScalarDefaultValuesAsEmpty()
    {
        XValidationContext context = new(new object());
        XValidationText text = XValidationText.Text("Required");

        context.Required(0, "Number", text);
        context.Required(false, "Boolean", text);
        context.Required(0m, "Decimal", text);

        Assert.Empty(context.Messages);
    }

    /// <summary>
    /// Verifies nullable and default value helper methods.
    /// </summary>
    [Fact]
    public void NullableAndDefaultHelpers_ShouldAddExpectedErrors()
    {
        XValidationContext context = new(new object());
        XValidationText text = XValidationText.Text("Required");

        context.RequiredNullable<int>(null, "NullableNumber", text);
        context.NotDefault(0, "DefaultNumber", text);
        context.NotDefault<int>(null, "NullDefaultNumber", text);
        context.NotDefault<int>(0, "NullableDefaultNumber", text);
        context.NotDefault<int>(12, "ValidNullableNumber", text);

        Assert.Equal(4, context.Messages.Count);
        Assert.DoesNotContain(context.Messages, message => message.PropertyNames.Contains("ValidNullableNumber", StringComparer.Ordinal));
    }

    /// <summary>
    /// Tests whether length and range helper methods add the expected validation errors.
    /// </summary>
    [Fact]
    public void LengthAndRangeHelpers_ShouldAddExpectedErrors()
    {
        XValidationContext context = new(new object());
        XValidationText text = XValidationText.Text("Invalid");

        context.MaxLength("12345", 3, "Text", text);
        context.Range(15, 1, 10, "Range", text);
        context.GreaterThan(5, 5, "GreaterThan", text);
        context.GreaterThanOrEqual(4, 5, "GreaterThanOrEqual", text);
        context.LessThan(5, 5, "LessThan", text);
        context.LessThanOrEqual(6, 5, "LessThanOrEqual", text);

        string[] actualPropertyNames =
        [
            .. context.Messages.Select(message => message.PropertyNames.Single())
        ];

        Assert.Equal(6, context.Messages.Count);
        Assert.Equal(
            LengthAndRangePropertyNames.AsEnumerable(),
            actualPropertyNames.AsEnumerable());
    }

    /// <summary>
    /// Verifies that multi-field date validation assigns the same error to both fields.
    /// </summary>
    [Fact]
    public void MustBeBeforeOrEqual_ShouldAssignErrorToBothProperties()
    {
        XValidationContext context = new(new object());

        context.MustBeBeforeOrEqual<DateTime>(
            new DateTime(2026, 05, 20),
            "BeginDate",
            new DateTime(2026, 05, 16),
            "EndDate",
            XValidationText.Text("Begin after end"));

        XValidationError error = Assert.Single(context.Messages);
        Assert.Equal(XValidationSeverity.Error, error.Severity);
        Assert.Equal(DateRangePropertyNames, error.PropertyNames);
    }

    /// <summary>
    /// Verifies asynchronous rule helpers.
    /// </summary>
    [Fact]
    public async Task MustBeTrueAsync_ShouldAddErrorWhenPredicateReturnsFalse()
    {
        XValidationContext context = new(new object());

        await context.MustBeTrueAsync(
            _ => Task.FromResult(false),
            XValidationText.Text("Async error"),
            CancellationToken.None,
            "ExternalState");

        XValidationError error = Assert.Single(context.Messages);
        Assert.Equal("Async error", error.Message);
        Assert.Equal("ExternalState", error.PropertyNames.Single());
    }

    /// <summary>
    /// Verifies that asynchronous rules do not add errors when the predicate returns true.
    /// </summary>
    [Fact]
    public async Task MustBeTrueAsync_ShouldNotAddErrorWhenPredicateReturnsTrue()
    {
        XValidationContext context = new(new object());

        await context.MustBeTrueAsync(
            _ => Task.FromResult(true),
            XValidationText.Text("Async error"),
            CancellationToken.None,
            "ExternalState");

        Assert.Empty(context.Messages);
    }
    #endregion
}
#endregion
