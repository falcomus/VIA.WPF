// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MockupScreen.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.Mockup.Core.Model;

/// <summary>
/// Represents an editable full-screen layout.
/// </summary>
public sealed class MockupScreen : MockupDocument
{
    public override MockupDocumentKind Kind => MockupDocumentKind.Screen;

    public bool IsStartupScreen { get; set; }
}
