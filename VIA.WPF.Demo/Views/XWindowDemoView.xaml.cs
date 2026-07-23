// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XWindowDemoView.xaml.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;

namespace VIA.WPF.Demo.Views;

#region ### Class XWindowDemoView ###
/// <summary>
/// Interaction logic for the XWindow showcase page.
/// </summary>
public partial class XWindowDemoView
{
    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="XWindowDemoView"/> class.
    /// </summary>
    public XWindowDemoView()
    {
        this.InitializeComponent();
    }
    #endregion

    #region ### Private Methods ###
    private void OpenDefaultWpfDemo_Click(object sender, RoutedEventArgs e)
    {
        DemoDefaultWPF window = new()
        {
            Owner = Window.GetWindow(this),
        };

        window.Show();
    }
    #endregion
}
#endregion
