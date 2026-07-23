// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MockupPopup.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.Mockup.Core.Model;

/// <summary>
/// Represents an editable popup layout.
/// </summary>
public sealed class MockupPopup : MockupDocument
{
    public override MockupDocumentKind Kind => MockupDocumentKind.Popup;
}
