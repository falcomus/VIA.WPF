// --------------------------------------------------------------------------------------------------------------------
// <copyright file="App.xaml.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows;
using VIA.WPF.Themes;

namespace VIA.WPF.Demo;

#region ### Class App ###

/// <summary>
/// Represents the application instance of the sample app.
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // Initialize the theme service
        XThemeService.Initialize();

        // Set Graphite as the default theme
        //XThemeService.ChangeTheme("Graphite");

        // Alternatively, create and set a custom theme
        //BuildCustomTheme();

        // Optionally, switch to dark mode
        //XThemeService.ChangeThemeMode(XThemeMode.Dark);
    }


    #region === BUILD CUSTOM THEME ===

    //private static void BuildCustomTheme()
    //{
    //    // Create a custom theme
    //    XTheme customTheme = new()
    //    {
    //        Name = "My Custom Theme #01",

    //        ThemeModeForeground = new XThemeModeColor
    //        {
    //            Light = Color.FromRgb(24, 119, 242),
    //            Dark = Color.FromRgb(144, 191, 255)
    //        },

    //        ControlBorder = new XThemeModeColor
    //        {
    //            Light = Color.FromRgb(190, 200, 214),
    //            Dark = Color.FromRgb(66, 76, 90)
    //        },

    //        ControlBorderStrong = new XThemeModeColor
    //        {
    //            Light = Color.FromRgb(140, 154, 172),
    //            Dark = Color.FromRgb(90, 100, 116)
    //        },

    //        PanelBorder = new XThemeModeColor
    //        {
    //            Light = Color.FromRgb(178, 188, 204),
    //            Dark = Color.FromRgb(58, 68, 82)
    //        },

    //        PanelBorderStrong = new XThemeModeColor
    //        {
    //            Light = Color.FromRgb(132, 146, 166),
    //            Dark = Color.FromRgb(80, 92, 108)
    //        },

    //        FocusBorder = new XThemeModeColor
    //        {
    //            Light = Color.FromRgb(24, 119, 242),
    //            Dark = Color.FromRgb(100, 170, 255)
    //        },

    //        Primary = new XThemeColorSet
    //        {
    //            Light = Color.FromRgb(24, 119, 242),
    //            Dark = Color.FromRgb(100, 170, 255),
    //            TextLight = Colors.White,
    //            TextDark = Colors.Black,
    //            VeryLightVariantLight = Color.FromRgb(233, 242, 255),
    //            VeryLightVariantDark = Color.FromRgb(34, 46, 62),
    //            LightVariantLight = Color.FromRgb(100, 170, 255),
    //            LightVariantDark = Color.FromRgb(130, 186, 255),
    //            DarkVariantLight = Color.FromRgb(19, 88, 186),
    //            DarkVariantDark = Color.FromRgb(24, 119, 242)
    //        },

    //        Background = new XThemeColorSet
    //        {
    //            Light = Color.FromRgb(244, 247, 251),
    //            Dark = Color.FromRgb(18, 22, 28),
    //            TextLight = Color.FromRgb(27, 36, 48),
    //            TextDark = Color.FromRgb(233, 238, 245),
    //            VeryLightVariantLight = Colors.White,
    //            VeryLightVariantDark = Color.FromRgb(26, 31, 38),
    //            LightVariantLight = Colors.White,
    //            LightVariantDark = Color.FromRgb(31, 37, 46),
    //            DarkVariantLight = Color.FromRgb(229, 234, 241),
    //            DarkVariantDark = Color.FromRgb(11, 14, 19)
    //        },

    //        Surface = new XThemeColorSet
    //        {
    //            Light = Colors.White,
    //            Dark = Color.FromRgb(25, 31, 39),
    //            TextLight = Color.FromRgb(32, 41, 56),
    //            TextDark = Color.FromRgb(239, 243, 248),
    //            VeryLightVariantLight = Color.FromRgb(251, 252, 254),
    //            VeryLightVariantDark = Color.FromRgb(31, 37, 46),
    //            LightVariantLight = Color.FromRgb(247, 249, 252),
    //            LightVariantDark = Color.FromRgb(36, 44, 54),
    //            DarkVariantLight = Color.FromRgb(233, 238, 245),
    //            DarkVariantDark = Color.FromRgb(20, 25, 32)
    //        },

    //        Border = new XThemeColorSet
    //        {
    //            Light = Color.FromRgb(201, 210, 222),
    //            Dark = Color.FromRgb(66, 76, 90),
    //            TextLight = Color.FromRgb(107, 119, 136),
    //            TextDark = Color.FromRgb(154, 165, 180),
    //            VeryLightVariantLight = Color.FromRgb(239, 243, 248),
    //            VeryLightVariantDark = Color.FromRgb(76, 86, 100),
    //            LightVariantLight = Color.FromRgb(224, 230, 238),
    //            LightVariantDark = Color.FromRgb(84, 95, 110),
    //            DarkVariantLight = Color.FromRgb(164, 176, 192),
    //            DarkVariantDark = Color.FromRgb(52, 60, 72)
    //        },

    //        Accent = new XThemeColorSet
    //        {
    //            Light = Color.FromRgb(0, 163, 108),
    //            Dark = Color.FromRgb(70, 210, 158),
    //            TextLight = Colors.White,
    //            TextDark = Color.FromRgb(16, 28, 24),
    //            VeryLightVariantLight = Color.FromRgb(232, 249, 242),
    //            VeryLightVariantDark = Color.FromRgb(30, 49, 41),
    //            LightVariantLight = Color.FromRgb(70, 210, 158),
    //            LightVariantDark = Color.FromRgb(110, 223, 180),
    //            DarkVariantLight = Color.FromRgb(0, 124, 82),
    //            DarkVariantDark = Color.FromRgb(0, 163, 108)
    //        },

    //        Success = new XThemeColorSet
    //        {
    //            Light = Color.FromRgb(39, 174, 96),
    //            Dark = Color.FromRgb(82, 199, 132),
    //            TextLight = Colors.White,
    //            TextDark = Color.FromRgb(18, 30, 22),
    //            VeryLightVariantLight = Color.FromRgb(233, 248, 239),
    //            VeryLightVariantDark = Color.FromRgb(31, 47, 37),
    //            LightVariantLight = Color.FromRgb(82, 199, 132),
    //            LightVariantDark = Color.FromRgb(118, 215, 159),
    //            DarkVariantLight = Color.FromRgb(30, 135, 75),
    //            DarkVariantDark = Color.FromRgb(39, 174, 96)
    //        },

    //        Warning = new XThemeColorSet
    //        {
    //            Light = Color.FromRgb(242, 163, 58),
    //            Dark = Color.FromRgb(247, 191, 106),
    //            TextLight = Color.FromRgb(46, 36, 21),
    //            TextDark = Color.FromRgb(46, 36, 21),
    //            VeryLightVariantLight = Color.FromRgb(255, 247, 233),
    //            VeryLightVariantDark = Color.FromRgb(56, 47, 32),
    //            LightVariantLight = Color.FromRgb(247, 191, 106),
    //            LightVariantDark = Color.FromRgb(250, 209, 142),
    //            DarkVariantLight = Color.FromRgb(212, 136, 31),
    //            DarkVariantDark = Color.FromRgb(224, 151, 45)
    //        },

    //        Danger = new XThemeColorSet
    //        {
    //            Light = Color.FromRgb(235, 87, 87),
    //            Dark = Color.FromRgb(240, 126, 126),
    //            TextLight = Color.FromRgb(27, 36, 48),
    //            TextDark = Colors.White,
    //            VeryLightVariantLight = Color.FromRgb(255, 236, 236),
    //            VeryLightVariantDark = Color.FromRgb(61, 36, 36),
    //            LightVariantLight = Color.FromRgb(240, 126, 126),
    //            LightVariantDark = Color.FromRgb(245, 156, 156),
    //            DarkVariantLight = Color.FromRgb(201, 60, 60),
    //            DarkVariantDark = Color.FromRgb(220, 72, 72)
    //        },

    //        Info = new XThemeColorSet
    //        {
    //            Light = Color.FromRgb(45, 156, 219),
    //            Dark = Color.FromRgb(97, 183, 230),
    //            TextLight = Colors.White,
    //            TextDark = Color.FromRgb(18, 31, 42),
    //            VeryLightVariantLight = Color.FromRgb(234, 246, 252),
    //            VeryLightVariantDark = Color.FromRgb(31, 46, 56),
    //            LightVariantLight = Color.FromRgb(97, 183, 230),
    //            LightVariantDark = Color.FromRgb(132, 200, 238),
    //            DarkVariantLight = Color.FromRgb(31, 127, 181),
    //            DarkVariantDark = Color.FromRgb(45, 156, 219)
    //        },

    //        TabHeaderBackground = new XThemeModeColor { Light = Colors.White, Dark = Color.FromRgb(25, 31, 39) },
    //        TabHeaderForeground = new XThemeModeColor { Light = Color.FromRgb(32, 41, 56), Dark = Color.FromRgb(239, 243, 248) },
    //        TabHeaderBorder = new XThemeModeColor { Light = Color.FromRgb(201, 210, 222), Dark = Color.FromRgb(66, 76, 90) },
    //        TabItemBackground = new XThemeModeColor { Light = Color.FromArgb(0, 255, 255, 255), Dark = Color.FromArgb(0, 255, 255, 255) },
    //        TabItemBackgroundHover = new XThemeModeColor { Light = Color.FromRgb(247, 249, 252), Dark = Color.FromRgb(36, 44, 54) },
    //        TabItemBackgroundSelected = new XThemeModeColor { Light = Colors.White, Dark = Color.FromRgb(25, 31, 39) },
    //        TabItemForeground = new XThemeModeColor { Light = Color.FromRgb(107, 119, 136), Dark = Color.FromRgb(154, 165, 180) },
    //        TabItemForegroundHover = new XThemeModeColor { Light = Color.FromRgb(32, 41, 56), Dark = Color.FromRgb(239, 243, 248) },
    //        TabItemForegroundSelected = new XThemeModeColor { Light = Color.FromRgb(32, 41, 56), Dark = Color.FromRgb(239, 243, 248) },
    //        TabItemBorder = new XThemeModeColor { Light = Color.FromArgb(0, 255, 255, 255), Dark = Color.FromArgb(0, 255, 255, 255) },
    //        TabItemBorderHover = new XThemeModeColor { Light = Color.FromRgb(188, 198, 212), Dark = Color.FromRgb(63, 72, 86) },
    //        TabItemBorderSelected = new XThemeModeColor { Light = Color.FromRgb(188, 198, 212), Dark = Color.FromRgb(63, 72, 86) },
    //        TabItemUnderline = new XThemeModeColor { Light = Color.FromArgb(0, 255, 255, 255), Dark = Color.FromArgb(0, 255, 255, 255) },
    //        TabItemUnderlineSelected = new XThemeModeColor { Light = Color.FromRgb(24, 119, 242), Dark = Color.FromRgb(100, 170, 255) },
    //        TabContentBackground = new XThemeModeColor { Light = Colors.White, Dark = Color.FromRgb(25, 31, 39) },
    //        TabContentBorder = new XThemeModeColor { Light = Color.FromRgb(188, 198, 212), Dark = Color.FromRgb(63, 72, 86) },
    //        TabActionButtonForeground = new XThemeModeColor { Light = Color.FromRgb(107, 119, 136), Dark = Color.FromRgb(154, 165, 180) },
    //        TabActionButtonForegroundHover = new XThemeModeColor { Light = Color.FromRgb(32, 41, 56), Dark = Color.FromRgb(239, 243, 248) },
    //        TabActionButtonBackgroundHover = new XThemeModeColor { Light = Color.FromRgb(247, 249, 252), Dark = Color.FromRgb(36, 44, 54) },
    //    };

    //    // Register the custom theme
    //    XThemeService.Registry.Register(customTheme);

    //    // Apply the custom theme
    //    XThemeService.ChangeTheme(customTheme);
    //}

    #endregion

}
#endregion