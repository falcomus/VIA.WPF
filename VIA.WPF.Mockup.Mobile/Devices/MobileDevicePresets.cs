// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MobileDevicePresets.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.Mockup.Mobile.Devices;

/// <summary>
/// Provides the initial mobile canvas presets.
/// </summary>
public static class MobileDevicePresets
{
    public static IReadOnlyList<MobileDevicePreset> All { get; } =
    [
        new("Phone 390 × 844", 390d, 844d, new MobileSafeArea(0d, 47d, 0d, 34d)),
        new("Phone 360 × 800", 360d, 800d, new MobileSafeArea(0d, 24d, 0d, 24d)),
        new("Tablet 768 × 1024", 768d, 1024d, new MobileSafeArea(0d, 24d, 0d, 20d))
    ];
}
