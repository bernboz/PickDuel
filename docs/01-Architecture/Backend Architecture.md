# Pick Duel - Backend Architecture

## 1. Overview

The backend will be built using ASP.NET Core Web API with C#.

The backend will follow Clean Architecture principles to create a system that is:

- Maintainable
- Testable
- Scalable
- Easy to extend
- Aligned with professional software development practices

The backend will expose REST APIs consumed by the Angular frontend.

---

# 2. Backend Technology Stack

## Core Technologies

- C#
- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL
- Docker

## Supporting Technologies

- JWT Authentication
- Swagger/OpenAPI
- Automated Testing
- Dependency Injection
- Logging

---

# 3. Clean Architecture Overview

The backend will be divided into four primary projects:

```
SportsPrediction.sln

|
|-- SportsPrediction.Api
|
|-- SportsPrediction.Application
|
|-- SportsPrediction.Domain
|
|-- SportsPrediction.Infrastructure
```

Each project has a specific responsibility.

---

# 4. Project Responsibilities

# SportsPrediction.Domain

## Purpose

The Domain layer contains the core business logic of the application.

This is the most important layer.

It represents the actual sports prediction business rules.

## Contains

Entities:

- User
- League
- LeagueMember
- Game
- Prediction
- Rating
- Season

Value Objects:

- Rating calculation details
- Prediction results

Domain Rules:

- How rankings work
- How points are awarded
- League rules

## Dependencies

The Domain layer should depend on nothing.

It should not know about:

- Databases
- APIs
- Controllers
- Frameworks

---

# SportsPrediction.Application

## Purpose

The Application layer contains the actions users can perform.

It coordinates business workflows.

## Examples

Creating a league:

```
Controller
    |
Application Service
    |
Domain Rules
```

Submitting a prediction:

```
User submits pick

Application Layer:
- Validate league membership
- Check deadline
- Save prediction request

Domain Layer:
- Apply prediction rules
```

## Contains

- Application services
- Interfaces
- DTOs
- Business workflows
- Validation logic

---

# SportsPrediction.Infrastructure

## Purpose

The Infrastructure layer handles external dependencies.

## Contains

Database:

- Entity Framework Core
- Database migrations
- Repository implementations

External services:

- Sports data APIs
- Email services
- Cloud services

The Infrastructure layer implements interfaces defined by the Application layer.

---

# SportsPrediction.Api

## Purpose

The API layer handles communication with external clients.

## Contains

- Controllers
- Middleware
- Authentication setup
- Dependency injection configuration
- API documentation

Examples:

```
GET /api/leagues

POST /api/predictions

GET /api/leaderboards
```

---

# 5. Dependency Direction

Dependencies should always point inward.

Correct:

```
API
 |
Infrastructure
 |
Application
 |
Domain
```

The Domain layer should never depend on outer layers.

This protects business logic from technology changes.

Example:

Changing PostgreSQL to SQL Server should not require changing the Domain layer.

---

# 6. API Design Principles

The backend will follow REST principles.

Guidelines:

- Use meaningful resource names
- Use HTTP methods correctly
- Return appropriate status codes
- Validate incoming requests
- Keep responses consistent

Examples:

```
GET    /api/leagues

POST   /api/leagues

GET    /api/leagues/{id}

POST   /api/leagues/{id}/predictions
```

---

# 7. Database Access Strategy

Entity Framework Core will be used for database communication.

Initial approach:

- Code-first migrations
- Repository pattern where useful
- LINQ queries
- Async database operations

The design should avoid unnecessary abstraction while maintaining clean separation.

---

# 8. Testing Strategy

Testing will be implemented at multiple levels.

## Unit Tests

Focus:

- Domain rules
- Rating calculations
- Prediction logic

## Integration Tests

Focus:

- API endpoints
- Database interactions

## End-to-End Tests

Focus:

- Complete user workflows

Example:

User joins league → makes prediction → receives rating update

---

# 9. Future Backend Considerations

Potential future improvements:

- Background jobs for game updates
- Caching with Redis
- Message queues
- Microservice extraction if needed
- Advanced analytics processing

These will only be introduced when complexity requires them.

---

# 10. Backend Goals

The backend should demonstrate:

- Professional architecture
- Strong object-oriented design
- Clean separation of responsibilities
- Secure API development
- Modern ASP.NET Core practices

The goal is not to over-engineer the application, but to create a foundation that could realistically grow into a production system.