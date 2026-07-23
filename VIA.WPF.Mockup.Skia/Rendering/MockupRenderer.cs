// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MockupRenderer.cs" company="VIA.WPF">
//   Copyright (c) VIA.WPF. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using SkiaSharp;
using VIA.WPF.Mockup.Core.Model;

namespace VIA.WPF.Mockup.Skia.Rendering;

/// <summary>
/// Provides the initial reusable Skia render entry point.
/// </summary>
public sealed class MockupRenderer
{
    private static readonly SKColor SurfaceColor = new(242, 245, 249);
    private static readonly SKColor GridColor = new(218, 226, 236);
    private static readonly SKColor TextColor = new(71, 85, 105);

    public void Render(MockupRenderContext context, MockupDocument? document)
    {
        ArgumentNullException.ThrowIfNull(context);

        SKCanvas canvas = context.Canvas;
        canvas.Clear(SurfaceColor);

        using SKPaint gridPaint = new()
        {
            Color = GridColor,
            StrokeWidth = 1f,
            IsAntialias = false
        };

        const int spacing = 16;
        for (int x = 0; x < context.PixelWidth; x += spacing)
        {
            canvas.DrawLine(x, 0, x, context.PixelHeight, gridPaint);
        }

        for (int y = 0; y < context.PixelHeight; y += spacing)
        {
            canvas.DrawLine(0, y, context.PixelWidth, y, gridPaint);
        }

        using SKTypeface typeface = SKTypeface.FromFamilyName("Segoe UI", SKFontStyle.Normal);
        using SKFont font = new(typeface, 16f)
        {
            Subpixel = true
        };

        using SKPaint textPaint = new()
        {
            Color = TextColor,
            IsAntialias = true,
            Style = SKPaintStyle.Fill
        };

        string text = document is null
            ? "VIA.WPF Mockup Skia surface"
            : $"{document.Kind}: {document.Name}";

        canvas.DrawText(text, 24f, 34f, SKTextAlign.Left, font, textPaint);
    }
}
