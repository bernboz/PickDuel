# Pick Duel - Coding Standards

## 1. Overview

This document defines coding standards used throughout the Pick Duel project.

The goal is to create code that is:

- Readable
- Maintainable
- Consistent
- Easy for other developers to understand

---

# 2. General Principles

## Keep Code Simple

Prefer clear solutions over unnecessary complexity.

Avoid:

- Over-engineering
- Premature optimization
- Unnecessary abstractions

---

## Single Responsibility Principle

Classes, components, and services should have one clear purpose.

A class should not:

- Handle database access
- Process business logic
- Format UI data

all at the same time.

---

## Write Self-Documenting Code

Prefer meaningful names over excessive comments.

Example:

Good:

```csharp
CalculateLeagueRanking()
```

Bad:

```csharp
Process()
```

---

# 3. C# Standards

## Naming Conventions

Classes:

PascalCase

Example:

```csharp
LeagueService
PredictionController
```

---

Methods:

PascalCase

Example:

```csharp
CalculateRating()
```

---

Variables:

camelCase

Example:

```csharp
leagueId
predictionResult
```

---

Interfaces:

Prefix with I

Example:

```csharp
ILeagueRepository
```

---

## Async Programming

Use async/await for operations involving:

- Database calls
- API calls
- File operations

Example:

```csharp
public async Task<League> GetLeagueAsync()
```

Avoid blocking calls.

---

## Dependency Injection

Use dependency injection instead of manually creating dependencies.

Preferred:

```csharp
public LeagueService(ILeagueRepository repository)
{
}
```

Avoid:

```csharp
_repository = new LeagueRepository();
```

---

# 4. TypeScript / Angular Standards

## Naming

Classes:

PascalCase

Example:

```typescript
LeagueComponent
PredictionService
```

---

Variables:

camelCase

Example:

```typescript
leagueId
currentUser
```

---

Files:

kebab-case

Example:

```
league-card.component.ts
prediction.service.ts
```

---

# 5. Angular Guidelines

Components should:

- Focus on presentation
- Remain small
- Avoid large business logic

Services should handle:

- API communication
- Shared logic
- Data processing

---

Avoid:

Large components containing:

- API calls
- Business rules
- Complex calculations

---

# 6. Database Standards

## Naming

Tables:

Plural PascalCase

Examples:

```
Users

Leagues

Predictions
```

---

Columns:

PascalCase

Examples:

```
CreatedDate

UserId
```

---

# 7. Git Standards

Commits should be:

- Small
- Focused
- Descriptive

Preferred format:

```
type: description
```

Examples:

```
feat: add league creation endpoint

fix: correct prediction scoring calculation

docs: update architecture documentation
```

---

# 8. Testing Standards

Tests should:

- Have clear names
- Test one behavior
- Be easy to understand

Example:

```
Should_ReturnLeague_When_UserCreatesLeague()
```

---

# 9. Formatting Tools

The project will use automated formatting tools.

Frontend:

- Prettier
- ESLint

Backend:

- .NET formatting tools
- Rider formatting rules

Formatting should happen automatically whenever possible.

---

# 10. Code Review Expectations

Before merging changes:

Developers should verify:

- Code follows standards
- Tests pass
- No unnecessary complexity exists
- Documentation is updated when needed

---

# 11. Engineering Philosophy

The goal is not perfect code.

The goal is code that:

- Another developer can understand
- Can be safely changed
- Can grow with the application