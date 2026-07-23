// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XNavigationListSideContent.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.ObjectModel;

namespace VIA.WPF.Controls.Navigation;

#region ### Class XNavigationListSideContent ###
/// <summary>
/// Represents standard side content that displays navigation entries in an <see cref="VIA.WPF.Controls.XNavigationList"/>.
/// </summary>
public sealed class XNavigationListSideContent : XNotifyPropertyChangedObject
{
    #region ### Fields ###
    private XNavigationEntry? selectedItem;
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="XNavigationListSideContent"/> class.
    /// </summary>
    public XNavigationListSideContent()
    {
        this.Items = new ObservableCollection<XNavigationEntry>();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="XNavigationListSideContent"/> class.
    /// </summary>
    /// <param name="items">The navigation entries.</param>
    public XNavigationListSideContent(IEnumerable<XNavigationEntry> items)
        : this()
    {
        foreach (XNavigationEntry item in items)
        {
            this.Items.Add(item);
        }

        this.SelectedItem = this.Items.FirstOrDefault();
    }
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets the navigation entries.
    /// </summary>
    public ObservableCollection<XNavigationEntry> Items { get; }

    /// <summary>
    /// Gets or sets the selected navigation entry.
    /// </summary>
    public XNavigationEntry? SelectedItem
    {
        get => this.selectedItem;
        set => this.SetProperty(ref this.selectedItem, value);
    }
    #endregion
}
#endregion