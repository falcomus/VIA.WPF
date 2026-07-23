// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XMockupCanvas.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using SkiaSharp.Views.Desktop;
using SkiaSharp.Views.WPF;
using VIA.WPF.Mockup.Skia.Rendering;

namespace VIA.WPF.Mockup.Wpf.Controls;

/// <summary>
/// Initial WPF host for the reusable Skia mockup surface.
/// </summary>
public sealed class XMockupCanvas : SKElement
{
    private readonly MockupRenderer renderer = new();

    public XMockupCanvas()
    {
        PaintSurface += OnPaintSurface;
    }

    private void OnPaintSurface(object? sender, SKPaintSurfaceEventArgs e)
    {
        renderer.Render(
            new MockupRenderContext(e.Surface.Canvas, e.Info.Width, e.Info.Height),
            document: null);
    }
}
