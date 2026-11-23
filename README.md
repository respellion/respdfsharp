# ResPdfSharp (Respellion Pdf Sharp)

**ResPdfSharp** is an open-source fork of [PDFsharp](https://docs.pdfsharp.net) & MigraDoc,
taken from the last state of the upstream project that was published under the **MIT License**
(tag `v7.0.0-preview-1`, commit `8d818d5f2ad5087201c4a54be3ab489b4920a6d2`, 2026-03-24).

ResPdfSharp stays **MIT licensed** — see [LICENSE](LICENSE).

> ResPdfSharp is an independent project and is **not** affiliated with, endorsed by, or
> sponsored by empira Software GmbH. "PDFsharp", "MigraDoc" and "empira" are names /
> trademarks of their respective owners and are used here only in an attributive sense to
> identify the upstream project this fork is derived from.

Based on the sources of version **7.0.0 Preview 1** (published 2026-03-24).

PDFsharp: Copyright (c) 2005-2026 empira Software GmbH, Troisdorf (Cologne Area), Germany
MigraDoc: Copyright (c) 2001-2026 empira Software GmbH, Troisdorf (Cologne Area), Germany
ResPdfSharp changes: Copyright (c) 2026 Respellion

Published Open Source under the [MIT License](LICENSE). See [NOTICE](NOTICE) for attribution
and [FORK.md](FORK.md) for the provenance record of this fork.

**Documentation & samples: <https://respellion.github.io/respdfsharp/>** (source in the
[`docs`](docs) folder of this repository). Install: `dotnet add package ResPdfSharp`
(current version **10.0.0**, on [nuget.org](https://www.nuget.org/packages/ResPdfSharp/)).

Internal/upstream working notes (not user docs) live in [`docs_old`](docs_old), including
the maintenance scripts of the original project in `docs_old/dev`.

## Why this fork exists

With PDFsharp 6.5.0 (2026-10-14, upstream commit `110751d`) the project moved from the MIT
license to a dual AGPL / commercial license. The code as of `8d818d5` — the last commit whose
`LICENSE` file was the MIT license — is permanently usable under MIT, and this fork continues
that MIT line under a new name, without the upstream project's branding.

## Fork features

### Built-in fallback fonts (Linux / containers)

The classic pain point of PDFsharp on Linux: without a custom `IFontResolver` and without
system fonts (typical in containers), any `new XFont("Arial", ...)` throws. ResPdfSharp
embeds three font families in the assembly and uses them automatically as the last-resort
fallback resolver — no configuration required:

- **Arimo** — metric-compatible with Arial/Helvetica
- **Tinos** — metric-compatible with Times New Roman
- **Cousine** — metric-compatible with Courier New

All in regular/bold/italic/bold-italic, licensed under the SIL Open Font License 1.1
(texts embedded and in [THIRD-PARTY-NOTICES.txt](THIRD-PARTY-NOTICES.txt)). Unknown or
generic family names ("sans-serif", "monospace", …) map heuristically to the closest
embedded family, so any font request succeeds. Custom resolvers set via
`GlobalFontSettings.FontResolver` keep priority; the bundled resolver (`PdfSharp.Fonts.BundledFontResolver`)
only acts when nothing else could resolve the face. Because the substitutes are
metric-compatible, existing layouts keep their line breaks and page counts.

## PDFsharp & MigraDoc 7 (original README content)

This is a preview version of the **PDFsharp** project, the main project of PDFsharp & MigraDoc 7 with updates for C# 12 and .NET 8, .NET 9, and .NET 10.

For more information on the original project see [docs.pdfsharp.net](https://docs.pdfsharp.net/).

## Read this FIRST

Project documentation of the original project can be found at: <https://docs.pdfsharp.net>.

Note that PowerShell 7 is required to execute the PowerShell scripts that come with the sources.

### Download assets first

Assets like bitmaps, fonts, or PDF files are not part of the repository anymore.
You must download them before compiling the solution for the first time.
Use `download-assets.ps1` in the `docs_old/dev` folder to create `assets` folder required for some unit tests and needed by some projects.
The script fetches the unmodified assets from the upstream host `assets.pdfsharp.net`; the
fork ships no upstream binaries beyond those build/test assets.

Execute

```ps
.\docs_old\dev\download-assets.ps1
```

### Build the solution

`dotnet build` should build the solution without any warnings or errors.

* You need the latest .NET SDK version installed
* The version is derived automatically by BuildConfig.
  You can set a tag to define a valid version, e.g.: `git tag v6.2.0` to make it build with a specific version number. Without tag, version 0.1.0 will be used.

### Central package management

The solution uses central package management.
Version numbers for all referenced packages are stored in file `Directory.Packages.props` in the `src` folder.
When adding new packages, add the required version here.

## Authors

PDFsharp and MigraDoc was mainly written by the following software developers.
With support of a lot of community developers who found issues and fixed bugs.

### Original PDFsharp developers

Stefan Lange
Niklas Schneider
David Stephensen

### Original MigraDoc developers

Klaus Potzesny
Niklas Schneider
Stefan Lange

### PDFsharp and MigraDoc developers at the fork point

Stefan Lange
Thomas Hövel
Martin Ossendorf
Andreas Seifert

### ResPdfSharp

Maintained by Respellion. Contribution policy: see [CONTRIBUTING](CONTRIBUTING).

## Libraries used by PDFsharp

The Core build of PDFsharp uses BigGustave to read PNG images. BigGustave was released into the public domain and does not restrict the MIT license used by PDFsharp.
Link to project repository: https://github.com/EliotJones/BigGustave

## Naming and trademark note

Assembly names and namespaces (`PdfSharp`, `MigraDoc`) are intentionally kept for source/API
compatibility; they are technical identifiers inherited from the original project. The
product name (`ResPdfSharp`), NuGet package ids (`ResPdfSharp*`), publisher, PDF producer
string, and all logo/branding are Respellion's. The upstream PDFsharp logo/icon was removed
from the NuGet packaging. "PDFsharp" may be a trademark of empira Software GmbH in your
jurisdiction — it is used in this repository only to describe the origin of the code, and
this fork is not a continuation of, or compatible-substitute claim for, the upstream branded
project.
