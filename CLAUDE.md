# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

A .NET 9 Minimal API with xUnit tests (unit + integration), used as a CI/CD example with GitHub Actions.

## Structure

```
src/CICDExample.Api/         # Minimal API (ItemService + 5 endpoints)
tests/CICDExample.Tests/
  Unit/                      # ItemService unit tests
  Integration/               # WebApplicationFactory integration tests
.github/workflows/ci.yml     # GitHub Actions: restore → build → test
```

## Common Commands

```bash
dotnet build                         # Build the solution
dotnet test                          # Run all 13 tests
dotnet test --filter "Unit"          # Run only unit tests
dotnet test --filter "Integration"   # Run only integration tests
dotnet run --project src/CICDExample.Api
```

## Architecture

- `ItemService` is a singleton in-memory store with a `List<Item>` — no database.
- `Program.cs` exposes 5 routes: `GET /health`, `GET /items`, `GET /items/{id}`, `POST /items`, `DELETE /items/{id}`.
- `public partial class Program {}` at the end of `Program.cs` exposes the type for `WebApplicationFactory<Program>` in integration tests.
- Solution uses the newer `.slnx` XML format; `dotnet sln` (old `.sln` commands) do not apply.
