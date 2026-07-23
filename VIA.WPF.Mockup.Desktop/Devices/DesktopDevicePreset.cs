// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DesktopDevicePreset.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.Mockup.Desktop.Devices;

/// <summary>
/// Describes a selectable desktop canvas size.
/// </summary>
public sealed record DesktopDevicePreset(string Name, double Width, double Height);
