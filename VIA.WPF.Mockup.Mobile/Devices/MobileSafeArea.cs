// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MobileSafeArea.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.Mockup.Mobile.Devices;

/// <summary>
/// Describes device safe-area insets in logical pixels.
/// </summary>
public readonly record struct MobileSafeArea(double Left, double Top, double Right, double Bottom);
