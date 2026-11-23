---
title: Drawing samples
permalink: /samples-drawing/
---

# Drawing samples

All samples use the `PdfSharp` package only and run on any platform, including Linux.

Copy the header below once into `drawing.cs`, then paste any section after it —
each snippet is a continuation of the same file. Run with `dotnet run drawing.cs`.

```csharp
#:package ResPdfSharp@10.0.0

using PdfSharp.Pdf;
using PdfSharp.Drawing;

using var document = new PdfDocument();
var page = document.AddPage();
var gfx = XGraphics.FromPdfPage(page);
```

## Text: alignment, measuring, positioning

```csharp
var font = new XFont("Arial", 14);

// Measure before drawing — essential for right/center alignment.
var size = gfx.MeasureString("Centered text", font);
gfx.DrawString("Centered text", font, XBrushes.Black,
    new XRect(0, 100, page.Width.Point, 0), XStringFormats.Center);

// Left / right margins from measured width.
gfx.DrawString("Right aligned", font, XBrushes.Black,
    new XPoint(page.Width.Point - 40 - size.Width, 150));
```

## Shapes, lines, pens

```csharp
var pen = new XPen(XColors.SteelBlue, 2) { DashStyle = XDashStyle.Dash };
gfx.DrawEllipse(pen, XBrushes.LightSkyBlue, 40, 40, 100, 60);
gfx.DrawLine(pen, 40, 120, 140, 180);

// Rounded rectangle via an XGraphicsPath.
var path = new XGraphicsPath();
path.AddRoundedRectangle(200, 40, 120, 60, 10, 10);
gfx.DrawPath(new XPen(XColors.DarkSlateGray, 1), XBrushes.Khaki, path);
```

## Images

```csharp
using var image = XImage.FromFile("logo.png");
// Keep aspect ratio inside a box.
var scale = Math.Min(200.0 / image.PixelWidth, 100.0 / image.PixelHeight);
gfx.DrawImage(image, 40, 220, image.PixelWidth * scale, image.PixelHeight * scale);
```

`XImage.FromFile` also accepts URLs (`XImage.FromUrl`) and streams
(`XImage.FromStream`). PNG, JPEG, TIFF and GIF are supported.

## Color, transparency, gradients

```csharp
gfx.DrawRectangle(XBrushes.DarkOrange, 40, 320, 60, 60);

var state = gfx.Save();                 // isolate the transparency
gfx.DrawRectangle(new XSolidBrush(XColor.FromArgb(128, 255, 0, 0)),
    120, 320, 60, 60);
gfx.Restore(state);

var brush = new XLinearGradientBrush(new XRect(240, 320, 80, 60),
    XColors.White, XColors.SteelBlue, XLinearGradientMode.Vertical);
gfx.DrawRectangle(brush, 240, 320, 80, 60);
```

Note: the cross-platform Core build draws filled shapes with `DrawRectangle` /
`DrawEllipse` / `DrawPath` (there are no `Fill…` methods), and `XGraphicsState`
is not disposable — pair `gfx.Save()` with `gfx.Restore(state)`.

## Coordinate transforms (rotation, scaling)

```csharp
var state = gfx.Save();
gfx.TranslateTransform(100, 450);
gfx.RotateTransform(-30);
gfx.DrawString("Rotated text", new XFont("Arial", 16),
    XBrushes.Black, 0, 0);
gfx.Restore(state);
```

## Fonts: styles and Unicode

```csharp
foreach (var (style, dy) in new (XFontStyleEx, int)[]
{
    (XFontStyleEx.Regular, 0), (XFontStyleEx.Bold, 24),
    (XFontStyleEx.Italic, 48), (XFontStyleEx.BoldItalic, 72),
})
{
    gfx.DrawString("The quick brown fox 1234567890 — €üñ",
        new XFont("Arial", 13, style), XBrushes.Black, 40, 520 + dy);
}
```

Unicode works out of the box: the bundled fallback fonts cover Latin, Greek,
Cyrillic and many more scripts, and glyphs are embedded in the PDF.

## Multiple pages

```csharp
for (int i = 0; i < 3; i++)
{
    var page = document.AddPage();
    var g = XGraphics.FromPdfPage(page);
    g.DrawString($"Page {i + 1}", new XFont("Arial", 30), XBrushes.Gray, 40, 80);
}
document.Save("pages.pdf");
```

Page indexing is **0-based**: `document.Pages[0]` is the first page, `document.PageCount`
gives the total.
