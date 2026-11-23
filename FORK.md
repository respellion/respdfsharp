# ResPdfSharp fork provenance

## Fork baseline

- Upstream repository: https://github.com/empira/PDFsharp (branch `master`)
- Fork commit: `8d818d5f2ad5087201c4a54be3ab489b4920a6d2`
- Tag at that commit: `v7.0.0-preview-1` (published 2026-03-24)
- Git tag in this repo marking the baseline: `fork-start`
- License file at the baseline commit: MIT (`LICENSE`, SHA-256
  `3c648a5072bd7f7386ca28687780489a708a57ed75f13927bc16dd3e34af3bf7`) — verified by
  `git show 8d818d5:LICENSE`
- The full baseline tree is archived byte-for-byte at
  `archive/respdfsharp-fork-start-8d818d5.tar.gz` (SHA-256
  `5c96d1a8b35c892e46436706bf384bef488d39d460f9af5030fea833fd425bf3`).

## The license change in upstream

- Upstream commit `110751d6cc0fba1ea1764a12f038daa9bb0a921a` (2026-10-14, "PDFsharp-Team")
  replaced the MIT `LICENSE` with an AGPL dual-license notice. It is the direct child of the
  fork commit `8d818d5`.
- Every commit at or before `8d818d5` was offered under MIT. MIT grants are irrevocable for
  the code already published, so forking at this commit is lawful: we may copy, modify,
  redistribute, and republish under our own name, keeping the MIT notice (LICENSE) intact.
- Names ("PDFsharp", "MigraDoc", "empira"), logos and branding are NOT part of the software
  license grant. The MIT license grants no trademark rights. The fork therefore renamed the
  product to ResPdfSharp and removed upstream branding assets:
  - README badges ("Maintained by PDFsharp Team", "Commercial Support") removed
  - NuGet package icons (`PDFsharp-128x128.png` / `MigraDoc-128x128.png`) removed from all
    12 nuspec files
  - product/publisher/author/owner/copyright metadata in `src/Directory.Build.props`,
    `PdfSharpProductVersionInformation.cs` and `MigraDocProductVersionInformation.cs`
    re-attributed to Respellion with upstream attribution preserved
  - PDF `Producer`/`Creator` strings and digital-signature app name changed to ResPdfSharp
  - NuGet package ids renamed `PDFsharp*` -> `ResPdfSharp*`
  - the strong-name key was replaced (`src/StrongnameKey.snk` + new public key token in
    `Directory.Build.props`) so fork assemblies cannot be mistaken for, or swapped into
    positions of, empira's signed upstream assemblies.

## Import policy for later upstream fixes

Upstream 6.5.0+ is AGPL/commercial. Changes from those commits are NOT automatically
importable. Any upstream fix imported into ResPdfSharp must either
(a) come from a commit that was published under MIT (i.e. ancestor of `fork-start`), or
(b) be re-implemented independently from the behavioural requirement, or
(c) carry documented MIT-compatible permission from the contributor/empira.

## Contributor agreements — finding

Upstream PDFsharp has/had **no Contributor License Agreement**: no `CONTRIBUTING` file, no
CLA bot, no DCO sign-off requirement in its git history, and its GitHub history shows only
"PDFsharp-Team" and "ThomasHoevel" as commit authors (community fixes arrive through the
forum and are committed by the team). That is "something off" worth noting: contributions
have no recorded assignment or license grant beyond copyright-license-by-implication
(contributing to an MIT project implies an MIT grant for the contributed change, but there
is no explicit grant and no way to relicense upstream contributions). For ResPdfSharp this
is not a defect that taints the fork — the inherited code was validly MIT-licensed by its
copyright holder — but it does mean:
- we cannot relicense the inherited code past MIT (no CLA, so no extra rights),
- our own contribution policy is documented in CONTRIBUTING,
- if Respellion ever wants AGPL-or-later or relicensing freedom, contributions must come
  with an explicit license grant.
