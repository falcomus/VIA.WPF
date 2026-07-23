// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XEditorViewModelBaseAdvancedTests.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using VIA.WPF.MVVM;
using VIA.WPF.Tests.Helpers;

namespace VIA.WPF.Tests.MVVM.ViewModels;

#region ### Class XEditorViewModelBaseAdvancedTests ###
/// <summary>
/// Tests advanced editor behavior.
/// </summary>
public sealed class XEditorViewModelBaseAdvancedTests
{
    #region ### Tests ###
    /// <summary>
    /// Verifies that validation invalidation does not mark an editor as dirty.
    /// </summary>
    [Fact]
    public async Task InvalidateProperties_ShouldNotMarkEditorDirty()
    {
        TestEditorViewModel viewModel = new()
        {
            ValidationDelay = TimeSpan.Zero,
            ValidateOnPropertyChanged = true
        };

        viewModel.MarkClean();
        viewModel.TriggerValueInvalidation();

        await TestAsyncHelper.WaitUntilAsync(() => viewModel.ValidationRunCount > 0);

        Assert.False(viewModel.IsDirty);
    }
    #endregion

    #region ### Private Classes ###
    private sealed class TestEditorViewModel : XEditorViewModelBase
    {
        #region ### Fields ###
        private int validationRunCount;
        private string? value;
        #endregion

        #region ### Public Properties ###
        public int ValidationRunCount => Volatile.Read(ref this.validationRunCount);

        public string? Value
        {
            get => this.value;
            set => this.SetProperty(ref this.value, value);
        }
        #endregion

        #region ### Public Methods ###
        public void TriggerValueInvalidation()
        {
            this.InvalidateProperties(nameof(this.Value));
        }
        #endregion

        #region ### Protected Methods ###
        protected override Task ValidateCoreAsync(XValidationContext context, CancellationToken cancellationToken)
        {
            _ = context;
            _ = cancellationToken;

            Interlocked.Increment(ref this.validationRunCount);
            return Task.CompletedTask;
        }
        #endregion
    }
    #endregion
}
#endregion
