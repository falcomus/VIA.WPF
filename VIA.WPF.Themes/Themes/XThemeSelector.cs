// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XThemeSelector.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace VIA.WPF.Themes;

#region ### Class XThemeSelector ###
/// <summary>
/// Represents a combo box that lists the registered VIA.WPF themes and applies the selected theme.
/// </summary>
public class XThemeSelector : ComboBox
{
    #region ### Private Fields ###
    private bool _isThemeManagerSubscribed;
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes static members of the <see cref="XThemeSelector"/> class.
    /// </summary>
    static XThemeSelector()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(XThemeSelector),
            new FrameworkPropertyMetadata(typeof(XThemeSelector)));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="XThemeSelector"/> class.
    /// </summary>
    public XThemeSelector()
    {
        this.DisplayMemberPath = nameof(XTheme.Name);

        this.Loaded += OnLoaded;
        this.Unloaded += OnUnloaded;
        this.SelectionChanged += OnSelectionChanged;
    }
    #endregion

    #region ### Private Methods ###
    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        XThemeService.EnsureBuiltInThemesRegistered();

        this.ItemsSource ??= XThemeRegistry.Current.Themes
            .OrderBy(theme => ReferenceEquals(theme, XThemePresets.Default) ? 0 : 1)
            .ThenBy(theme => theme.Name)
            .ToList();

        this.SelectedItem = XThemeManager.Current.CurrentTheme;

        if (!this._isThemeManagerSubscribed)
        {
            XThemeManager.Current.PropertyChanged += OnThemeManagerPropertyChanged;
            this._isThemeManagerSubscribed = true;
        }
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        if (!this._isThemeManagerSubscribed)
        {
            return;
        }

        XThemeManager.Current.PropertyChanged -= OnThemeManagerPropertyChanged;
        this._isThemeManagerSubscribed = false;
    }

    private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (this.SelectedItem is XTheme selectedTheme &&
            !ReferenceEquals(selectedTheme, XThemeManager.Current.CurrentTheme))
        {
            XThemeService.ChangeTheme(selectedTheme);
        }
    }

    private void OnThemeManagerPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(XThemeManager.CurrentTheme) &&
            !ReferenceEquals(this.SelectedItem, XThemeManager.Current.CurrentTheme))
        {
            this.SelectedItem = XThemeManager.Current.CurrentTheme;
        }
    }
    #endregion
}
#endregion
