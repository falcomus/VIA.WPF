// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XValidatableObjectAdvancedTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections;
using VIA.WPF.MVVM;
using VIA.WPF.Tests.Helpers;

namespace VIA.WPF.Tests.MVVM.Validation;

#region ### Class XValidatableObjectAdvancedTests ###
/// <summary>
/// Tests advanced <see cref="XValidatableObject" /> behavior.
/// </summary>
public sealed class XValidatableObjectAdvancedTests
{
    #region ### Tests ###
    /// <summary>
    /// Verifies detailed validation result output and summary properties.
    /// </summary>
    [Fact]
    public async Task ValidateAllDetailedAsync_ShouldReturnResultAndUpdateSummaryProperties()
    {
        AdvancedValidatableObject viewModel = new()
        {
            ValidateOnPropertyChanged = false,
            Name = "ab",
            Code = "invalid",
            ShowWarning = true,
            ShowInformation = true
        };

        XValidationResult result = await viewModel.ValidateAllDetailedAsync();

        Assert.False(result.IsValid);
        Assert.Equal(2, result.Errors.Count);
        Assert.Single(result.Warnings);
        Assert.Single(result.InformationMessages);
        Assert.Equal(2, viewModel.ValidationErrorCount);
        Assert.Equal(1, viewModel.ValidationWarningCount);
        Assert.Equal(1, viewModel.ValidationInformationCount);
        Assert.True(viewModel.HasValidationMessages);
        Assert.True(viewModel.HasValidationWarnings);
        Assert.True(viewModel.HasValidationInformation);
        Assert.Contains("Name too short.", viewModel.ValidationSummaryText, StringComparison.Ordinal);
    }

    /// <summary>
    /// Verifies that severity-filtered validation snapshots are reused until messages change.
    /// </summary>
    [Fact]
    public async Task ValidationSeveritySnapshots_ShouldRemainStableUntilMessagesChange()
    {
        AdvancedValidatableObject viewModel = new()
        {
            ValidateOnPropertyChanged = false,
            Name = "ab",
            Code = "invalid",
            ShowWarning = true,
            ShowInformation = true
        };

        await viewModel.ValidateAllAsync();

        IReadOnlyList<XValidationError> errors = viewModel.ValidationErrors;
        IReadOnlyList<XValidationError> warnings = viewModel.ValidationWarnings;
        IReadOnlyList<XValidationError> informationMessages = viewModel.ValidationInformationMessages;
        string summaryText = viewModel.ValidationSummaryText;

        Assert.Same(errors, viewModel.ValidationErrors);
        Assert.Same(warnings, viewModel.ValidationWarnings);
        Assert.Same(informationMessages, viewModel.ValidationInformationMessages);
        Assert.Equal(summaryText, viewModel.ValidationSummaryText);

        viewModel.Name = "Valid";
        await viewModel.ValidateAllAsync();

        Assert.NotSame(errors, viewModel.ValidationErrors);
    }

    /// <summary>
    /// Verifies that dependent properties can be revalidated without changing their value.
    /// </summary>
    [Fact]
    public async Task InvalidateProperties_ShouldRevalidateDependentProperties()
    {
        DependentValidatableObject viewModel = new()
        {
            ValidationDelay = TimeSpan.Zero,
            IsRequired = false,
            Value = null
        };

        viewModel.IsRequired = true;

        await TestAsyncHelper.WaitUntilAsync(() => viewModel.HasErrors);

        Assert.NotEmpty(GetErrors(viewModel, nameof(DependentValidatableObject.Value)));
    }

    /// <summary>
    /// Verifies that unchanged error lists do not raise repeated ErrorsChanged notifications.
    /// </summary>
    [Fact]
    public async Task ValidateAllAsync_ShouldNotRaiseErrorsChangedWhenErrorsDidNotChange()
    {
        AdvancedValidatableObject viewModel = new()
        {
            ValidateOnPropertyChanged = false,
            Name = "ab"
        };

        int errorsChangedCount = 0;
        viewModel.ErrorsChanged += (_, _) => errorsChangedCount++;

        await viewModel.ValidateAllAsync();
        await viewModel.ValidateAllAsync();

        Assert.Equal(1, errorsChangedCount);
    }
    #endregion

    #region ### Private Methods ###
    private static XValidationError[] GetErrors(XValidatableObject viewModel, string propertyName)
    {
        return [.. ((IEnumerable)viewModel.GetErrors(propertyName)).Cast<XValidationError>()];
    }
    #endregion

    #region ### Private Classes ###
    private sealed class AdvancedValidatableObject : XValidatableObject
    {
        #region ### Fields ###
        private string? name = "Valid";
        private string? code = "123";
        private bool showWarning;
        private bool showInformation;
        #endregion

        #region ### Public Properties ###
        public string? Name
        {
            get => this.name;
            set => this.SetProperty(ref this.name, value);
        }

        public string? Code
        {
            get => this.code;
            set => this.SetProperty(ref this.code, value);
        }

        public bool ShowWarning
        {
            get => this.showWarning;
            set => this.SetProperty(ref this.showWarning, value);
        }

        public bool ShowInformation
        {
            get => this.showInformation;
            set => this.SetProperty(ref this.showInformation, value);
        }
        #endregion

        #region ### Protected Methods ###
        protected override Task ValidateCoreAsync(XValidationContext context, CancellationToken cancellationToken)
        {
            _ = cancellationToken;

            context.MinLength(this.Name, 3, nameof(this.Name), XValidationText.Text("Name too short."));
            context.Matches(this.Code, "^[0-9]+$", nameof(this.Code), XValidationText.Text("Code must be numeric."));
            context.WarningIf(this.ShowWarning, XValidationText.Text("Warning."), nameof(this.Name));
            context.InformationIf(this.ShowInformation, XValidationText.Text("Information."), nameof(this.Name));

            return Task.CompletedTask;
        }
        #endregion
    }

    private sealed class DependentValidatableObject : XValidatableObject
    {
        #region ### Fields ###
        private bool isRequired;
        private string? value;
        #endregion

        #region ### Public Properties ###
        public bool IsRequired
        {
            get => this.isRequired;
            set
            {
                if (this.SetProperty(ref this.isRequired, value))
                {
                    this.InvalidateProperties(nameof(this.Value));
                }
            }
        }

        public string? Value
        {
            get => this.value;
            set => this.SetProperty(ref this.value, value);
        }
        #endregion

        #region ### Protected Methods ###
        protected override Task ValidateCoreAsync(XValidationContext context, CancellationToken cancellationToken)
        {
            _ = cancellationToken;

            context.RequiredIf(this.IsRequired, this.Value, nameof(this.Value), XValidationText.Text("Value is required."));

            return Task.CompletedTask;
        }
        #endregion
    }
    #endregion
}
#endregion
