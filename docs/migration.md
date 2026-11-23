---
title: Migration guide
permalink: /migration/
---

# Migrating to ResPdfSharp

## From upstream PDFsharp / MigraDoc 6.2.x or 7.0.0-preview

If your code targets PDFsharp 6.2.x or the 7.0.0 preview (MIT era), the API is
essentially unchanged. Replace the package ids:

| Upstream package | ResPdfSharp package |
|---|---|
| `PDFsharp` | `ResPdfSharp` |
| `PDFsharp-MigraDoc` | `ResPdfSharp-MigraDoc` |
| `PDFsharp-gdi` | `ResPdfSharp-GDI` |
| `PDFsharp-wpf` | `ResPdfSharp-WPF` |

Namespaces (`PdfSharp.*`, `MigraDoc.*`) and assembly names are kept for
compatibility. Strong-name identity differs (token `df349515262cddb1`); if you
had assembly-binding rules keyed to the old public key token, update them.

What changes for you:

- **Fonts on Linux just work.** The bundled fallback resolver fills in for any
  family the platform cannot provide. If you had installed a custom
  `FailsafeFontResolver` or similar workaround, you can delete it — your own
  `GlobalFontSettings.FontResolver` still takes priority.
- **PDF producer string** is `ResPdfSharp …` instead of `PDFsharp …`. Tests that
  compare metadata byte-for-byte may need updating.
- **Package version numbers** are fork versions (10.0.0 and up), not upstream
  semver — pin them like any other dependency.

## From MigraDoc 6.x code (classic API)

The 7.x object model changed shape in a few places. Common patterns:

| Classic (6.x) | 7.x / ResPdfSharp 10 |
|---|---|
| `new Paragraph("text")` / `section.AddParagraph("text", "Style")` | `AddParagraph` on sections still exists; inline use: `var p = new Paragraph(); p.AddText("text");` |
| `table.AddColumn("4cm")` | `table.Columns.AddColumn().Width = "4cm"` |
| `table.AddHeaderRow()` | not present; use `table.AddRow()` (repeat header via `Rows`) |
| `row.AddCell("text")` | `row[i].Add(paragraph)` |
| `new Unit(2.5, UnitScale.Centimeter)` | `Unit.FromCentimeter(2.5)` |
| `pageSetup.PageSize = PageSize.A4` | page size via `PageSetup.PageWidth/PageHeight` (A4 is default) |
| `paragraph.AddPageNumber(PageNumberFormat.Normal)` | `paragraph.Add(new PageField())` |
| `p.AddHyperlink("text", "http://…")` | `var h = p.AddHyperlink(url, HyperlinkType.Url); h.AddText("text");` |

## From PDFsharp 6.5+ (the AGPL line)

Upstream moved to AGPL/commercial with 6.5.0. Your code keeps compiling —
ResPdfSharp is API-compatible with the MIT-era code those releases were built
on — but you now owe AGPL obligations to upstream. Switching to ResPdfSharp
removes them for the fork's code.
