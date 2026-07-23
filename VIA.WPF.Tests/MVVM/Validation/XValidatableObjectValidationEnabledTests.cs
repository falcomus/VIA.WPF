// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XValidatableObjectValidationEnabledTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using XValidatableObjectBase = global::VIA.WPF.MVVM.XValidatableObject;
using XValidationContextType = global::VIA.WPF.MVVM.XValidationContext;
using XValidationTextType = global::VIA.WPF.MVVM.XValidationText;

namespace VIA.WPF.Tests.MVVM.Validation;

#region ### Class XValidatableObjectValidationEnabledTests ###
/// <summary>
/// Tests validation enable and disable behavior.
/// </summary>
public sealed class XValidatableObjectValidationEnabledTests
{
    #region ### Tests ###
    /// <summary>
    /// Verifies that disabled validation clears existing messages and suppresses explicit validation.
    /// </summary>
    [Fact]
    public async Task IsValidationEnabledFalse_ShouldClearAndSuppressExplicitValidation()
    {
        TestValidatableObject viewModel = new();

        await viewModel.ValidateAllAsync();

        Assert.False(viewModel.IsValid);
        Assert.Equal(1, viewModel.ValidateCoreCallCount);
        Assert.NotEmpty(viewModel.ValidationMessages);

        viewModel.IsValidationEnabled = false;
        bool isValid = await viewModel.ValidateAllAsync();

        Assert.True(isValid);
        Assert.True(viewModel.IsValid);
        Assert.Empty(viewModel.ValidationMessages);
        Assert.Equal(1, viewModel.ValidateCoreCallCount);
    }
    #endregion

    #region ### Class TestValidatableObject ###
    private sealed class TestValidatableObject : XValidatableObjectBase
    {
        #region ### Public Properties ###
        /// <summary>
        /// Gets the number of times validation was executed.
        /// </summary>
        public int ValidateCoreCallCount { get; private set; }
        #endregion

        #region ### Protected Methods ###
        /// <inheritdoc />
        protected override Task ValidateCoreAsync(XValidationContextType context, CancellationToken cancellationToken)
        {
            this.ValidateCoreCallCount++;
            context.Required(null, "Name", XValidationTextType.Text("Name is required."));
            return Task.CompletedTask;
        }
        #endregion
    }
    #endregion
}
#endregion