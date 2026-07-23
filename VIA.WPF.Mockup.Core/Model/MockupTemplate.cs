// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MockupTemplate.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace VIA.WPF.Mockup.Core.Model;

/// <summary>
/// Represents a reusable editable layout template.
/// </summary>
public sealed class MockupTemplate : MockupDocument
{
    public override MockupDocumentKind Kind => MockupDocumentKind.Template;
}
