// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MockupProject.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.Mockup.Core.Model;

/// <summary>
/// Represents a complete mockup design project.
/// </summary>
public sealed class MockupProject
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string AuthorTeam { get; set; } = string.Empty;

    public string Version { get; set; } = "1.0.0";

    public MockupProjectTarget Target { get; set; } = MockupProjectTarget.Both;

    public string? StartupScreenId { get; set; }

    public List<MockupScreen> Screens { get; set; } = [];

    public List<MockupPopup> Popups { get; set; } = [];

    public List<MockupTemplate> Templates { get; set; } = [];
}
