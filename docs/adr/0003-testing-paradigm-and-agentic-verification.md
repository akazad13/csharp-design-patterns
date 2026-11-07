# ADR-0003: Testing Paradigm and Agentic Verification

## Status
Accepted

## Date
2026-11-07

## Context
The `csharp-design-patterns` repository provides an exhaustive, production-oriented reference of Gang of Four (GoF) and architectural software patterns in modern C#. Historically, pattern implementations in this repository were verified primarily through standalone executable console applications (`Program.cs`) that print execution traces to standard output.

While console demonstrations provide immediate visual intuition for human readers, they are insufficient for rigorous software engineering and autonomous AI agent workflows:
1. **Lack of Automated Agentic Verification**: Autonomous AI agents (Antigravity, Claude Code, Copilot, Cursor) require deterministic, machine-verifiable gates (`dotnet test`) rather than qualitative inspection of terminal output.
2. **The Fragility of Interaction Mocking**: Many conventional .NET test suites rely heavily on interaction-based mocking libraries (e.g., `mock.Verify(x => x.SomeMethod(), Times.Once)`). Verifying method call sequences couples test suites to internal implementation details, causing tests to break during harmless refactorings even when observable behavior is preserved.
3. **Third-Party Mocking Vulnerabilities & Bloat**: Certain third-party mocking libraries (such as `Moq` during the SponsorLink incident) have introduced supply-chain risks, opaque telemetry, or unnecessary dynamic proxy overhead.
4. **Contract Isolation Requirements**: Design patterns embody polymorphic contracts (abstract factories, interchangeable strategies, recursive composites, structural adapters, state machines). Tests must verify that these implementations strictly fulfill their behavioral contracts across diverse input states.

To establish a world-class standard for pattern testing and empower autonomous AI agents with a robust verification loop, we require a formal testing paradigm and clear technological invariants.

## Decision
We adopt a comprehensive **Pattern Testing Paradigm & Agentic Verification Framework** rooted in Test-Driven Development (TDD) principles:

### 1. Behavioral Verification Over Method Call Assertions
- **Test Outcomes, Not Interactions**: Tests must assert on the observable outcome (returned values, state changes, published events, or handled exceptions) of an operation, rather than inspecting which private or internal methods were called.
- **Refactoring Resilience**: Tests should remain green when the internal mechanics of a pattern are optimized or refactored, as long as the external behavioral contract remains intact.
- **DAMP over DRY**: Test specifications prioritize **DAMP (Descriptive And Meaningful Phrases)** clarity over extreme DRY deduplication. Each test should clearly explain the scenario under test using the Arrange-Act-Assert (AAA) pattern.

### 2. Contract Isolation & Pattern Verification Invariants
Every pattern category has distinct behavioral contracts that unit tests must verify deterministically:
- **Creational Patterns** (Abstract Factory, Factory Method, Builder, Prototype, Singleton):
  - Verify that concrete products fulfill abstract interface contracts.
  - Verify polymorphic factory interchangeability (e.g., swapping `IndiaShoppingCartPurchaseFactory` for `ChinaShoppingCartPurchaseFactory` cleanly updates tax and shipping calculations).
  - Verify instance identity guarantees (e.g., `ReferenceEquals` for Singletons across multiple concurrent accesses; deep/shallow copy boundaries for Prototype).
- **Structural Patterns** (Adapter, Bridge, Composite, Decorator, Facade, Flyweight, Proxy):
  - Verify interface compliance and transparent substitution (e.g., a Decorator must preserve component contracts while correctly augmenting behaviors).
  - Verify translation accuracy in Adapters without leaking adaptee-specific dependencies.
  - Verify recursive invariant aggregation in Composite trees regardless of hierarchy depth.
  - Verify lazy initialization, access control, or caching in Proxy and Flyweight implementations.
- **Behavioral Patterns** (Chain of Responsibility, Command, Iterator, Mediator, Memento, Observer, State, Strategy, Template Method, Visitor):
  - Verify algorithmic substitution (e.g., switching Strategy implementations changes algorithmic outputs deterministically).
  - Verify state transitions and event propagation (e.g., Observers receive state updates without reference leaks; State transitions adhere to valid state machines).
  - Verify undo/redo state restoration in Command and Memento.

### 3. Technological Invariants
- **Testing Framework**: Standardize on **xUnit** (`xunit`, `xunit.runner.visualstudio`, `Microsoft.NET.Test.Sdk`) targeting **.NET 8+**.
- **Banned Packages Policy**:
  - **`Moq` is strictly prohibited** due to supply-chain risk and anti-pattern enforcement.
  - **Zero-Mock Default**: Prioritize **real implementations** and **pure C# lightweight test doubles** (in-memory fakes, dummy objects, or test stubs) over dynamic proxy mocking frameworks.
  - If a dynamic test double is strictly required for isolating an external system boundary, `NSubstitute` is the only approved framework.
- **Pure & Fast Execution**:
  - Tests must be **Small (in-memory)** unit tests that run in milliseconds.
  - Zero external I/O, zero network dependencies, zero persistent file system access, and zero non-deterministic delays (`Thread.Sleep` is prohibited).
  - Test suites must be fully parallelizable and isolated (no shared static mutable state between test executions).

### 4. Agentic Verification Workflow
Autonomous AI agents operating in the repository must adhere to the Red-Green-Refactor lifecycle:
1. **RED**: Write a failing xUnit test capturing the required pattern contract, edge case, or bug reproduction.
2. **GREEN**: Implement the minimal modern C# code necessary to pass the test.
3. **REFACTOR**: Modernize and simplify code (applying C# 12+ idioms, records, pattern matching) while ensuring all tests stay green.
4. **GATE**: Execute `dotnet test` before marking any implementation task complete.

## Alternatives Considered

### Retaining Only Console Applications (`Program.cs`)
- **Pros**: Zero additional test projects; simple visual demonstration for human readers.
- **Cons**: No automated regression gating; impossible for AI agents or CI pipelines to verify behavior reliably.
- **Rejected**: Console apps serve a pedagogical demonstration role but do not provide engineering verification.

### Interaction-Based Mocking with Moq
- **Pros**: Common historical standard in older .NET enterprise codebases.
- **Cons**: Promotes brittle tests that assert on call counts rather than business logic; historical supply-chain security issues (SponsorLink); adds heavy dynamic proxy dependencies.
- **Rejected**: State-based behavioral verification with pure C# doubles is more resilient, secure, and maintainable.

### Heavy End-to-End / Integration Suites
- **Pros**: Tests real-world database or external integration boundaries.
- **Cons**: Slows down verification cycle; introduces network/environment flakiness; unnecessary for validating core GoF design pattern mechanics.
- **Rejected**: Design patterns represent pure in-memory object-oriented structures; unit tests must remain instantaneous and deterministic.

## Consequences
- Every design pattern can be verified autonomously via `dotnet test` with machine-readable, deterministic assertions.
- Pattern refactoring can occur safely without breaking test suites anchored to private implementation details.
- The repository remains secure and lean, free of banned packages and heavy dynamic mocking dependencies.
- AI agents possess a verifiable, automated testing loop that guarantees high code quality across all pattern additions and modernizations.
