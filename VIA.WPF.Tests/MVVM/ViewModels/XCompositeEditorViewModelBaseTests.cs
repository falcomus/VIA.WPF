// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XCompositeEditorViewModelBaseTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using VIA.WPF.MVVM;

namespace VIA.WPF.Tests.MVVM.ViewModels;

#region ### Class XCompositeEditorViewModelBaseTests ###
/// <summary>
/// Tests <see cref="XCompositeEditorViewModelBase" />.
/// </summary>
public sealed class XCompositeEditorViewModelBaseTests
{
    #region ### Tests ###
    /// <summary>
    /// Ensures that child dirty state marks the composite editor as dirty.
    /// </summary>
    [Fact]
    public void ChildDirtyState_ShouldMarkCompositeDirty()
    {
        TestCompositeEditorViewModel composite = new();
        composite.MarkClean();

        composite.Child.Name = "Changed";

        Assert.True(composite.Child.IsDirty);
        Assert.True(composite.IsDirty);
        Assert.True(composite.HasDirtyChildEditors);
    }

    /// <summary>
    /// Ensures that MarkClean resets all child editors.
    /// </summary>
    [Fact]
    public void MarkClean_ShouldResetChildEditors()
    {
        TestCompositeEditorViewModel composite = new();
        composite.Child.Name = "Changed";

        composite.MarkClean();

        Assert.False(composite.IsDirty);
        Assert.False(composite.Child.IsDirty);
        Assert.False(composite.HasDirtyChildEditors);
    }

    /// <summary>
    /// Ensures that save validation includes child editor validation results.
    /// </summary>
    [Fact]
    public async Task ValidateForSaveAsync_ShouldIncludeChildEditorErrors()
    {
        TestCompositeEditorViewModel composite = new()
        {
            ValidateOnPropertyChanged = false
        };
        composite.Child.ValidateOnPropertyChanged = false;
        composite.Child.Name = null;

        bool result = await composite.ValidateForSaveAsync();

        Assert.False(result);
        Assert.True(composite.HasAnyErrors);
        Assert.True(composite.HasChildErrors);
        Assert.Contains(composite.ValidationErrors, error => error.Message == "Name is required.");
    }
    #endregion

    #region ### Private Classes ###
    private sealed class TestCompositeEditorViewModel : XCompositeEditorViewModelBase
    {
        #region ### Constructors ###
        public TestCompositeEditorViewModel()
        {
            this.Child = this.RegisterChildEditor(new TestChildEditorViewModel());
        }
        #endregion

        #region ### Public Properties ###
        public TestChildEditorViewModel Child { get; }
        #endregion
    }

    private sealed class TestChildEditorViewModel : XEditorViewModelBase
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
