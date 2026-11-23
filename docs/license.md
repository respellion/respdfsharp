---
title: License & provenance
permalink: /license/
---

# License & provenance

## The short answer

- **Code:** MIT. Use it in commercial closed-source apps — no obligations
  beyond keeping the copyright notices.
- **Bundled fonts:** SIL Open Font License 1.1. Embedding them in your
  application is fine; OFL covers the *fonts*, not your *documents* — PDFs you
  generate (including the embedded font subsets) are yours.
- **Name:** “PDFsharp” and “empira” are names/trademarks of empira Software GmbH
  and are used here only to describe where the code comes from. This fork is
  **ResPdfSharp**, maintained by Respellion, and is not affiliated with or
  endorsed by empira.

## Provenance

ResPdfSharp is forked from upstream [empira/PDFsharp](https://github.com/empira/PDFsharp)
at commit `8d818d5f2ad5087201c4a54be3ab489b4920a6d2` (tag `v7.0.0-preview-1`,
2026-03-24) — the last commit whose `LICENSE` file was the MIT license. The
next commit upstream (`110751d`, 2026-10-14) switched the project to a dual
AGPL / commercial license. MIT grants on the code already published are
irrevocable, so this fork permanently stays MIT.

The fork baseline is tagged `fork-start` in the repository, and the untouched
tree is archived byte-for-byte at
`archive/respdfsharp-fork-start-8d818d5.tar.gz` (hash in the adjacent `.sha256`).

Because upstream commits after the fork point are AGPL, they are **not** blind
cherry-picked into this fork; they are reimplemented with documented
MIT-compatible provenance where useful. Details in
[FORK.md](https://github.com/respellion/respdfsharp/blob/main/FORK.md).

## Attribution

The MIT license requires preserving the original copyright notice. You'll find:

- [LICENSE](https://github.com/respellion/respdfsharp/blob/main/LICENSE) —
  original empira MIT notice + Respellion attribution for fork changes.
- [NOTICE](https://github.com/respellion/respdfsharp/blob/main/NOTICE) —
  provenance and naming terms.
- [THIRD-PARTY-NOTICES.txt](https://github.com/respellion/respdfsharp/blob/main/THIRD-PARTY-NOTICES.txt) —
  Arimo/Tinos/Cousine OFL texts.

## Trademark note

Assembly names and namespaces (`PdfSharp`, `MigraDoc`) were intentionally kept
so that migration is a package swap. They are technical identifiers, not a
claim that this is the upstream branded product. Product name, NuGet ids
(`ResPdfSharp*`), publisher metadata and PDF producer strings are ResPdfSharp.
