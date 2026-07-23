// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XEditorExternalValidationTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using VIA.WPF.MVVM;

namespace VIA.WPF.Tests.MVVM.ViewModels;

#region ### Class XEditorExternalValidationTests ###
/// <summary>
/// Tests external validation support for <see cref="XEditorViewModelBase" />.
/// </summary>
public sealed class XEditorExternalValidationTests
{
    #region ### Tests ###
    /// <summary>
    /// Ensures that external validation errors participate in save validation.
    /// </summary>
    [Fact]
    public async Task ValidateForSaveAsync_ShouldIncludeExternalValidationErrors()
    {
        TestEditorViewModel viewModel = new()
        {
            ValidateOnPropertyChanged = false
        };

        viewModel.AddExternalValidationError("Server rejected the code.", nameof(TestEditorViewModel.Code), code: "DuplicateCode");

        bool result = await viewModel.ValidateForSaveAsync();

        Assert.False(result);
        Assert.True(viewModel.HasExternalValidationErrors);
        Assert.True(viewModel.HasErrors);
        XValidationError error = Assert.Single(viewModel.ValidationErrors);
        Assert.Equal("Server rejected the code.", error.Message);
        Assert.Equal("DuplicateCode", error.Code);
        Assert.Contains(nameof(TestEditorViewModel.Code), error.PropertyNames);
    }

    /// <summary>
    /// Ensures that clearing external validation errors removes them from the next save validation.
    /// </summary>
    [Fact]
    public async Task ClearExternalValidationErrors_ShouldRemoveErrorsFromNextValidation()
    {
        TestEditorViewModel viewModel = new()
        {
            ValidateOnPropertyChanged = false
        };

        viewModel.AddExternalValidationError("Server rejected the code.", nameof(TestEditorViewModel.Code));
        _ = await viewModel.ValidateForSaveAsync();

        viewModel.ClearExternalValidationErrors();
        bool result = await viewModel.ValidateForSaveAsync();

        Assert.True(result);
        Assert.False(viewModel.HasExternalValidationErrors);
        Assert.False(viewModel.HasErrors);
    }
    #endregion

    #region ### Private Classes ###
    private sealed class TestEditorViewModel : XEditorViewModelBase
    {
        #region ### Fields ###
        private string? code = "VALID";
        #endregion

        #region ### Public Properties ###
        public string? Code
        {
            get => this.code;
            set => this.SetProperty(ref this.code, value);
        }
        #endregion
    }
    #endregion
}
#endregion
