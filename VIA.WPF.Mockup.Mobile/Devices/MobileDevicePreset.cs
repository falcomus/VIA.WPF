// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MobileDevicePreset.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.Mockup.Mobile.Devices;

/// <summary>
/// Describes a selectable mobile design surface.
/// </summary>
public sealed record MobileDevicePreset(
    string Name,
    double Width,
    double Height,
    MobileSafeArea SafeArea);
