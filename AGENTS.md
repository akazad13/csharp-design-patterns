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
- `README.md`: Master reference catalog (1800+ lines) detailing 25+ design patterns with analogies, software engineering trade-offs, and sample implementations.

---

## 2. Inviolable Architectural Invariants

1. **Modern Idiomatic C#**:
   - Code examples must follow modern C# best practices:
     - Nullable reference types enabled (`#nullable enable`).
     - Pattern matching (`switch` expressions, type patterns).
     - Readonly structs / records where immutability is desired.
     - `Lazy<T>` for thread-safe lazy initializations (e.g., Singleton).
2. **Pedagogical Structure**:
   - Every pattern added to the catalog or codebase must include:
     1. Real-life analogy (everyday metaphor).
     2. Software engineering scenario (real production use case).
     3. Problem statement (why anti-patterns fail).
     4. Solution & trade-offs (pros and cons).
     5. Complete, runnable C# code snippet.
3. **Thread-Safety & Resource Discipline**:
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
