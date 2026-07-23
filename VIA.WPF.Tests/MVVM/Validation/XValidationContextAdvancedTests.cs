// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XValidationContextAdvancedTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Text.RegularExpressions;
using VIA.WPF.MVVM;

namespace VIA.WPF.Tests.MVVM.Validation;

#region ### Class XValidationContextAdvancedTests ###
/// <summary>
/// Tests advanced validation context helper methods.
/// </summary>
public sealed class XValidationContextAdvancedTests
{
    #region ### Fields ###
    private static readonly string[] ConditionalPropertyNames = ["Required", "MaxLength", "Range"];

    private static readonly string[] StringHelperPropertyNames =
    [
        "MinLength",
        "Matches",
        "MatchesOptions",
        "Email",
        "DisplayNameEmail",
        "Url",
        "WebUrl",
        "RestrictedUrl"
    ];

    private static readonly string[] AllowedWebSchemes = [Uri.UriSchemeHttp, Uri.UriSchemeHttps];

    private static readonly string[] ComparePropertyNames = ["Start", "End"];

    private static readonly string[] UnnormalizedPropertyNames = [" Name ", "Name", "", "  ", "Other"];

    private static readonly string[] NormalizedPropertyNames = ["Name", "Other"];

    private static readonly string[] EmptyPropertyNames = [];

    private static readonly string[] EntityLevelPropertyNames = [string.Empty];
    #endregion

    #region ### Tests ###
    /// <summary>
    /// Verifies conditional helper methods.
    /// </summary>
    [Fact]
    public void ConditionalHelpers_ShouldOnlyAddErrorsWhenConditionMatches()
    {
        XValidationContext context = new(new object());
        XValidationText text = XValidationText.Text("Invalid");

        context.RequiredIf(true, " ", "Required", text);
        context.RequiredIf(false, " ", "SkippedRequired", text);
        context.MaxLengthIf(true, "1234", 3, "MaxLength", text);
        context.MaxLengthIf(false, "1234", 3, "SkippedMaxLength", text);
        context.RangeIf(true, 12, 1, 10, "Range", text);
        context.RangeIf(false, 12, 1, 10, "SkippedRange", text);

        string[] propertyNames = [.. context.Messages.Select(message => message.PropertyNames.Single())];

        Assert.Equal(ConditionalPropertyNames, propertyNames);
    }

    /// <summary>
    /// Verifies string helper methods.
    /// </summary>
    [Fact]
    public void StringHelpers_ShouldAddExpectedErrors()
    {
        XValidationContext context = new(new object());
        XValidationText text = XValidationText.Text("Invalid");

        context.MinLength("ab", 3, "MinLength", text);
        context.Matches("abc", "^[0-9]+$", "Matches", text);
        context.Matches("ABC", "^[a-z]+$", RegexOptions.None, "MatchesOptions", text);
        context.Email("not-an-email", "Email", text);
        context.Email("Name <user@example.com>", "DisplayNameEmail", text);
        context.Url("not-an-url", "Url", text);
        context.WebUrl("ftp://example.com/file.txt", "WebUrl", text);
        context.Url("ftp://example.com/file.txt", "RestrictedUrl", text, AllowedWebSchemes);

        string[] propertyNames = [.. context.Messages.Select(message => message.PropertyNames.Single())];

        Assert.Equal(StringHelperPropertyNames, propertyNames);
    }

    /// <summary>
    /// Verifies that null values are skipped by optional string validators.
    /// </summary>
    [Fact]
    public void StringHelpers_ShouldSkipNullValues()
    {
        XValidationContext context = new(new object());
        XValidationText text = XValidationText.Text("Invalid");

        context.MinLength(null, 3, "MinLength", text);
        context.MaxLength(null, 3, "MaxLength", text);
        context.Matches(null, "^[0-9]+$", "Matches", text);
        context.Matches(null, "^[0-9]+$", RegexOptions.IgnoreCase, "MatchesOptions", text);
        context.Email(null, "Email", text);
        context.Url(null, "Url", text);
        context.WebUrl(null, "WebUrl", text);

        Assert.Empty(context.Messages);
    }

    /// <summary>
    /// Verifies that optional URL scheme restriction keeps unrestricted absolute URI behavior intact.
    /// </summary>
    [Fact]
    public void Url_ShouldAllowEveryAbsoluteSchemeUnlessSchemesAreRestricted()
    {
        XValidationContext context = new(new object());
        XValidationText text = XValidationText.Text("Invalid");

        context.Url("ftp://example.com/file.txt", "UnrestrictedUrl", text);
        context.Url("https://example.com", "RestrictedUrl", text, AllowedWebSchemes);
        context.WebUrl("https://example.com", "WebUrl", text);

        Assert.Empty(context.Messages);
    }

    /// <summary>
    /// Verifies custom cross-property comparison.
    /// </summary>
    [Fact]
    public void Compare_ShouldSkipNullValuesAndAddErrorForInvalidValues()
    {
        XValidationContext context = new(new object());
        XValidationText text = XValidationText.Text("Invalid");

        context.Compare<DateTime?>(
            null,
            "NullStart",
            new DateTime(2026, 05, 20),
            "NullEnd",
            (start, end) => start > end,
            text);

        context.Compare<DateTime?>(
            new DateTime(2026, 05, 21),
            "Start",
            new DateTime(2026, 05, 20),
            "End",
            (start, end) => start > end,
            text);

        XValidationError error = Assert.Single(context.Messages);

        Assert.Equal(ComparePropertyNames, error.PropertyNames);
    }

    /// <summary>
    /// Verifies property name normalization.
    /// </summary>
    [Fact]
    public void AddMessage_ShouldNormalizePropertyNames()
    {
        XValidationContext context = new(new object());

        context.AddMessage(
            XValidationText.Text("Invalid"),
            XValidationSeverity.Error,
            UnnormalizedPropertyNames);

        XValidationError error = Assert.Single(context.Messages);

        Assert.Equal(NormalizedPropertyNames, error.PropertyNames);
    }

    /// <summary>
    /// Verifies entity-level fallback for messages without property names.
    /// </summary>
    [Fact]
    public void AddMessage_ShouldUseEntityLevelFallbackWhenPropertyNamesAreEmpty()
    {
        XValidationContext context = new(new object());

        context.AddMessage(XValidationText.Text("Invalid"), XValidationSeverity.Error, EmptyPropertyNames);

        XValidationError error = Assert.Single(context.Messages);

        Assert.Equal(EntityLevelPropertyNames, error.PropertyNames);
    }
    #endregion
}
#endregion
