// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MockupBand.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using VIA.WPF.Mockup.Core.Geometry;

namespace VIA.WPF.Mockup.Core.Model;

/// <summary>
/// Represents a named layout band inside a mockup document.
/// </summary>
public sealed class MockupBand
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = string.Empty;

    public MockupBounds Bounds { get; set; }

    public List<MockupElement> Elements { get; set; } = [];
}
