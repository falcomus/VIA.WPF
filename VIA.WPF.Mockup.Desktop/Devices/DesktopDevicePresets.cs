// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DesktopDevicePresets.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.Mockup.Desktop.Devices;

/// <summary>
/// Provides the initial desktop canvas presets.
/// </summary>
public static class DesktopDevicePresets
{
    public static IReadOnlyList<DesktopDevicePreset> All { get; } =
    [
        new("Desktop 1920 × 1080", 1920d, 1080d),
        new("Desktop 1440 × 900", 1440d, 900d),
        new("Laptop 1366 × 768", 1366d, 768d),
        new("Tablet Landscape 1280 × 800", 1280d, 800d)
    ];
}
