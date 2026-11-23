---
title: Fonts & Linux
permalink: /fonts/
---

# Fonts & Linux

The most common complaint about PDFsharp on Linux: `new XFont("Arial", 12)` throws
*"No appropriate font found for family name 'Arial'"* on a machine without those fonts —
which is every slim container image. **ResPdfSharp fixes this out of the box.**

## How font resolution works

When you use a font, ResPdfSharp asks, in order:

1. **Your custom resolver** — `GlobalFontSettings.FontResolver` (set one if you ship
   your own fonts).
2. **The platform resolver** — real system fonts. On Windows this always works; on Linux
   only if the fonts are installed (and under WSL2 if enabled).
3. **Your fallback resolver** — `GlobalFontSettings.FallbackFontResolver`, if you set one.
4. **The bundled resolver** *(ResPdfSharp feature)* — fonts embedded inside the assembly.
   This last step always succeeds, so font requests never fail out of the box.

## Bundled fonts

| Bundled family | Metric-compatible with | Styles |
|---|---|---|
| **Arimo** | Arial / Helvetica | regular, bold, italic, bold-italic |
| **Tinos** | Times New Roman | regular, bold, italic, bold-italic |
| **Cousine** | Courier New | regular, bold, italic, bold-italic |

Because they are *metric-compatible*, text measured with Arial wraps identically with
Arimo — layouts, line breaks and page counts don't shift when the fallback kicks in.

Unknown or generic family names are mapped heuristically: `sans-serif`/`helvetica`/`Segoe UI`
→ Arimo, `serif`/`georgia` → Tinos, anything containing `mono`/`courier`/`consolas` →
Cousine, and everything else → Arimo. You can therefore safely use the usual web font
names in cross-platform code.

```csharp
// Works on Linux containers with zero configuration:
var a = new XFont("Arial", 12);
var b = new XFont("Times New Roman", 12, XFontStyleEx.BoldItalic);
var c = new XFont("monospace", 10);
```

The used glyphs are embedded in the PDF as subsets, so the viewer always sees the text
correctly regardless of its own fonts.

## License of the fonts

All three fonts are under the **SIL Open Font License 1.1**. The license texts ship
inside the assembly (`BundledFontResolver.GetFontLicense("Arimo")`) and in
[THIRD-PARTY-NOTICES.txt](https://github.com/respellion/respdfsharp/blob/main/THIRD-PARTY-NOTICES.txt)
in the repository. The OFL imposes no restrictions on the documents you produce —
the PDFs and their embedded font subsets are yours.

## Escape hatches

- Force a resolver for full control:

```csharp
GlobalFontSettings.FontResolver = new MyEmbeddedFontResolver(); // your fonts
// or make the bundled resolver the explicit fallback:
PdfSharp.Fonts.BundledFontResolver.Install();
```

- Use real system fonts on Linux: install them (`fontconfig`, e.g.
  `apt install fonts-liberation`) — the platform resolver picks them up before the
  bundled fallback is consulted.
