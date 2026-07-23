// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XMasterDetailSplitViewDemoView.xaml.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows.Input;
using VIA.WPF.Demo.ViewModels;

namespace VIA.WPF.Demo.Views;

#region ### Class XMasterDetailSplitViewDemoView ###

/// <summary>
/// Interaction logic for the XMasterDetailSplitView showcase page.
/// </summary>
public partial class XMasterDetailSplitViewDemoView
{
    #region ### Constructors ###

    /// <summary>
    /// Initializes a new instance of the <see cref="XMasterDetailSplitViewDemoView"/> class.
    /// </summary>
    public XMasterDetailSplitViewDemoView()
    {
        this.InitializeComponent();
    }

    #endregion

    #region ### Private Methods ###

    /// <summary>
    /// Opens the detail pane when an article row is double-clicked.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event arguments.</param>
    private void OnArticlesGridMouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (this.DataContext is XMasterDetailSplitViewDemoViewModel viewModel
            && viewModel.OpenDetailsCommand.CanExecute(null))
        {
            viewModel.OpenDetailsCommand.Execute(null);
        }
    }

    #endregion
}

#endregion
