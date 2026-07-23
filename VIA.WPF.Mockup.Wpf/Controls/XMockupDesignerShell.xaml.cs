// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XMockupDesignerShell.xaml.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows.Controls;
using VIA.WPF.Mockup.Wpf.ViewModels;

namespace VIA.WPF.Mockup.Wpf.Controls;

/// <summary>
/// Interaction logic for the reusable mockup designer application shell.
/// </summary>
public partial class XMockupDesignerShell : UserControl
{
    public XMockupDesignerShell()
    {
        InitializeComponent();
        DataContext = new MockupDesignerShellViewModel();
    }
}
