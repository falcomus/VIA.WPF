// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XValidationResultTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using VIA.WPF.MVVM;

namespace VIA.WPF.Tests.MVVM.Validation;

#region ### Class XValidationResultTests ###
/// <summary>
/// Tests <see cref="XValidationResult" />.
/// </summary>
public sealed class XValidationResultTests
{
    #region ### Tests ###
    /// <summary>
    /// Verifies successful result factory behavior.
    /// </summary>
    [Fact]
    public void Success_ShouldCreateValidEmptyResult()
    {
        XValidationResult result = XValidationResult.Success();
        XValidationResult secondResult = XValidationResult.Success();
        XValidationResult fromEmptyMessages = XValidationResult.FromMessages([]);

        Assert.Same(result, secondResult);
        Assert.Same(result, fromEmptyMessages);
        Assert.True(result.IsValid);
        Assert.False(result.HasMessages);
        Assert.Empty(result.Messages);
        Assert.Empty(result.Errors);
        Assert.Empty(result.Warnings);
        Assert.Empty(result.InformationMessages);
    }

    /// <summary>
    /// Verifies single-message factories.
    /// </summary>
    [Fact]
    public void Factories_ShouldCreateSeveritySpecificResults()
    {
        XValidationResult error = XValidationResult.FromError(XValidationText.Text("Error"), "Name");
        XValidationResult warning = XValidationResult.FromWarning(XValidationText.Text("Warning"), "Name");
        XValidationResult information = XValidationResult.FromInformation(XValidationText.Text("Information"), "Name");

        Assert.False(error.IsValid);
        Assert.Single(error.Errors);
        Assert.True(warning.IsValid);
        Assert.Single(warning.Warnings);
        Assert.True(information.IsValid);
        Assert.Single(information.InformationMessages);
    }

    /// <summary>
    /// Verifies aggregation by severity.
    /// </summary>
    [Fact]
    public void FromMessages_ShouldAggregateBySeverity()
    {
        XValidationResult result = XValidationResult.FromMessages(
        [
            new XValidationError(XValidationText.Text("Error"), XValidationSeverity.Error, ["A"]),
            new XValidationError(XValidationText.Text("Warning"), XValidationSeverity.Warning, ["B"]),
            new XValidationError(XValidationText.Text("Information"), XValidationSeverity.Information, ["C"])
        ]);

        Assert.False(result.IsValid);
        Assert.Equal(3, result.Messages.Count);
        Assert.Single(result.Errors);
        Assert.Single(result.Warnings);
        Assert.Single(result.InformationMessages);
        Assert.True(result.HasMessages);
        Assert.True(result.HasWarnings);
        Assert.True(result.HasInformation);
    }

    /// <summary>
    /// Verifies semantic validation message comparison helpers.
    /// </summary>
    [Fact]
    public void MessageComparison_ShouldCompareMessageContent()
    {
        XValidationResult first = XValidationResult.FromError(XValidationText.Text("Error"), " Name ", "Name");
        XValidationResult second = XValidationResult.FromError(XValidationText.Text("Error"), "Name");
        XValidationResult different = XValidationResult.FromWarning(XValidationText.Text("Error"), "Name");

        Assert.True(first.HasSameMessagesAs(second));
        Assert.True(XValidationResult.MessagesEqual(first, second));
        Assert.True(XValidationResult.MessagesEqual(first.Messages, second.Messages));
        Assert.False(first.HasSameMessagesAs(different));
        Assert.False(XValidationResult.MessagesEqual(first, null));
    }

    #endregion
}
#endregion
