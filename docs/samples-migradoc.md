---
title: MigraDoc samples
permalink: /samples-migradoc/
---

# MigraDoc samples

MigraDoc is the high-level document layer on top of ResPdfSharp: you describe
documents (paragraphs, tables, styles, headers) and it renders them to PDF with
automatic pagination. Install `ResPdfSharp-MigraDoc` (it pulls in the core).

All samples below were compiled and rendered against ResPdfSharp 10.0 on Linux
with no font setup.

```csharp
#:package ResPdfSharp-MigraDoc@10.0.0

using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.DocumentObjectModel.Fields;
using MigraDoc.Rendering;
```

> **MigraDoc 7 note:** the object model was modernized. Paragraphs are created
> empty and filled with `AddText(...)` (there is no `new Paragraph("text")`
> constructor), and tables are built with `Columns.AddColumn()` — see
> [Migration](/respdfsharp/migration/).

## A first document

```csharp
var document = new Document();

var style = document.Styles.AddStyle("ReportTitle", "Normal");
style.Font.Name = "Arial";
style.Font.Size = 20;
style.Font.Bold = true;
style.Font.Color = Colors.SteelBlue;
style.ParagraphFormat.SpaceAfter = 24;

var section = document.AddSection();
section.AddParagraph("Quarterly Report", "ReportTitle");
section.AddParagraph("Sales grew 12% quarter over quarter.");

var renderer = new PdfDocumentRenderer { Document = document };
renderer.RenderDocument();
renderer.Save("report.pdf");
```

## Tables

Columns are defined first; cells are addressed by row/column index:

```csharp
var table = new Table { Style = "TableGrid" };   // TableGrid = with borders
table.Columns.AddColumn().Width = "4cm";
table.Columns.AddColumn().Width = "3cm";
table.Columns.AddColumn().Width = "3cm";

var header = table.AddRow();
header.Shading.Color = Colors.LightSteelBlue;
foreach (var (cell, title) in new[] { (0, "Product"), (1, "Units"), (2, "Revenue") })
{
    var hp = new Paragraph(); hp.AddText(title);
    header[cell].Add(hp);
}

foreach (var (product, units, revenue) in new[]
{
    ("Widget", 120, "1,200 €"),
    ("Gadget",  80, "1,600 €"),
})
{
    var row = table.AddRow();
    var p0 = new Paragraph(); p0.AddText(product);
    row[0].Add(p0);
    var p1 = new Paragraph(); p1.AddText(units.ToString());
    p1.Format.Alignment = ParagraphAlignment.Right;
    row[1].Add(p1);
    var p2 = new Paragraph(); p2.AddText(revenue);
    p2.Format.Alignment = ParagraphAlignment.Right;
    row[2].Add(p2);
}
section.Add(table);
```

Note: for the header row use the same `new Paragraph(); p.AddText(...)` pattern
as for the data rows. Without `Style = "TableGrid"` (or explicit borders) a
table renders borderless.

## Styles: inheritance and document-wide typography

Named styles inherit from a parent (`"ReportTitle"` from `"Normal"`). Set the
document default once:

```csharp
document.Styles["Normal"].Font.Name = "Arial";
document.Styles["Normal"].Font.Size = 11;
```

Because Arial is covered by the metric-compatible
[bundled fallback fonts](/respdfsharp/fonts/), this looks the same on Windows and Linux.

## Page setup, margins, footer with page numbers

```csharp
var section = document.AddSection();
section.PageSetup.Orientation = Orientation.Landscape;
section.PageSetup.TopMargin = Unit.FromCentimeter(2.5);
// page size is set via PageWidth/PageHeight or DDL; A4 is the default.

var footer = section.Footers.Primary;
var fp = footer.AddParagraph();
fp.Add(new PageField());                       // current page number
fp.Format.Alignment = ParagraphAlignment.Center;
```

Other fields: `NumPagesField`, `DateField { Format = "yyyy/MM/dd" }`,
`InfoField`, `SectionField`.

## Images

```csharp
var image = section.AddImage("logo.png");
image.Width = "6cm";   // height keeps aspect ratio automatically
```

## Bookmarks and hyperlinks

```csharp
var p = section.AddParagraph("See the ");
p.AddBookmark("top");
var link = p.AddHyperlink("https://respellion.github.io/respdfsharp/",
                          HyperlinkType.Url);
link.AddText("online documentation");
```

## Post-processing the rendered PDF

The renderer exposes the underlying `PdfSharp.Pdf.PdfDocument`, so you can stamp
or secure the result with the low-level API:

```csharp
var renderer = new PdfDocumentRenderer { Document = document };
renderer.RenderDocument();
var pdf = renderer.PdfDocument;
// ... e.g. watermark with XGraphics, set metadata, security ...
pdf.Save("report.pdf");
```

A runnable sample project lives in the repository at
`src/samples/src/MigraDoc/src/HelloWorld` — it shows fonts, footers with
date fields, images and PDF/A setup.
