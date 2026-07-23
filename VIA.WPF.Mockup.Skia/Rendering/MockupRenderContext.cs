// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MockupRenderContext.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using SkiaSharp;

namespace VIA.WPF.Mockup.Skia.Rendering;

/// <summary>
/// Carries the neutral Skia render target and viewport dimensions.
/// </summary>
public sealed class MockupRenderContext
{
    public MockupRenderContext(SKCanvas canvas, int pixelWidth, int pixelHeight)
    {
        Canvas = canvas ?? throw new ArgumentNullException(nameof(canvas));
        PixelWidth = pixelWidth;
        PixelHeight = pixelHeight;
    }

    public SKCanvas Canvas { get; }

    public int PixelWidth { get; }

    public int PixelHeight { get; }
}
