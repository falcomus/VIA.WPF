// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MockupFeatureCatalog.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.Mockup;

/// <summary>
/// Exposes the initial feature identity of the aggregate mockup package.
/// </summary>
public static class MockupFeatureCatalog
{
    public const string PackageName = "VIA.WPF.Mockup";

    public static IReadOnlyList<string> InitialFeatures { get; } =
    [
        "Project",
        "Screen",
        "Template",
        "Popup",
        "Preview"
    ];
}
