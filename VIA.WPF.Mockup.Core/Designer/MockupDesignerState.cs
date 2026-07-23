// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MockupDesignerState.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.Mockup.Core.Designer;

/// <summary>
/// Stores neutral viewport and interaction state for a mockup designer.
/// </summary>
public sealed class MockupDesignerState
{
    public MockupDesignerMode Mode { get; set; } = MockupDesignerMode.Select;

    public double Zoom { get; set; } = 1d;

    public double PanX { get; set; }

    public double PanY { get; set; }

    public bool IsGridVisible { get; set; } = true;

    public bool IsSnapEnabled { get; set; } = true;

    public double SnapSpacing { get; set; } = 8d;

    public HashSet<Guid> SelectedElementIds { get; set; } = [];
}
