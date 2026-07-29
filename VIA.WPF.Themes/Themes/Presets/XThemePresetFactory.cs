// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XThemePresetFactory.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows.Media;

namespace VIA.WPF.Themes;

#region ### Class XThemePresetFactory ###
/// <summary>
/// Creates complete VIA.WPF themes from compact palette seed definitions.
/// </summary>
internal static class XThemePresetFactory
{
    #region ### Public Methods ###
    /// <summary>
    /// Creates a complete theme from the specified palette.
    /// </summary>
    /// <param name="palette">The source palette.</param>
    /// <returns>The generated theme.</returns>
    public static XTheme Create(XThemePalette palette)
    {
        ArgumentNullException.ThrowIfNull(palette);

        Color backgroundTextLight = Color.FromRgb(24, 30, 38);
        Color backgroundTextDark = Color.FromRgb(238, 241, 245);
        Color surfaceTextLight = Color.FromRgb(28, 35, 44);
        Color surfaceTextDark = Color.FromRgb(242, 244, 247);
        Color secondaryTextLight = Color.FromRgb(82, 96, 109);
        Color secondaryTextDark = Color.FromRgb(180, 187, 196);

        // Material-style depth starts with a tinted canvas and increasingly prominent
        // containers. Keep the main surface close to its palette seed so cards and panels
        // remain visibly distinct from the canvas in every preset.
        Color surfaceLight = Mix(palette.BackgroundLight, palette.SurfaceLight, 0.88d);
        Color surfaceDark = Mix(palette.BackgroundDark, palette.SurfaceDark, 0.88d);
        Color inputBackgroundLight = palette.SurfaceLight;
        Color inputBackgroundDark = Mix(surfaceDark, surfaceTextDark, 0.055d);

        Color controlBorderLight = Mix(inputBackgroundLight, secondaryTextLight, 0.42d);
        Color controlBorderDark = Mix(inputBackgroundDark, secondaryTextDark, 0.34d);
        Color controlBorderStrongLight = Mix(inputBackgroundLight, secondaryTextLight, 0.64d);
        Color controlBorderStrongDark = Mix(inputBackgroundDark, secondaryTextDark, 0.50d);

        Color panelBorderLight = Mix(surfaceLight, secondaryTextLight, 0.34d);
        Color panelBorderDark = Mix(surfaceDark, secondaryTextDark, 0.30d);
        Color panelBorderStrongLight = Mix(surfaceLight, secondaryTextLight, 0.52d);
        Color panelBorderStrongDark = Mix(surfaceDark, secondaryTextDark, 0.44d);

        Color hoverBackgroundLight = Mix(surfaceLight, palette.PrimaryLight, 0.065d);
        Color hoverBackgroundDark = Mix(surfaceDark, palette.PrimaryDark, 0.10d);
        Color pressedBackgroundLight = Mix(surfaceLight, palette.PrimaryLight, 0.14d);
        Color pressedBackgroundDark = Mix(surfaceDark, palette.PrimaryDark, 0.18d);

        Color navigationHeaderLight = Mix(palette.NavigationLight, palette.PrimaryLight, 0.18d);
        Color navigationHeaderDark = Mix(palette.NavigationDark, palette.PrimaryDark, 0.08d);
        Color navigationHoverLight = Mix(palette.NavigationLight, palette.PrimaryLight, 0.16d);
        Color navigationHoverDark = Mix(palette.NavigationDark, palette.PrimaryDark, 0.10d);
        Color navigationSelectedLight = Mix(palette.NavigationLight, palette.PrimaryLight, 0.30d);
        Color navigationSelectedDark = Mix(palette.NavigationDark, palette.PrimaryDark, 0.18d);

        Color navigationPanelHeaderBackgroundLight = Resolve(palette.NavigationPanelHeaderBackgroundLight, navigationHeaderLight);
        Color navigationPanelHeaderBackgroundDark = Resolve(palette.NavigationPanelHeaderBackgroundDark, navigationHeaderDark);
        Color navigationPanelBackgroundLight = Resolve(palette.NavigationPanelBackgroundLight, palette.NavigationLight);
        Color navigationPanelBackgroundDark = Resolve(palette.NavigationPanelBackgroundDark, palette.NavigationDark);
        Color navigationPanelItemSelectedBackgroundLight = Resolve(palette.NavigationPanelItemSelectedBackgroundLight, navigationSelectedLight);
        Color navigationPanelItemSelectedBackgroundDark = Resolve(palette.NavigationPanelItemSelectedBackgroundDark, navigationSelectedDark);
        Color navigationPanelBorderLight = Mix(navigationPanelBackgroundLight, Colors.White, 0.18d);
        Color navigationPanelBorderDark = Mix(navigationPanelBackgroundDark, Colors.White, 0.14d);

        return new XTheme
        {
            Name = palette.Name,

            ThemeModeForeground = Pair(palette.ThemeModeForegroundLight, palette.ThemeModeForegroundDark, palette.PrimaryLight, palette.PrimaryDark),
            ControlBorder = Pair(palette.ControlBorderLight, palette.ControlBorderDark, controlBorderLight, controlBorderDark),
            ControlBorderStrong = Pair(palette.ControlBorderStrongLight, palette.ControlBorderStrongDark, controlBorderStrongLight, controlBorderStrongDark),
            PanelBorder = Pair(palette.PanelBorderLight, palette.PanelBorderDark, panelBorderLight, panelBorderDark),
            PanelBorderStrong = Pair(palette.PanelBorderStrongLight, palette.PanelBorderStrongDark, panelBorderStrongLight, panelBorderStrongDark),
            FocusBorder = Pair(palette.FocusBorderLight, palette.FocusBorderDark, palette.PrimaryLight, palette.PrimaryDark),

            Primary = Semantic(palette.PrimaryLight, palette.PrimaryDark, palette.SurfaceLight, palette.SurfaceDark),
            Background = Neutral(palette.BackgroundLight, palette.BackgroundDark, backgroundTextLight, backgroundTextDark),
            Surface = Neutral(surfaceLight, surfaceDark, surfaceTextLight, surfaceTextDark),
            Border = Neutral(
                Resolve(palette.BorderLight, controlBorderLight),
                Resolve(palette.BorderDark, controlBorderDark),
                secondaryTextLight,
                secondaryTextDark),
            Accent = Semantic(palette.AccentLight, palette.AccentDark, palette.SurfaceLight, palette.SurfaceDark),
            Success = Semantic(palette.SuccessLight, palette.SuccessDark, palette.SurfaceLight, palette.SurfaceDark),
            Warning = Semantic(palette.WarningLight, palette.WarningDark, palette.SurfaceLight, palette.SurfaceDark),
            Danger = Semantic(palette.DangerLight, palette.DangerDark, palette.SurfaceLight, palette.SurfaceDark),
            Info = Semantic(palette.InfoLight, palette.InfoDark, palette.SurfaceLight, palette.SurfaceDark),

            SelectionBackground = Pair(
                palette.SelectionBackgroundLight,
                palette.SelectionBackgroundDark,
                Mix(inputBackgroundLight, palette.PrimaryLight, 0.14d),
                Mix(inputBackgroundDark, palette.PrimaryDark, 0.13d)),
            SelectionBorder = Pair(
                palette.SelectionBorderLight,
                palette.SelectionBorderDark,
                palette.PrimaryLight,
                palette.PrimaryDark),
            SelectionForeground = Pair(palette.SelectionForegroundLight, palette.SelectionForegroundDark, surfaceTextLight, surfaceTextDark),
            HoverBackground = Pair(palette.HoverBackgroundLight, palette.HoverBackgroundDark, hoverBackgroundLight, hoverBackgroundDark),
            HoverBorder = Pair(palette.HoverBorderLight, palette.HoverBorderDark, controlBorderStrongLight, controlBorderStrongDark),
            PressedBackground = Pair(palette.PressedBackgroundLight, palette.PressedBackgroundDark, pressedBackgroundLight, pressedBackgroundDark),
            PressedBorder = Pair(palette.PressedBorderLight, palette.PressedBorderDark, palette.PrimaryLight, palette.PrimaryDark),
            DisabledBackground = Pair(
                palette.DisabledBackgroundLight,
                palette.DisabledBackgroundDark,
                Mix(surfaceLight, palette.BackgroundLight, 0.52d),
                Mix(surfaceDark, palette.BackgroundDark, 0.44d)),
            DisabledForeground = Pair(
                palette.DisabledForegroundLight,
                palette.DisabledForegroundDark,
                Mix(inputBackgroundLight, secondaryTextLight, 0.74d),
                Mix(inputBackgroundDark, secondaryTextDark, 0.50d)),
            DisabledBorder = Pair(
                palette.DisabledBorderLight,
                palette.DisabledBorderDark,
                Mix(inputBackgroundLight, secondaryTextLight, 0.20d),
                Mix(inputBackgroundDark, secondaryTextDark, 0.18d)),
            GridLine = Pair(
                palette.GridLineLight,
                palette.GridLineDark,
                Mix(surfaceLight, secondaryTextLight, 0.13d),
                Mix(surfaceDark, secondaryTextDark, 0.14d)),
            GridHeaderBackground = Pair(
                palette.GridHeaderBackgroundLight,
                palette.GridHeaderBackgroundDark,
                surfaceLight,
                surfaceDark),
            GridHeaderForeground = Pair(palette.GridHeaderForegroundLight, palette.GridHeaderForegroundDark, surfaceTextLight, surfaceTextDark),
            InputBackground = Pair(palette.InputBackgroundLight, palette.InputBackgroundDark, inputBackgroundLight, inputBackgroundDark),
            InputBorder = Pair(palette.InputBorderLight, palette.InputBorderDark, controlBorderLight, controlBorderDark),
            InputPlaceholder = Pair(palette.InputPlaceholderLight, palette.InputPlaceholderDark, secondaryTextLight, secondaryTextDark),
            InputReadOnlyBackground = Pair(
                palette.InputReadOnlyBackgroundLight,
                palette.InputReadOnlyBackgroundDark,
                Mix(inputBackgroundLight, palette.BackgroundLight, 0.22d),
                Mix(inputBackgroundDark, palette.BackgroundDark, 0.32d)),

            NavigationPanelHeaderBackground = Pair(navigationPanelHeaderBackgroundLight, navigationPanelHeaderBackgroundDark),
            NavigationPanelHeaderForeground = Pair(
                palette.NavigationPanelHeaderForegroundLight,
                palette.NavigationPanelHeaderForegroundDark,
                Readable(navigationPanelHeaderBackgroundLight),
                Readable(navigationPanelHeaderBackgroundDark)),
            NavigationPanelHeaderBorder = Pair(
                palette.NavigationPanelHeaderBorderLight,
                palette.NavigationPanelHeaderBorderDark,
                WithAlpha(Colors.White, 40),
                WithAlpha(Colors.White, 26)),
            ToolbarBackground = Pair(palette.ToolbarBackgroundLight, palette.ToolbarBackgroundDark, surfaceLight, surfaceDark),
            ToolbarForeground = Pair(palette.ToolbarForegroundLight, palette.ToolbarForegroundDark, surfaceTextLight, surfaceTextDark),
            ToolbarSecondaryForeground = Pair(palette.ToolbarSecondaryForegroundLight, palette.ToolbarSecondaryForegroundDark, secondaryTextLight, secondaryTextDark),
            ToolbarBorder = Pair(
                palette.ToolbarBorderLight,
                palette.ToolbarBorderDark,
                WithAlpha(controlBorderStrongLight, 80),
                WithAlpha(controlBorderStrongDark, 72)),
            BreadcrumbBackground = Pair(
                palette.BreadcrumbBackgroundLight,
                palette.BreadcrumbBackgroundDark,
                Mix(palette.BackgroundLight, surfaceLight, 0.68d),
                Mix(palette.BackgroundDark, surfaceDark, 0.68d)),
            BreadcrumbForeground = Pair(palette.BreadcrumbForegroundLight, palette.BreadcrumbForegroundDark, surfaceTextLight, surfaceTextDark),
            BreadcrumbSecondaryForeground = Pair(palette.BreadcrumbSecondaryForegroundLight, palette.BreadcrumbSecondaryForegroundDark, secondaryTextLight, secondaryTextDark),

            TabHeaderBackground = Pair(palette.TabHeaderBackgroundLight, palette.TabHeaderBackgroundDark, surfaceLight, surfaceDark),
            TabHeaderForeground = Pair(palette.TabHeaderForegroundLight, palette.TabHeaderForegroundDark, surfaceTextLight, surfaceTextDark),
            TabHeaderBorder = Pair(palette.TabHeaderBorderLight, palette.TabHeaderBorderDark, controlBorderLight, controlBorderDark),
            TabItemBackground = Pair(palette.TabItemBackgroundLight, palette.TabItemBackgroundDark, Colors.Transparent, Colors.Transparent),
            TabItemBackgroundHover = Pair(palette.TabItemBackgroundHoverLight, palette.TabItemBackgroundHoverDark, hoverBackgroundLight, hoverBackgroundDark),
            TabItemBackgroundSelected = Pair(palette.TabItemBackgroundSelectedLight, palette.TabItemBackgroundSelectedDark, inputBackgroundLight, inputBackgroundDark),
            TabItemForeground = Pair(palette.TabItemForegroundLight, palette.TabItemForegroundDark, secondaryTextLight, secondaryTextDark),
            TabItemForegroundHover = Pair(palette.TabItemForegroundHoverLight, palette.TabItemForegroundHoverDark, surfaceTextLight, surfaceTextDark),
            TabItemForegroundSelected = Pair(palette.TabItemForegroundSelectedLight, palette.TabItemForegroundSelectedDark, surfaceTextLight, surfaceTextDark),
            TabItemBorder = Pair(palette.TabItemBorderLight, palette.TabItemBorderDark, Colors.Transparent, Colors.Transparent),
            TabItemBorderHover = Pair(palette.TabItemBorderHoverLight, palette.TabItemBorderHoverDark, controlBorderLight, controlBorderDark),
            TabItemBorderSelected = Pair(palette.TabItemBorderSelectedLight, palette.TabItemBorderSelectedDark, controlBorderStrongLight, controlBorderStrongDark),
            TabItemUnderline = Pair(palette.TabItemUnderlineLight, palette.TabItemUnderlineDark, Colors.Transparent, Colors.Transparent),
            TabItemUnderlineSelected = Pair(palette.TabItemUnderlineSelectedLight, palette.TabItemUnderlineSelectedDark, palette.PrimaryLight, palette.PrimaryDark),
            TabContentBackground = Pair(palette.TabContentBackgroundLight, palette.TabContentBackgroundDark, surfaceLight, surfaceDark),
            TabContentBorder = Pair(palette.TabContentBorderLight, palette.TabContentBorderDark, controlBorderLight, controlBorderDark),
            TabActionButtonForeground = Pair(palette.TabActionButtonForegroundLight, palette.TabActionButtonForegroundDark, secondaryTextLight, secondaryTextDark),
            TabActionButtonForegroundHover = Pair(palette.TabActionButtonForegroundHoverLight, palette.TabActionButtonForegroundHoverDark, surfaceTextLight, surfaceTextDark),
            TabActionButtonBackgroundHover = Pair(palette.TabActionButtonBackgroundHoverLight, palette.TabActionButtonBackgroundHoverDark, hoverBackgroundLight, hoverBackgroundDark),

            NavigationPanelBackground = Pair(navigationPanelBackgroundLight, navigationPanelBackgroundDark),
            NavigationPanelForeground = Pair(
                palette.NavigationPanelForegroundLight,
                palette.NavigationPanelForegroundDark,
                Readable(navigationPanelBackgroundLight),
                Readable(navigationPanelBackgroundDark)),
            NavigationPanelBorder = Pair(
                palette.NavigationPanelBorderLight,
                palette.NavigationPanelBorderDark,
                navigationPanelBorderLight,
                navigationPanelBorderDark),
            NavigationPanelItemHoverBackground = Pair(palette.NavigationPanelItemHoverBackgroundLight, palette.NavigationPanelItemHoverBackgroundDark, navigationHoverLight, navigationHoverDark),
            NavigationPanelItemSelectedBackground = Pair(navigationPanelItemSelectedBackgroundLight, navigationPanelItemSelectedBackgroundDark),
            NavigationPanelItemSelectedForeground = Pair(
                palette.NavigationPanelItemSelectedForegroundLight,
                palette.NavigationPanelItemSelectedForegroundDark,
                Readable(navigationPanelItemSelectedBackgroundLight),
                Readable(navigationPanelItemSelectedBackgroundDark))
        };
    }
    #endregion

    #region ### Private Methods ###
    private static XThemeColorSet Semantic(Color light, Color dark, Color lightSurface, Color darkSurface)
    {
        return XThemeColorUtility.CreateSemanticColorSet(light, dark, lightSurface, darkSurface);
    }

    private static XThemeColorSet Neutral(Color light, Color dark, Color textLight, Color textDark)
    {
        return XThemeColorUtility.CreateNeutralColorSet(light, dark, textLight, textDark);
    }

    private static XThemeModeColor Pair(Color light, Color dark)
    {
        return XThemeColorUtility.CreateModeColor(light, dark);
    }

    private static XThemeModeColor Pair(Color? lightOverride, Color? darkOverride, Color light, Color dark)
    {
        return Pair(Resolve(lightOverride, light), Resolve(darkOverride, dark));
    }

    private static Color Resolve(Color? colorOverride, Color fallback)
    {
        return colorOverride ?? fallback;
    }

    private static Color Mix(Color from, Color to, double amount)
    {
        return XThemeColorUtility.Mix(from, to, amount);
    }

    private static Color Readable(Color background)
    {
        return XThemeColorUtility.GetReadableForeground(background);
    }

    private static Color WithAlpha(Color color, byte alpha)
    {
        return Color.FromArgb(alpha, color.R, color.G, color.B);
    }
    #endregion
}
#endregion
