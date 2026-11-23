---
title: PDF manipulation samples
permalink: /samples-pdf/
---

# PDF manipulation samples

Reading and editing existing PDFs with the `PdfSharp` package.

## Open, inspect, save

```csharp
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

using var document = PdfReader.Open("input.pdf", PdfDocumentOpenMode.Modify);
Console.WriteLine($"Pages: {document.PageCount}, encrypted: {document.SecuritySettings.IsEncrypted}");
document.Save("output.pdf");
```

Open modes: `Modify` (read/write), `Import` (pages for merging),
`InformationOnly` (metadata without loading content streams). If the file is
encrypted, use the overload `PdfReader.Open(path, password, mode)`.

## Merge PDFs

```csharp
using var output = new PdfDocument();
foreach (var file in new[] { "a.pdf", "b.pdf", "c.pdf" })
{
    using var input = PdfReader.Open(file, PdfDocumentOpenMode.Import);
    foreach (var page in input.Pages)
        output.AddPage(page);          // adds a copy of the page
}
output.Save("merged.pdf");
```

To copy only a range: `output.AddPage(input.Pages[3])`, or
`output.AddPageRange(input, 0, 4)` for pages 0–4.

## Split a PDF

```csharp
using var source = PdfReader.Open("big.pdf", PdfDocumentOpenMode.Import);
for (int i = 0; i < source.PageCount; i++)
{
    using var single = new PdfDocument();
    single.AddPage(source.Pages[i]);
    single.Save($"page-{i + 1}.pdf");
}
```

## Page size and orientation

```csharp
using var document = new PdfDocument();
var page = document.AddPage();
page.Size = PdfSharp.PageSize.A4;          // or Letter, Legal, A3…
page.Orientation = PageOrientation.Landscape;
// Custom size in points (72 pt = 1 inch):
page.Width = XUnit.FromCentimeter(10);
page.Height = XUnit.FromCentimeter(10);
```

## Metadata

```csharp
document.Info.Title = "Quarterly report";
document.Info.Author = "Respellion";
document.Info.Subject = "Q3 numbers";
document.Info.CreationDate = DateTime.UtcNow;
document.Info.Keywords = "report, q3";
```

## Password protection

```csharp
document.SecuritySettings.OwnerPassword = "s3cret";
document.SecuritySettings.UserPassword = "";   // empty: anyone can open the file
document.Save("protected.pdf");

// To modify such a document you must supply the owner password:
using var editable = PdfReader.Open("protected.pdf", "s3cret", PdfDocumentOpenMode.Modify);
```

The property is `SecuritySettings` (not `Security`), on `PdfDocument` and on loaded
documents (`document.SecuritySettings.IsEncrypted`). Fine-grained permission flags
(print/modify/copy) depend on the security handler; see `PdfSharp.Pdf.Security`.

## Stamps and watermarks on existing pages

```csharp
using var document = PdfReader.Open("input.pdf", PdfDocumentOpenMode.Modify);
foreach (var page in document.Pages)
{
    var gfx = XGraphics.FromPdfPage(page);
    var font = new XFont("Arial", 60, XFontStyleEx.Bold);
    var state = gfx.Save();
    gfx.TranslateTransform(page.Width.Point / 2, page.Height.Point / 2);
    gfx.RotateTransform(-45);
    gfx.DrawRectangle(new XSolidBrush(XColor.FromArgb(60, 255, 0, 0)),
        new XRect(-200, -30, 400, 60));
    gfx.DrawString("DRAFT", font, XBrushes.Red,
        new XRect(-200, -30, 400, 60), XStringFormats.Center);
    gfx.Restore(state);
    gfx.Dispose();
}
document.Save("stamped.pdf");
```

`XGraphics.FromPdfPage(page)` appends on top of the existing content. The overload
with `XGraphicsPdfPageOptions.Prepend` draws **behind** it, but can fail on pages
whose content stream structure is unusual (e.g. some imported/encrypted files) —
prefer the default when in doubt.
