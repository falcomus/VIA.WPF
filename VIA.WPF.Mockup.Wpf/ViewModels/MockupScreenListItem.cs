// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MockupScreenListItem.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Windows.Media;

namespace VIA.WPF.Mockup.Wpf.ViewModels;

/// <summary>
/// Represents an item in the Project-page screen preview list.
/// </summary>
public sealed class MockupScreenListItem
{
    public string Name { get; init; } = string.Empty;

    public string Target { get; init; } = string.Empty;

    public string? State { get; init; }

    public string UpdatedText { get; init; } = string.Empty;

    public string CreatedText { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;

    public string Resolution { get; init; } = string.Empty;

    public bool IsMain { get; init; }

    public int UsedControlsCount { get; init; }

    public Brush AccentBrush { get; init; } = Brushes.SteelBlue;
}
