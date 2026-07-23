// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MockupDesignerShellViewModel.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using CommunityToolkit.Mvvm.ComponentModel;

namespace VIA.WPF.Mockup.Wpf.ViewModels;

/// <summary>
/// Provides the root state for the reusable mockup designer shell.
/// </summary>
public sealed partial class MockupDesignerShellViewModel : ObservableObject
{
    public MockupDesignerShellViewModel()
    {
        Project = new MockupProjectViewModel();
    }

    public MockupProjectViewModel Project { get; }
}
