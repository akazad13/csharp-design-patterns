# AGENTS.md — AI Agent Guidance & Repository Rules

This document provides mandatory guidance for autonomous AI coding agents (Antigravity, Claude Code, GitHub Copilot, Cursor, Windsurf, Devin) operating in `akazad13/csharp-design-patterns`.

---

## 1. System Identity & Architecture

**Product**: `C# Design Patterns` — Exhaustive, production-oriented reference implementation of Gang of Four (GoF) and architectural software patterns in C# 12 / .NET 8+.
- **Language**: C# 12 / 13.
- **Framework**: .NET 8 / 10.
- **Core Focus**: Creational, Structural, Behavioral, and Enterprise Architectural design patterns.

### Project Layout:
- `DesignPatterns/`: Executable C# solution and console demonstration projects.
- `docs/adr/`: Architecture Decision Records tracking architectural invariants, design trade-offs, and decisions.
- `README.md`: Master reference catalog (1800+ lines) detailing 25+ design patterns with analogies, software engineering trade-offs, and sample implementations.

---

## 2. Inviolable Architectural Invariants

1. **Architecture Decision Records (ADRs) Compliance**:
   - All architectural conventions are formalized in [docs/adr/](docs/adr/README.md).
   - [ADR-0001](docs/adr/0001-record-architecture-decisions.md): Architecture decisions must be recorded as ADRs in `docs/adr/NNNN-title.md` and indexed in `docs/adr/README.md`. Never re-litigate accepted decisions without creating a superseding ADR.
   - [ADR-0002](docs/adr/0002-dotnet-modernization-and-pattern-hierarchy.md): Standardize on .NET 8 (LTS), modern C# 12+ idioms, standalone runnable console projects grouped by GoF category (`DesignPatterns/<Category>/<Pattern>/`), and the 5-point pedagogical catalog format.
2. **Modern Idiomatic C#**:
   - Code examples must follow modern C# best practices:
     - Nullable reference types enabled (`#nullable enable`).
     - Pattern matching (`switch` expressions, type patterns).
     - Readonly structs / records where immutability is desired.
     - `Lazy<T>` for thread-safe lazy initializations (e.g., Singleton).
3. **Pedagogical Structure**:
   - Every pattern added to the catalog or codebase must include:
     1. Real-life analogy (everyday metaphor).
     2. Software engineering scenario (real production use case).
     3. Problem statement (why anti-patterns fail).
     4. Solution & trade-offs (pros and cons).
     5. Complete, runnable C# code snippet.
4. **Thread-Safety & Resource Discipline**:
   - Resource-holding patterns (Object Pool, Unit of Work, Proxy) must implement `IDisposable` / `IAsyncDisposable`.
   - Concurrency-sensitive patterns must be thread-safe without creating deadlocks.

---

## 3. Commands Cheat Sheet

```bash
# Build pattern projects
dotnet build DesignPatterns/DesignPatterns.sln

# Run pattern executable demo
dotnet run --project DesignPatterns/DesignPatterns.csproj
```

---

## 4. Verification Gates for AI Agents

Before submitting code changes or finishing a turn:
1. Run `dotnet build` on the pattern solution — must compile with **0 errors**.
2. Ensure markdown documentation formatting and table-of-contents anchor links remain valid.
3. If introducing architectural shifts, new dependencies, or pattern reorganization, ensure an ADR is drafted in [docs/adr/](docs/adr/README.md) per [ADR-0001](docs/adr/0001-record-architecture-decisions.md).
