// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XThemeManager.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;

namespace VIA.WPF.Themes;

#region ### Class XThemeManager ###
/// <summary>
/// Manages the active <see cref="XTheme"/> and the current light or dark mode
/// and exposes the related runtime resources for VIA.WPF.
/// </summary>
public sealed class XThemeManager : INotifyPropertyChanged
{
    #region ### Private Constants ###
    private const string ThemeDictionaryMarkerKey = "VIA.WPF.Theme.Runtime.Marker";
    #endregion

    #region ### Private Fields ###
    private XThemeMode _currentMode;
    private XTheme? _currentTheme;
    private ResourceDictionary? _resourceDictionary;
    #endregion

    #region ### Constructors ###
    /// <summary>
    /// Initializes a new instance of the <see cref="XThemeManager"/> class.
    /// </summary>
    public XThemeManager()
    {
        this.TransitionDuration = TimeSpan.FromMilliseconds(600);
        this._currentMode = XThemeMode.Light;
    }
    #endregion

    #region ### Public Events ###
    /// <summary>
    /// Occurs when a public property value changes.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;
    #endregion

    #region ### Public Properties ###
    /// <summary>
    /// Gets the currently applied theme.
    /// </summary>
    public XTheme? CurrentTheme
    {
        get => this._currentTheme;
        private set
        {
            if (ReferenceEquals(this._currentTheme, value))
            {
                return;
            }

            this._currentTheme = value;
            this.OnPropertyChanged();
        }
    }

    /// <summary>
    /// Gets the currently applied theme mode.
    /// </summary>
    public XThemeMode CurrentMode
    {
        get => this._currentMode;
        private set
        {
            if (this._currentMode == value)
            {
                return;
            }

            this._currentMode = value;
            this.OnPropertyChanged();
        }
    }

    /// <summary>
    /// Gets or sets the default visual theme transition duration.
    /// </summary>
    public TimeSpan TransitionDuration { get; set; }

    /// <summary>
    /// Gets the global default instance of the theme manager.
    /// </summary>
    public static XThemeManager Current { get; } = new();
    #endregion

    #region ### Public Methods ###
    /// <summary>
    /// Applies the specified theme to the current application.
    /// </summary>
    /// <param name="theme">The theme to apply.</param>
    public void ApplyTheme(XTheme theme)
    {
        ArgumentNullException.ThrowIfNull(theme);

        Application application = Application.Current
                                  ?? throw new InvalidOperationException("No current WPF application is available.");

        this.ApplyTheme(theme, application.Resources);
    }

    /// <summary>
    /// Applies the specified theme to the specified resource dictionary.
    /// </summary>
    /// <param name="theme">The theme to apply.</param>
    /// <param name="resources">The target resource dictionary.</param>
    public void ApplyTheme(XTheme theme, ResourceDictionary resources)
    {
        ArgumentNullException.ThrowIfNull(theme);
        ArgumentNullException.ThrowIfNull(resources);

        EnsureGlobalInfrastructureResources(resources);

        this._resourceDictionary = GetOrCreateThemeDictionary(resources);
        this.CurrentTheme = theme;

        this.ApplyCurrentState();
    }

    /// <summary>
    /// Sets the active theme mode and updates the resources.
    /// </summary>
    /// <param name="mode">The target mode.</param>
    public void SetMode(XThemeMode mode)
    {
        if (this.CurrentMode == mode)
        {
            return;
        }

        this.CurrentMode = mode;
        this.ApplyCurrentState();
    }

    /// <summary>
    /// Toggles between light mode and dark mode.
    /// </summary>
    public void ToggleMode()
    {
        this.SetMode(this.CurrentMode == XThemeMode.Light ? XThemeMode.Dark : XThemeMode.Light);
    }
    #endregion

    #region ### Private Methods ###
    private void ApplyCurrentState()
    {
        if (this.CurrentTheme is null || this._resourceDictionary is null)
        {
            return;
        }

        this.ApplyModeColor(this.CurrentTheme.ThemeModeForeground, XBrushKeys.ThemeModeForeground);
        this.ApplyModeColor(this.CurrentTheme.ControlBorder, XBrushKeys.ControlBorder);
        this.ApplyModeColor(this.CurrentTheme.ControlBorderStrong, XBrushKeys.ControlBorderStrong);
        this.ApplyModeColor(this.CurrentTheme.PanelBorder, XBrushKeys.PanelBorder);
        this.ApplyModeColor(this.CurrentTheme.PanelBorderStrong, XBrushKeys.PanelBorderStrong);
        this.ApplyModeColor(this.CurrentTheme.FocusBorder, XBrushKeys.FocusBorder);

        this.ApplyModeColor(this.CurrentTheme.SelectionBackground, XBrushKeys.SelectionBackground);
        this.ApplyModeColor(this.CurrentTheme.SelectionBorder, XBrushKeys.SelectionBorder);
        this.ApplyModeColor(this.CurrentTheme.SelectionForeground, XBrushKeys.SelectionForeground);
        this.ApplyModeColor(this.CurrentTheme.HoverBackground, XBrushKeys.HoverBackground);
        this.ApplyModeColor(this.CurrentTheme.HoverBorder, XBrushKeys.HoverBorder);
        this.ApplyModeColor(this.CurrentTheme.PressedBackground, XBrushKeys.PressedBackground);
        this.ApplyModeColor(this.CurrentTheme.PressedBorder, XBrushKeys.PressedBorder);
        this.ApplyModeColor(this.CurrentTheme.DisabledBackground, XBrushKeys.DisabledBackground);
        this.ApplyModeColor(this.CurrentTheme.DisabledForeground, XBrushKeys.DisabledForeground);
        this.ApplyModeColor(this.CurrentTheme.DisabledBorder, XBrushKeys.DisabledBorder);
        this.ApplyModeColor(this.CurrentTheme.GridLine, XBrushKeys.GridLine);
        this.ApplyModeColor(this.CurrentTheme.GridHeaderBackground, XBrushKeys.GridHeaderBackground);
        this.ApplyModeColor(this.CurrentTheme.GridHeaderForeground, XBrushKeys.GridHeaderForeground);
        this.ApplyModeColor(this.CurrentTheme.InputBackground, XBrushKeys.InputBackground);
        this.ApplyModeColor(this.CurrentTheme.InputBorder, XBrushKeys.InputBorder);
        this.ApplyModeColor(this.CurrentTheme.InputPlaceholder, XBrushKeys.InputPlaceholder);
        this.ApplyModeColor(this.CurrentTheme.InputReadOnlyBackground, XBrushKeys.InputReadOnlyBackground);

        this.ApplyColorSet(this.CurrentTheme.Primary, XBrushKeys.Primary, XBrushKeys.PrimaryText, XBrushKeys.PrimaryVeryLight, XBrushKeys.PrimaryLight, XBrushKeys.PrimaryDark);
        this.ApplyColorSet(this.CurrentTheme.Background, XBrushKeys.Background, XBrushKeys.BackgroundText, XBrushKeys.BackgroundLight, XBrushKeys.BackgroundLight, XBrushKeys.BackgroundDark);
        this.ApplyColorSet(this.CurrentTheme.Surface, XBrushKeys.Surface, XBrushKeys.SurfaceText, XBrushKeys.SurfaceVeryLight, XBrushKeys.SurfaceLight, XBrushKeys.SurfaceDark);
        this.ApplyColorSet(this.CurrentTheme.Border, XBrushKeys.Border, XBrushKeys.BorderText, XBrushKeys.BorderLight, XBrushKeys.BorderLight, XBrushKeys.BorderDark);
        this.ApplyColorSet(this.CurrentTheme.Accent, XBrushKeys.Accent, XBrushKeys.AccentText, XBrushKeys.AccentVeryLight, XBrushKeys.AccentLight, XBrushKeys.AccentDark);
        this.ApplyColorSet(this.CurrentTheme.Success, XBrushKeys.Success, XBrushKeys.SuccessText, XBrushKeys.SuccessVeryLight, XBrushKeys.SuccessLight, XBrushKeys.SuccessDark);
        this.ApplyColorSet(this.CurrentTheme.Warning, XBrushKeys.Warning, XBrushKeys.WarningText, XBrushKeys.WarningVeryLight, XBrushKeys.WarningLight, XBrushKeys.WarningDark);
        this.ApplyColorSet(this.CurrentTheme.Danger, XBrushKeys.Danger, XBrushKeys.DangerText, XBrushKeys.DangerVeryLight, XBrushKeys.DangerLight, XBrushKeys.DangerDark);
        this.ApplyColorSet(this.CurrentTheme.Info, XBrushKeys.Info, XBrushKeys.InfoText, XBrushKeys.InfoVeryLight, XBrushKeys.InfoLight, XBrushKeys.InfoDark);

        this.ApplyCanonicalSemanticBrushes();

        this.ApplyModeColor(this.CurrentTheme.TabHeaderBackground, XBrushKeys.TabHeaderBackground);
        this.ApplyModeColor(this.CurrentTheme.TabHeaderForeground, XBrushKeys.TabHeaderForeground);
        this.ApplyModeColor(this.CurrentTheme.TabHeaderBorder, XBrushKeys.TabHeaderBorder);

        this.ApplyModeColor(this.CurrentTheme.TabItemBackground, XBrushKeys.TabItemBackground);
        this.ApplyModeColor(this.CurrentTheme.TabItemBackgroundHover, XBrushKeys.TabItemBackgroundHover);
        this.ApplyModeColor(this.CurrentTheme.TabItemBackgroundSelected, XBrushKeys.TabItemBackgroundSelected);

        this.ApplyModeColor(this.CurrentTheme.TabItemForeground, XBrushKeys.TabItemForeground);
        this.ApplyModeColor(this.CurrentTheme.TabItemForegroundHover, XBrushKeys.TabItemForegroundHover);
        this.ApplyModeColor(this.CurrentTheme.TabItemForegroundSelected, XBrushKeys.TabItemForegroundSelected);

        this.ApplyModeColor(this.CurrentTheme.TabItemBorder, XBrushKeys.TabItemBorder);
        this.ApplyModeColor(this.CurrentTheme.TabItemBorderHover, XBrushKeys.TabItemBorderHover);
        this.ApplyModeColor(this.CurrentTheme.TabItemBorderSelected, XBrushKeys.TabItemBorderSelected);

        this.ApplyModeColor(this.CurrentTheme.TabItemUnderline, XBrushKeys.TabItemUnderline);
        this.ApplyModeColor(this.CurrentTheme.TabItemUnderlineSelected, XBrushKeys.TabItemUnderlineSelected);

        this.ApplyModeColor(this.CurrentTheme.TabContentBackground, XBrushKeys.TabContentBackground);
        this.ApplyModeColor(this.CurrentTheme.TabContentBorder, XBrushKeys.TabContentBorder);

        this.ApplyModeColor(this.CurrentTheme.TabActionButtonForeground, XBrushKeys.TabActionButtonForeground);
        this.ApplyModeColor(this.CurrentTheme.TabActionButtonForegroundHover, XBrushKeys.TabActionButtonForegroundHover);
        this.ApplyModeColor(this.CurrentTheme.TabActionButtonBackgroundHover, XBrushKeys.TabActionButtonBackgroundHover);

        this.ApplyModeColor(this.CurrentTheme.NavigationPanelBackground, XBrushKeys.NavigationPanelBackground);
        this.ApplyModeColor(this.CurrentTheme.NavigationPanelForeground, XBrushKeys.NavigationPanelForeground);
        this.ApplyModeColor(this.CurrentTheme.NavigationPanelBorder, XBrushKeys.NavigationPanelBorder);
        this.ApplyModeColor(this.CurrentTheme.NavigationPanelItemHoverBackground, XBrushKeys.NavigationPanelItemHoverBackground);
        this.ApplyModeColor(this.CurrentTheme.NavigationPanelItemSelectedBackground, XBrushKeys.NavigationPanelSelectedItemBackground);
        this.ApplyModeColor(this.CurrentTheme.NavigationPanelItemSelectedForeground, XBrushKeys.NavigationPanelSelectedItemForeground);

        this.ApplyModeColor(this.CurrentTheme.NavigationPanelHeaderBackground, XBrushKeys.NavigationPanelHeaderBackground);
        this.ApplyModeColor(this.CurrentTheme.NavigationPanelHeaderForeground, XBrushKeys.NavigationPanelHeaderForeground);
        this.ApplyModeColor(this.CurrentTheme.NavigationPanelHeaderBorder, XBrushKeys.NavigationPanelHeaderBorder);
        this.ApplyModeColor(this.CurrentTheme.ToolbarBackground, XBrushKeys.ToolbarBackground);
        this.ApplyModeColor(this.CurrentTheme.ToolbarForeground, XBrushKeys.ToolbarForeground);
        this.ApplyModeColor(this.CurrentTheme.ToolbarSecondaryForeground, XBrushKeys.ToolbarSecondaryForeground);
        this.ApplyModeColor(this.CurrentTheme.ToolbarBorder, XBrushKeys.ToolbarBorder);
        this.ApplyModeColor(this.CurrentTheme.BreadcrumbBackground, XBrushKeys.BreadcrumbBackground);
        this.ApplyModeColor(this.CurrentTheme.BreadcrumbForeground, XBrushKeys.BreadcrumbForeground);
        this.ApplyModeColor(this.CurrentTheme.BreadcrumbSecondaryForeground, XBrushKeys.BreadcrumbSecondaryForeground);

        this.ApplySemanticContractBrushes();
    }

    private void ApplySemanticContractBrushes()
    {
        if (this.CurrentTheme is null)
        {
            return;
        }

        XTheme theme = this.CurrentTheme;
        XThemeMode mode = this.CurrentMode;

        this.ApplyBrush(XBrushKeys.Canvas, theme.Background.GetBaseColor(mode));
        this.ApplyBrush(XBrushKeys.SurfaceRaised, theme.Surface.GetVeryLightVariantColor(mode));
        this.ApplyBrush(XBrushKeys.SurfaceSunken, theme.Background.GetDarkVariantColor(mode));

        this.ApplyBrush(XBrushKeys.TextPrimary, theme.Surface.GetTextColor(mode));
        this.ApplyBrush(XBrushKeys.TextSecondary, theme.Border.GetTextColor(mode));
        this.ApplyBrush(XBrushKeys.TextTertiary, theme.InputPlaceholder.GetColor(mode));

        this.ApplyBrush(XBrushKeys.BorderSubtle, theme.PanelBorder.GetColor(mode));
        this.ApplyBrush(XBrushKeys.BorderDefault, theme.ControlBorder.GetColor(mode));
        this.ApplyBrush(XBrushKeys.BorderStrong, theme.ControlBorderStrong.GetColor(mode));
        this.ApplyBrush(XBrushKeys.FocusRing, theme.FocusBorder.GetColor(mode));
        this.ApplyBrush(XBrushKeys.FocusRingInner, theme.Surface.GetBaseColor(mode));

        this.ApplyBrush(XBrushKeys.StateHover, theme.HoverBackground.GetColor(mode));
        this.ApplyBrush(XBrushKeys.StatePressed, theme.PressedBackground.GetColor(mode));
        this.ApplyBrush(XBrushKeys.StateSelected, theme.SelectionBackground.GetColor(mode));
        this.ApplyBrush(XBrushKeys.StateSelectedStrong, theme.Primary.GetBaseColor(mode));
        this.ApplyBrush(XBrushKeys.Scrim, mode == XThemeMode.Dark
            ? Color.FromArgb(176, 0, 0, 0)
            : Color.FromArgb(112, 0, 0, 0));

        this.ApplyBrush(XBrushKeys.CommandBarBackground, theme.ToolbarBackground.GetColor(mode));
        this.ApplyBrush(XBrushKeys.CommandBarForeground, theme.ToolbarForeground.GetColor(mode));
        this.ApplyBrush(XBrushKeys.CommandBarHoverBackground, theme.HoverBackground.GetColor(mode));
        this.ApplyBrush(XBrushKeys.CommandBarPressedBackground, theme.PressedBackground.GetColor(mode));
        this.ApplyBrush(XBrushKeys.CommandBarGroupHeaderBackground, theme.NavigationPanelHeaderBackground.GetColor(mode));
        this.ApplyBrush(XBrushKeys.NavigationSelectionIndicator, theme.Primary.GetBaseColor(mode));
    }

    private void ApplyCanonicalSemanticBrushes()
    {
        if (this.CurrentTheme is null)
        {
            return;
        }

        XTheme theme = this.CurrentTheme;
        XThemeMode mode = this.CurrentMode;

        this.ApplyBrush(XBrushKeys.PrimaryForeground, theme.Primary.GetTextColor(mode));
        this.ApplyBrush(XBrushKeys.PrimarySubtle, theme.Primary.GetVeryLightVariantColor(mode));
        this.ApplyBrush(XBrushKeys.PrimarySubtleHover, theme.Primary.GetLightVariantColor(mode));
        this.ApplyBrush(XBrushKeys.PrimaryStrong, theme.Primary.GetDarkVariantColor(mode));

        this.ApplyBrush(XBrushKeys.AccentForeground, theme.Accent.GetTextColor(mode));
        this.ApplyBrush(XBrushKeys.AccentSubtle, theme.Accent.GetVeryLightVariantColor(mode));
        this.ApplyBrush(XBrushKeys.AccentSubtleHover, theme.Accent.GetLightVariantColor(mode));
        this.ApplyBrush(XBrushKeys.AccentStrong, theme.Accent.GetDarkVariantColor(mode));

        this.ApplyBrush(XBrushKeys.StatusSuccess, theme.Success.GetBaseColor(mode));
        this.ApplyBrush(XBrushKeys.StatusSuccessForeground, theme.Success.GetTextColor(mode));
        this.ApplyBrush(XBrushKeys.StatusSuccessSubtle, theme.Success.GetVeryLightVariantColor(mode));
        this.ApplyBrush(XBrushKeys.StatusWarning, theme.Warning.GetBaseColor(mode));
        this.ApplyBrush(XBrushKeys.StatusWarningForeground, theme.Warning.GetTextColor(mode));
        this.ApplyBrush(XBrushKeys.StatusWarningSubtle, theme.Warning.GetVeryLightVariantColor(mode));
        this.ApplyBrush(XBrushKeys.StatusDanger, theme.Danger.GetBaseColor(mode));
        this.ApplyBrush(XBrushKeys.StatusDangerForeground, theme.Danger.GetTextColor(mode));
        this.ApplyBrush(XBrushKeys.StatusDangerSubtle, theme.Danger.GetVeryLightVariantColor(mode));
        this.ApplyBrush(XBrushKeys.StatusInfo, theme.Info.GetBaseColor(mode));
        this.ApplyBrush(XBrushKeys.StatusInfoForeground, theme.Info.GetTextColor(mode));
        this.ApplyBrush(XBrushKeys.StatusInfoSubtle, theme.Info.GetVeryLightVariantColor(mode));

        this.ApplyBrush(XBrushKeys.SurfaceOverlay, theme.Surface.GetVeryLightVariantColor(mode));
        this.ApplyBrush(XBrushKeys.TextDisabled, theme.DisabledForeground.GetColor(mode));
        this.ApplyBrush(XBrushKeys.Divider, theme.PanelBorder.GetColor(mode));
    }

    private void ApplyColorSet(
        XThemeColorSet colorSet,
        ComponentResourceKey baseKey,
        ComponentResourceKey textKey,
        ComponentResourceKey veryLightKey,
        ComponentResourceKey lightKey,
        ComponentResourceKey darkKey)
    {
        this.ApplyBrush(baseKey, colorSet.GetBaseColor(this.CurrentMode));
        this.ApplyBrush(textKey, colorSet.GetTextColor(this.CurrentMode));
        this.ApplyBrush(veryLightKey, colorSet.GetVeryLightVariantColor(this.CurrentMode));
        this.ApplyBrush(lightKey, colorSet.GetLightVariantColor(this.CurrentMode));
        this.ApplyBrush(darkKey, colorSet.GetDarkVariantColor(this.CurrentMode));
    }

    private void ApplyModeColor(XThemeModeColor modeColor, ComponentResourceKey key)
    {
        this.ApplyBrush(key, modeColor.GetColor(this.CurrentMode));
    }

    private static ResourceDictionary GetOrCreateThemeDictionary(ResourceDictionary resources)
    {
        ResourceDictionary? existingDictionary = resources.MergedDictionaries
            .FirstOrDefault(dictionary =>
                dictionary.Contains(ThemeDictionaryMarkerKey) &&
                Equals(dictionary[ThemeDictionaryMarkerKey], true));

        if (existingDictionary is not null)
        {
            return existingDictionary;
        }

        ResourceDictionary dictionary = new()
        {
            [ThemeDictionaryMarkerKey] = true
        };

        resources.MergedDictionaries.Add(dictionary);

        return dictionary;
    }

    private static void EnsureGlobalInfrastructureResources(ResourceDictionary resources)
    {
        MergeGlobalInfrastructureDictionary(resources, "/VIA.WPF.Controls;component/Themes/XScrollBar.xaml");
    }

    private static void MergeGlobalInfrastructureDictionary(ResourceDictionary resources, string source)
    {
        Uri uri = new(source, UriKind.Relative);

        bool alreadyMerged = resources.MergedDictionaries.Any(dictionary => dictionary.Source == uri);
        if (alreadyMerged)
        {
            return;
        }

        resources.MergedDictionaries.Add(
            new ResourceDictionary
            {
                Source = uri
            });
    }

    private void ApplyBrush(ComponentResourceKey key, Color targetColor)
    {
        if (this._resourceDictionary is null)
        {
            return;
        }

        this._resourceDictionary[key] = XBrushFactory.CreateRuntimeBrush(targetColor);
    }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    #endregion

}
#endregion
