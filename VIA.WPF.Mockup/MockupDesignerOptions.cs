// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MockupDesignerOptions.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.Mockup;

/// <summary>
/// Describes host-level defaults for the reusable mockup designer shell.
/// </summary>
public sealed class MockupDesignerOptions
{
    public string ProductName { get; set; } = "VIA.WPF Mockup Designer";

    public bool EnableDesktopDesigner { get; set; } = true;

    public bool EnableMobileDesigner { get; set; } = true;
}
