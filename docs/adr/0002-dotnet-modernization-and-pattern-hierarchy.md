# ADR-0002: .NET Modernization and Pattern Hierarchy

## Status
Accepted

## Date
2026-11-07

## Context
The repository provides a reference catalog of software design patterns in C#. Many online design pattern references remain anchored to legacy .NET Framework 4.x idioms (verbose class definitions, manual getters/setters, thread-unsafe singletons, mutable DTOs, missing nullability annotations). 

To serve as a production-grade educational and practical reference, the codebase requires clear conventions regarding:
1. Target .NET runtime version and C# language features.
2. Taxonomy and directory hierarchy of pattern implementations.
3. Code layout: runnable self-contained console projects vs. master documentation catalog.

## Decision
We establish the following architectural standards across `csharp-design-patterns`:

1. **Target Runtime & Language Baseline**:
   - Target framework: **.NET 8 (LTS)** with C# 12+ language features.
   - Nullable reference types enabled (`<Nullable>enable</Nullable>`) across all projects.
   - Implicit usings enabled (`<ImplicitUsings>enable</ImplicitUsings>`).
   - Idiomatic modern C# syntax:
     - `record` and `record struct` for immutable values/DTOs.
     - Collection expressions (`[...]`) and primary constructors where appropriate.
     - Pattern matching (`switch` expressions, relational/type patterns).
     - `Lazy<T>` for thread-safe lazy initialization.
     - Proper disposal semantics (`IDisposable` / `IAsyncDisposable`) for resource-managing patterns.

2. **Pattern Taxonomy & Hierarchy**:
   - Organize patterns into the 3 classic Gang of Four (GoF) categories, plus an architectural extension:
     - `DesignPatterns/CreationalPatterns/` (Singleton, Factory Method, Abstract Factory, Builder, Prototype).
     - `DesignPatterns/StructuralPatterns/` (Adapter, Bridge, Composite, Decorator, Facade, Flyweight, Proxy).
     - `DesignPatterns/BehavioralPatterns/` (Chain of Responsibility, Command, Interpreter, Iterator, Mediator, Memento, Observer, State, Strategy, Template Method, Visitor, Repository, Unit of Work).
   - Each pattern implementation lives in its own dedicated project directory with a `.csproj` and runnable `Program.cs` demonstration.
   - Master Visual Studio solution `DesignPatterns/DesignPatterns.sln` aggregates all pattern projects.

3. **Master Reference Catalog**:
   - `README.md` acts as the single-source pedagogical reference guide containing:
     - Real-life analogy.
     - Production software engineering scenario.
     - Problem statement (anti-pattern vs. pattern).
     - Runnable C# implementation code.
   - Pattern code in `README.md` mirrors the source code in `DesignPatterns/`.

## Alternatives Considered

### Single Monolithic Console App with Menu Switch
- **Pros**: One single `.csproj` to build.
- **Cons**: Namespace collisions between pattern components (e.g., multiple patterns defining a `Product`, `Customer`, or `Logger`), monolithic compilation, harder to inspect individual patterns in isolation.
- **Rejected**: Separate isolated projects per pattern preserve clean namespaces and realistic standalone usage.

### Retaining C# 7 / .NET Core 2.x Legacy Code
- **Pros**: Backward compatibility with legacy .NET projects.
- **Cons**: Fails to demonstrate how modern C# idioms simplify classic patterns (e.g., records eliminating boilerplate DTOs, `Lazy<T>` replacing double-checked locking, collection expressions streamlining composite trees).
- **Rejected**: Modern C# significantly improves pattern clarity, safety, and conciseness.

## Consequences
- All projects build cleanly under the .NET 8 SDK with zero warnings or errors.
- Every pattern is immediately executable via `dotnet run --project DesignPatterns/<Category>/<Pattern>/<Pattern>.csproj`.
- Clear navigation hierarchy for learners and AI agents navigating the codebase.
- Future patterns added must adhere to the 5-point pedagogical structure and modern C# invariants.
