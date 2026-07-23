// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using VIA.WPF.Demo.ViewModels;

namespace VIA.WPF.Demo;

#region ### Class MainWindow ###
/// <summary>
/// Interaction logic for the main demo window.
/// </summary>
public partial class MainWindow
{
    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="MainWindow"/> class.
    /// </summary>
    public MainWindow()
    {
        this.InitializeComponent();
        this.DataContext = new MainWindowViewModel();
    }
    #endregion

}
#endregion

