// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MockupElement.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using VIA.WPF.Mockup.Core.Geometry;

namespace VIA.WPF.Mockup.Core.Model;

/// <summary>
/// Represents a neutral control instance placed inside a mockup document.
/// </summary>
public sealed class MockupElement
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string ControlType { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public MockupBounds Bounds { get; set; }

    public int ZIndex { get; set; }

    public Dictionary<string, string?> Properties { get; set; } = [];
}
