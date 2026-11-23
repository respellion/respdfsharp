# ResPdfSharp 10.0.0

First stable release of the ResPdfSharp fork. **`dotnet add package ResPdfSharp`** —
[ResPdfSharp](https://www.nuget.org/packages/ResPdfSharp/10.0.0) and
[ResPdfSharp-MigraDoc](https://www.nuget.org/packages/ResPdfSharp-MigraDoc/10.0.0) are
published on nuget.org. Documentation & samples: <https://respellion.github.io/respdfsharp/>

## New: bundled fallback fonts — Linux out of the box

The most common PDFsharp pain point on Linux/containers is gone. The assembly now
embeds three font families and uses them as an automatic last-resort resolver, so
`new XFont("Arial", …)` never throws, with zero configuration:

- **Arimo** (metric-compatible with Arial/Helvetica)
- **Tinos** (metric-compatible with Times New Roman)
- **Cousine** (metric-compatible with Courier New)

Each in regular/bold/italic/bold-italic, licensed SIL OFL 1.1 (license texts embedded;
see `THIRD-PARTY-NOTICES.txt`). Generic/unknown family names map heuristically
(`sans-serif`→Arimo, `monospace`→Cousine, `serif`→Tinos, else Arimo). Custom
resolvers set via `GlobalFontSettings.FontResolver` keep priority. Metric
compatibility means existing layouts keep their line breaks and page counts.

Implementation: `PdfSharp.Fonts.BundledFontResolver` + implicit hookup in
`FontFactory` when no custom/platform/fallback resolver could resolve a typeface.

## Docs

New user documentation with runnable samples in `docs/`, published as a GitHub
Pages site at <https://respellion.github.io/respdfsharp/> (built from this repo,
so it stays in sync): getting started, fonts & Linux, drawing samples, PDF
manipulation samples, MigraDoc samples, migration guide, license & provenance.
All samples were compiled and rendered against this build.

The former internal team notes (`docs/`) moved to `docs_old/` (including the
old `dev/*.ps1` maintenance scripts); upstream `azure-pipelines.yml` removed.

## Versioning

This fork now versions independently: **10.0.0** (re-bumped from
7.0.0-preview-1-fork.x as requested). Fork baseline remains MIT commit
`8d818d5` (upstream `v7.0.0-preview-1`), tagged `fork-start`; see `FORK.md`.

## Package contents

- `ResPdfSharp` 10.0.0 — net8.0, net9.0, net10.0, netstandard2.0 (now includes the
  ~5.6 MB of embedded fallback fonts).
- `ResPdfSharp-MigraDoc` 10.0.0 — same TFM set, depends on ResPdfSharp.
- Strong-name token `df349515262cddb1` (fork key; update binding rules if you pinned the old token).
