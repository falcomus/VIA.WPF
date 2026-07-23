// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XEditorViewModelBaseTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using VIA.WPF.MVVM;

namespace VIA.WPF.Tests.MVVM.ViewModels;

#region ### Class XEditorViewModelBaseTests ###
/// <summary>
/// Tests dirty tracking and save validation of <see cref="XEditorViewModelBase" />.
/// </summary>
public sealed class XEditorViewModelBaseTests
{
    #region ### Tests ###
    /// <summary>
    /// Verifies that changing an editor property marks the editor as dirty.
    /// </summary>
    [Fact]
    public void PropertyChange_ShouldMarkEditorDirty()
    {
        TestEditorViewModel viewModel = new()
        {
            Name = "Changed"
        };

        Assert.True(viewModel.IsDirty);
    }

    /// <summary>
    /// Verifies that MarkClean resets the dirty state.
    /// </summary>
    [Fact]
    public void MarkClean_ShouldResetDirtyState()
    {
        TestEditorViewModel viewModel = new()
        {
            Name = "Changed"
        };

        viewModel.MarkClean();

        Assert.False(viewModel.IsDirty);
    }

    /// <summary>
    /// Verifies that initialization can be performed without dirty tracking.
    /// </summary>
    [Fact]
    public void WithoutDirtyTracking_ShouldSuppressDirtyTracking()
    {
        TestEditorViewModel viewModel = new();

        viewModel.LoadSilently("Loaded");

        Assert.False(viewModel.IsDirty);
        Assert.Equal("Loaded", viewModel.Name);
    }

    /// <summary>
    /// Verifies that disabled dirty tracking suppresses dirty state changes.
    /// </summary>
    [Fact]
    public void IsDirtyTrackingEnabledFalse_ShouldSuppressDirtyTracking()
    {
        TestEditorViewModel viewModel = new()
        {
            IsDirtyTrackingEnabled = false
        };

        viewModel.MarkClean();
        viewModel.Name = "Changed";

        Assert.False(viewModel.IsDirty);
    }

    /// <summary>
    /// Verifies that editor state properties do not mark the editor as dirty.
    /// </summary>
    [Fact]
    public void EditorStateProperties_ShouldNotMarkEditorDirty()
    {
        TestEditorViewModel viewModel = new()
        {
            IsDirty = true
        };

        viewModel.MarkClean();
        viewModel.IsReadOnly = true;
        viewModel.IsDirtyTrackingEnabled = false;

        Assert.False(viewModel.IsDirty);
    }

    /// <summary>
    /// Verifies that ValidateForSaveAsync executes normal validation.
    /// </summary>
    [Fact]
    public async Task ValidateForSaveAsync_ShouldRunValidation()
    {
        TestEditorViewModel viewModel = new()
        {
            Name = null,
            ValidateOnPropertyChanged = false
        };

        bool isValid = await viewModel.ValidateForSaveAsync();

        Assert.False(isValid);
        Assert.True(viewModel.HasErrors);
        Assert.Single(viewModel.ValidationErrors);
    }
    #endregion

    #region ### Private Classes ###
    private sealed class TestEditorViewModel : XEditorViewModelBase
    {
        #region ### Fields ###
        private string? name = "Valid";
        #endregion

        #region ### Public Properties ###
        public string? Name
        {
            get => this.name;
            set => this.SetProperty(ref this.name, value);
        }
        #endregion

        #region ### Public Methods ###
        public void LoadSilently(string? value)
        {
            this.WithoutDirtyTracking(() => this.Name = value);
        }
        #endregion

        #region ### Protected Methods ###
        protected override Task ValidateCoreAsync(XValidationContext context, CancellationToken cancellationToken)
        {
            _ = cancellationToken;
            context.Required(this.Name, nameof(this.Name), XValidationText.Text("Name is required."));
            return Task.CompletedTask;
        }
        #endregion
    }
    #endregion
}
#endregion
