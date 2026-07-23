// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XValidatableObjectReentrancyTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using VIA.WPF.MVVM;
using VIA.WPF.Tests.Helpers;

namespace VIA.WPF.Tests.MVVM.Validation;

#region ### Class XValidatableObjectReentrancyTests ###
/// <summary>
/// Regression tests for validation re-entrancy and validation activity state.
/// </summary>
public sealed class XValidatableObjectReentrancyTests
{
    #region ### Tests ###
    /// <summary>
    /// Verifies that derived display properties do not trigger additional validation runs
    /// when they are correctly excluded from automatic validation.
    /// </summary>
    [Fact]
    public async Task DerivedDisplayProperties_ShouldNotTriggerAdditionalValidationRuns()
    {
        ViewModelWithDerivedProperties viewModel = new()
        {
            ValidationDelay = TimeSpan.Zero,
            Name = "changed"
        };
        await TestAsyncHelper.WaitUntilAsync(() => !viewModel.IsValidating);

        Assert.Equal(1, viewModel.ValidateCoreCallCount);

        viewModel.Name = "changed again";
        await TestAsyncHelper.WaitUntilAsync(() => !viewModel.IsValidating);

        Assert.Equal(2, viewModel.ValidateCoreCallCount);
    }

    /// <summary>
    /// Verifies that IsValidating returns to false after ValidateAllAsync completes,
    /// even when a debounced QueueValidation call was pending at the same time.
    /// </summary>
    [Fact]
    public async Task ValidateAllAsync_ShouldClearIsValidatingAfterCompletion_EvenWhenQueuedValidationWasPending()
    {
        SimpleValidatableObject viewModel = new()
        {
            ValidationDelay = TimeSpan.FromMilliseconds(200),
            Name = "trigger queue"
        };
        bool isValid = await viewModel.ValidateAllAsync();

        await Task.Delay(300);

        Assert.False(viewModel.IsValidating, "IsValidating was still true after ValidateAllAsync completed.");
        Assert.True(isValid);
    }

    /// <summary>
    /// Verifies that IsValidating is false after a normal property-change-triggered validation completes.
    /// </summary>
    [Fact]
    public async Task IsValidating_ShouldReturnToFalse_AfterQueuedValidationCompletes()
    {
        SimpleValidatableObject viewModel = new()
        {
            ValidationDelay = TimeSpan.Zero,
            Name = "trigger"
        };
        await TestAsyncHelper.WaitUntilAsync(() => !viewModel.IsValidating);

        Assert.False(viewModel.IsValidating, "IsValidating remained true after queued validation completed.");
    }
    #endregion

    #region ### Private Classes ###
    /// <summary>
    /// A view model that fires secondary display-property notifications from its override,
    /// comparable to demo status properties such as DirtyBadgeText or ActivityBadgeText.
    /// </summary>
    private sealed class ViewModelWithDerivedProperties : XValidatableObject
    {
        #region ### Fields ###
        private string? name = "initial";
        #endregion

        #region ### Public Properties ###
        /// <summary>
        /// Gets the number of validation core executions.
        /// </summary>
        public int ValidateCoreCallCount { get; private set; }

        /// <summary>
        /// Gets or sets the editable data property.
        /// </summary>
        public string? Name
        {
            get => this.name;
            set => this.SetProperty(ref this.name, value);
        }

        /// <summary>
        /// Gets a derived display property without its own backing field.
        /// </summary>
        public string DisplayName => this.Name ?? string.Empty;

        /// <summary>
        /// Gets another derived display property that depends on validation activity.
        /// </summary>
        public string ActivityText => this.IsValidating ? "Validating" : "Ready";
        #endregion

        #region ### Protected Methods ###
        /// <inheritdoc />
        protected override Task ValidateCoreAsync(XValidationContext context, CancellationToken cancellationToken)
        {
            this.ValidateCoreCallCount++;
            context.Required(this.Name, nameof(this.Name), XValidationText.Text("Required."));
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        protected override void OnPropertyChanged(System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.PropertyName is nameof(this.Name))
            {
                this.OnPropertyChanged(nameof(this.DisplayName));
            }

            if (e.PropertyName is nameof(this.IsValidating))
            {
                this.OnPropertyChanged(nameof(this.ActivityText));
            }
        }

        /// <inheritdoc />
        protected override bool ShouldValidateAfterPropertyChanged(string? propertyName)
        {
            return base.ShouldValidateAfterPropertyChanged(propertyName)
                && propertyName is not nameof(this.DisplayName)
                && propertyName is not nameof(this.ActivityText);
        }
        #endregion
    }

    /// <summary>
    /// A minimal validatable object for IsValidating lifecycle tests.
    /// </summary>
    private sealed class SimpleValidatableObject : XValidatableObject
    {
        #region ### Fields ###
        private string? name = "valid";
        #endregion

        #region ### Public Properties ###
        /// <summary>
        /// Gets or sets the validated name.
        /// </summary>
        public string? Name
        {
            get => this.name;
            set => this.SetProperty(ref this.name, value);
        }
        #endregion

        #region ### Protected Methods ###
        /// <inheritdoc />
        protected override Task ValidateCoreAsync(XValidationContext context, CancellationToken cancellationToken)
        {
            context.Required(this.Name, nameof(this.Name), XValidationText.Text("Required."));
            return Task.CompletedTask;
        }
        #endregion
    }
    #endregion
}
#endregion
