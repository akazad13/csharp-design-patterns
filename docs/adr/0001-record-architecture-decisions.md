# ADR-0001: Record Architecture Decisions

## Status
Accepted

## Date
2026-11-07

## Context
As `csharp-design-patterns` evolves with C# language releases, modern .NET runtime paradigms, and contributions from both human engineers and autonomous AI agents, architectural decisions need to be documented systematically. Without an explicit decision log, future contributors and AI agents risk revisiting settled decisions (e.g., repository organization, targeting LTS .NET versions, idiomatic modern syntax choices, pedagogical pattern catalog structure).

We need a lightweight, version-controlled mechanism to record architectural decisions, trade-offs, and constraints directly within the repository.

## Decision
We will use Architecture Decision Records (ADRs) stored in `docs/adr/` using sequential numeric prefixes (`NNNN-title.md`) and standard Markdown format.

Key standards:
1. **Location**: All architectural decisions are stored in `docs/adr/`.
2. **Numbering**: Sequential 4-digit numbering (`0001-...md`, `0002-...md`).
3. **Format**: Follow standard ADR anatomy: Title, Status, Date, Context, Decision, Alternatives Considered, and Consequences.
4. **Index**: A root index file at `docs/adr/README.md` tracks all ADRs with status and summary.
5. **Immutability & Lifecycle**: ADRs are immutable historical records. Status transitions proceed from Proposed → Accepted → Superseded / Deprecated. When a decision changes, a new ADR is created that references and supersedes the predecessor.

## Alternatives Considered

### Relying solely on Git commit messages and PR descriptions
- **Pros**: Zero overhead, no extra files.
- **Cons**: Difficult to discover, scattered across commit history, lacks structured comparison of alternatives, inaccessible without git history traversal.
- **Rejected**: Context is easily lost as repositories mature or when viewed outside full git blame tools.

### Wiki / Confluence / External Docs
- **Pros**: Rich text editing, WYSIWYG tools.
- **Cons**: Decoupled from codebase changes, quickly falls out of sync with code, inaccessible during offline development and difficult for autonomous agents to read reliably.
- **Rejected**: Decisions must reside directly beside the code they govern.

### Single Monolithic ARCHITECTURE.md
- **Pros**: Single place to read.
- **Cons**: Doesn't capture point-in-time decision evolution, difficult to trace why a decision was made at a specific milestone, merges historical and current constraints.
- **Rejected**: Monolithic files conflate current state description with decision rationale.

## Consequences
- Every significant technical choice (runtime baseline, project layout, testing framework, dependency introduction) will have a documented record.
- AI agents and human developers can review settled decisions before proposing modifications.
- Low maintenance overhead: creating an ADR requires only a markdown document checked in alongside the corresponding code changes.
