# Architecture Decision Records (ADRs)

This directory contains records of significant architectural and technical decisions made in `csharp-design-patterns`. Following the [documentation-and-adrs](https://github.com/akazad13/csharp-design-patterns) practices, ADRs explain the context, trade-offs, and consequences of choices so future maintainers and autonomous AI agents can understand *why* the codebase is structured this way.

## Decision Log

| ID | Date | Title | Status | Summary |
|---|---|---|---|---|
| [ADR-0001](0001-record-architecture-decisions.md) | 2026-11-07 | Record Architecture Decisions | Accepted | Establish `docs/adr/` with sequential numbering (`NNNN-title.md`) to capture rationale and prevent re-debating settled decisions. |
| [ADR-0002](0002-dotnet-modernization-and-pattern-hierarchy.md) | 2026-11-07 | .NET Modernization and Pattern Hierarchy | Accepted | Standardize on .NET 8 (LTS) / C# 12+ idioms, standalone runnable projects grouped by GoF category, and 5-point pedagogical catalog standards. |

## Lifecycle

```
PROPOSED → ACCEPTED → (SUPERSEDED or DEPRECATED)
```

- **Immutable History**: ADRs are not deleted when decisions change. Instead, submit a new ADR that references and marks the old ADR as `Superseded by ADR-XXXX`.
- **Naming Convention**: `NNNN-short-descriptive-title.md` (e.g. `0003-benchmarks-and-unit-tests.md`).
- **Anatomy**: Each ADR must include Title, Status, Date, Context, Decision, Alternatives Considered, and Consequences.
