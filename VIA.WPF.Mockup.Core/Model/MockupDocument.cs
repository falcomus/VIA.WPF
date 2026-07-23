// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MockupDocument.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.Mockup.Core.Model;

/// <summary>
/// Base model for screens, popups and templates.
/// </summary>
public abstract class MockupDocument
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public abstract MockupDocumentKind Kind { get; }

    public double Width { get; set; }

    public double Height { get; set; }

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

    public List<MockupBand> Bands { get; set; } = [];
}
