---
title: Getting Started
nav_order: 1
---

# ResPdfSharp

ResPdfSharp creates and modifies PDF documents from .NET code. It is an MIT-licensed
fork of PDFsharp & MigraDoc that keeps the original API and adds out-of-the-box fonts
for Linux and containers.

| | |
|---|---|
| Package | `ResPdfSharp` (core), `ResPdfSharp-MigraDoc` (document object model) |
| Version | **10.0.0** |
| License | MIT (code) — bundled fonts under SIL OFL 1.1 |
| Targets | net8.0, net9.0, net10.0, netstandard2.0 |
| NuGet | <https://www.nuget.org/packages/ResPdfSharp/> |

## Install

```bash
dotnet add package ResPdfSharp
# optionally, for high-level document layout:
dotnet add package ResPdfSharp-MigraDoc
```

## Hello, PDF

One file, no project ceremony. Save as `hello.cs` — .NET 10 file-based apps read
the package reference straight from the source file:

```csharp
#:package ResPdfSharp@10.0.0

using PdfSharp.Pdf;
using PdfSharp.Drawing;

using var document = new PdfDocument();
var page = document.AddPage();
var gfx = XGraphics.FromPdfPage(page);

var font = new XFont("Arial", 20, XFontStyleEx.Bold);
gfx.DrawString("Hello, ResPdfSharp!", font, XBrushes.Black,
    new XPoint(40, 80));

document.Save("hello.pdf");
```

```bash
dotnet run hello.cs   # → hello.pdf
```

That's the whole model: a `PdfDocument` holds pages, an `XGraphics` object draws on a
page with the same API you know from GDI+ (`DrawString`, `DrawLine`, `DrawImage`,
paths, transforms…).

No font configuration is needed — on Windows the system fonts are used; on Linux and
inside containers the [bundled fallback fonts](/respdfsharp/fonts/) kick in automatically.

## What's on this site

- [Fonts & Linux](/respdfsharp/fonts/) — how font resolution works and the bundled fallback.
- [Drawing samples](/respdfsharp/samples-drawing/) — shapes, text, images, transparency.
- [PDF manipulation samples](/respdfsharp/samples-pdf/) — merge, split, security, page sizes.
- [MigraDoc samples](/respdfsharp/samples-migradoc/) — documents, tables, styles, page layout.
- [Migration from upstream](/respdfsharp/migration/) — moving from PDFsharp 6.x/preview.
- [License & provenance](/respdfsharp/license/) — what you may do with this package.

## Which package do I need?

| Scenario | Package |
|---|---|
| Any platform (Linux/macOS/Windows/containers, Blazor, ASP.NET) | `ResPdfSharp` |
| Reports with paragraphs, tables, headers/footers | `ResPdfSharp` + `ResPdfSharp-MigraDoc` |
| Windows desktop apps using System.Drawing (GDI+) | `ResPdfSharp-GDI` (Windows only) |
| WPF apps | `ResPdfSharp-WPF` (Windows only) |

The core package has no Windows dependency and embeds everything it needs,
including fonts.
