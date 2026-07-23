// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XValidatableObjectTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections;
using VIA.WPF.MVVM;
using VIA.WPF.Tests.Helpers;

namespace VIA.WPF.Tests.MVVM.Validation;

#region ### Class XValidatableObjectTests ###
/// <summary>
/// Tests <see cref="XValidatableObject" /> validation flow and error publishing.
/// </summary>
public sealed class XValidatableObjectTests
{
    #region ### Fields ###
    private static readonly string[] DateRangePropertyNames = [nameof(TestValidatableObject.BeginDate), nameof(TestValidatableObject.EndDate)];
    #endregion

    #region ### Tests ###
    /// <summary>
    /// Verifies that full validation publishes field errors and messages.
    /// </summary>
    [Fact]
    public async Task ValidateAllAsync_ShouldPublishErrorsAndMessages()
    {
        TestValidatableObject viewModel = new()
        {
            ValidateOnPropertyChanged = false,
            Name = " ",
            Age = 140,
            BeginDate = new DateTime(2026, 05, 20),
            EndDate = new DateTime(2026, 05, 16),
            IsExternalStateValid = false
        };

        List<string> changedProperties = [];
        viewModel.ErrorsChanged += (_, e) => changedProperties.Add(e.PropertyName ?? string.Empty);

        bool isValid = await viewModel.ValidateAllAsync();

        Assert.False(isValid);
        Assert.False(viewModel.IsValid);
        Assert.True(viewModel.HasErrors);
        Assert.Equal(4, viewModel.ValidationErrors.Count);
        Assert.Contains(nameof(TestValidatableObject.Name), changedProperties);
        Assert.Contains(nameof(TestValidatableObject.Age), changedProperties);
        Assert.Contains(nameof(TestValidatableObject.BeginDate), changedProperties);
        Assert.Contains(nameof(TestValidatableObject.EndDate), changedProperties);
        Assert.NotEmpty(GetErrors(viewModel, nameof(TestValidatableObject.Name)));
        Assert.NotEmpty(GetErrors(viewModel, nameof(TestValidatableObject.Age)));
        Assert.NotEmpty(GetErrors(viewModel, nameof(TestValidatableObject.BeginDate)));
        Assert.NotEmpty(GetErrors(viewModel, nameof(TestValidatableObject.EndDate)));
    }

    /// <summary>
    /// Verifies that warnings and information messages do not make the object invalid.
    /// </summary>
    [Fact]
    public async Task ValidateAllAsync_ShouldKeepObjectValidWhenOnlyWarningsAndInformationExist()
    {
        TestValidatableObject viewModel = new()
        {
            ValidateOnPropertyChanged = false,
            ShowWarning = true,
            ShowInformation = true
        };

        bool isValid = await viewModel.ValidateAllAsync();

        Assert.True(isValid);
        Assert.True(viewModel.IsValid);
        Assert.False(viewModel.HasErrors);
        Assert.Empty(viewModel.ValidationErrors);
        Assert.Equal(2, viewModel.ValidationMessages.Count);
        Assert.Contains(viewModel.ValidationMessages, message => message.Severity == XValidationSeverity.Warning);
        Assert.Contains(viewModel.ValidationMessages, message => message.Severity == XValidationSeverity.Information);
    }

    /// <summary>
    /// Verifies that multi-field errors are exposed for each affected property.
    /// </summary>
    [Fact]
    public async Task ValidateAllAsync_ShouldExposeMultiFieldErrorForBothProperties()
    {
        TestValidatableObject viewModel = new()
        {
            ValidateOnPropertyChanged = false,
            BeginDate = new DateTime(2026, 05, 20),
            EndDate = new DateTime(2026, 05, 16)
        };

        await viewModel.ValidateAllAsync();

        XValidationError beginDateError = Assert.Single(GetErrors(viewModel, nameof(TestValidatableObject.BeginDate)));
        XValidationError endDateError = Assert.Single(GetErrors(viewModel, nameof(TestValidatableObject.EndDate)));
        Assert.Same(beginDateError, endDateError);
        Assert.Equal(DateRangePropertyNames, beginDateError.PropertyNames);
    }

    /// <summary>
    /// Verifies that ClearValidation removes errors and messages.
    /// </summary>
    [Fact]
    public async Task ClearValidation_ShouldRemoveErrorsAndMessages()
    {
        TestValidatableObject viewModel = new()
        {
            ValidateOnPropertyChanged = false,
            Name = null
        };

        await viewModel.ValidateAllAsync();
        Assert.True(viewModel.HasErrors);

        viewModel.ClearValidation();

        Assert.True(viewModel.IsValid);
        Assert.False(viewModel.HasErrors);
        Assert.Empty(viewModel.ValidationMessages);
        Assert.Empty(GetErrors(viewModel, nameof(TestValidatableObject.Name)));
    }

    /// <summary>
    /// Verifies that disabling validation clears existing messages and suppresses later validation.
    /// </summary>
    [Fact]
    public async Task IsValidationEnabledFalse_ShouldClearAndSuppressValidation()
    {
        TestValidatableObject viewModel = new()
        {
            ValidateOnPropertyChanged = false,
            Name = null
        };

        await viewModel.ValidateAllAsync();
        Assert.True(viewModel.HasErrors);

        viewModel.IsValidationEnabled = false;
        bool isValid = await viewModel.ValidateAllAsync();

        Assert.True(isValid);
        Assert.True(viewModel.IsValid);
        Assert.Empty(viewModel.ValidationMessages);
        Assert.Equal(1, viewModel.ValidateCoreCallCount);
    }

    /// <summary>
    /// Verifies automatic validation after property changes.
    /// </summary>
    [Fact]
    public async Task PropertyChange_ShouldQueueValidationWhenEnabled()
    {
        TestValidatableObject viewModel = new()
        {
            ValidationDelay = TimeSpan.Zero,
            Name = null
        };

        await TestAsyncHelper.WaitUntilAsync(() => viewModel.HasErrors);

        Assert.False(viewModel.IsValid);
        Assert.NotEmpty(GetErrors(viewModel, nameof(TestValidatableObject.Name)));
    }

    /// <summary>
    /// Verifies that outdated asynchronous validation results do not overwrite newer results.
    /// </summary>
    [Fact]
    public async Task PropertyChange_ShouldIgnoreOutdatedValidationResults()
    {
        DelayedValidationObject viewModel = new()
        {
            ValidationDelay = TimeSpan.Zero,
            Name = "slow-invalid"
        };
        await TestAsyncHelper.WaitUntilAsync(() => viewModel.ValidationRunCount >= 1);

        viewModel.Name = "valid";
        await TestAsyncHelper.WaitUntilAsync(() => viewModel.ValidationRunCount >= 2);
        await Task.Delay(180);

        Assert.True(viewModel.IsValid);
        Assert.False(viewModel.HasErrors);
        Assert.Empty(viewModel.ValidationErrors);
    }
    #endregion

    #region ### Private Methods ###
    private static XValidationError[] GetErrors(XValidatableObject viewModel, string propertyName)
    {
        return [.. ((IEnumerable)viewModel.GetErrors(propertyName)).Cast<XValidationError>()];
    }
    #endregion

    #region ### Private Classes ###
    private sealed class TestValidatableObject : XValidatableObject
    {
        #region ### Fields ###
        private string? name = "Valid name";
        private int? age = 25;
        private DateTime? beginDate = new DateTime(2026, 05, 16);
        private DateTime? endDate = new DateTime(2026, 05, 20);
        private bool isExternalStateValid = true;
        private bool showWarning;
        private bool showInformation;
        #endregion

        #region ### Public Properties ###
        public int ValidateCoreCallCount { get; private set; }

        public string? Name
        {
            get => this.name;
            set => this.SetProperty(ref this.name, value);
        }

        public int? Age
        {
            get => this.age;
            set => this.SetProperty(ref this.age, value);
        }

        public DateTime? BeginDate
        {
            get => this.beginDate;
            set => this.SetProperty(ref this.beginDate, value);
        }

        public DateTime? EndDate
        {
            get => this.endDate;
            set => this.SetProperty(ref this.endDate, value);
        }

        public bool IsExternalStateValid
        {
            get => this.isExternalStateValid;
            set => this.SetProperty(ref this.isExternalStateValid, value);
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
        protected override async Task ValidateCoreAsync(XValidationContext context, CancellationToken cancellationToken)
        {
            this.ValidateCoreCallCount++;

            context.Required(this.Name, nameof(this.Name), XValidationText.Text("Name is required."));
            context.Range(this.Age, 0, 120, nameof(this.Age), XValidationText.Text("Age is out of range."));
            context.MustBeBeforeOrEqual<DateTime>(
                this.BeginDate,
                nameof(this.BeginDate),
                this.EndDate,
                nameof(this.EndDate),
                XValidationText.Text("Begin date must not be after end date."));

            if (this.ShowWarning)
            {
                context.WarningIf(true, XValidationText.Text("Warning message."), nameof(this.Name));
            }

            if (this.ShowInformation)
            {
                context.AddInformation(XValidationText.Text("Information message."), nameof(this.Name));
            }

            await context.MustBeTrueAsync(
                _ => Task.FromResult(this.IsExternalStateValid),
                XValidationText.Text("External state is invalid."),
                cancellationToken,
                nameof(this.Name));
        }
        #endregion
    }

    private sealed class DelayedValidationObject : XValidatableObject
    {
        #region ### Fields ###
        private int validationRunCount;
        private string? name = "valid";
        #endregion

        #region ### Public Properties ###
        public int ValidationRunCount => Volatile.Read(ref this.validationRunCount);

        public string? Name
        {
            get => this.name;
            set => this.SetProperty(ref this.name, value);
        }
        #endregion

        #region ### Protected Methods ###
        protected override async Task ValidateCoreAsync(XValidationContext context, CancellationToken cancellationToken)
        {
            _ = cancellationToken;
            string? snapshot = this.Name;
            Interlocked.Increment(ref this.validationRunCount);

            if (string.Equals(snapshot, "slow-invalid", StringComparison.Ordinal))
            {
                await Task.Delay(120, CancellationToken.None).ConfigureAwait(false);
                context.AddError(XValidationText.Text("Old invalid value."), nameof(this.Name));
                return;
            }

            await Task.Delay(10, CancellationToken.None).ConfigureAwait(false);
        }
        #endregion
    }
    #endregion
}
#endregion