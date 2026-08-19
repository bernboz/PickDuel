# PickDuel

PickDuel is a competitive sports prediction platform where users compete in head-to-head prediction leagues with friends.

Users make predictions on NFL and NBA games, earn or lose rating points based on prediction accuracy, and compete throughout a season to become the top predictor.

The goal of PickDuel is to combine the competitive experience of fantasy sports with the strategy and analysis of sports prediction.

---

# Features

## Current Development Status

Backend domain and application architecture actively under development.

Completed foundations:

* Clean Architecture backend structure
* Domain-driven entity design
* Prediction and scoring domain models
* Playoff bracket domain models
* Application service layer
* Repository abstractions
* Unit testing infrastructure
* Extensible scoring rule architecture

Planned features:

* User accounts and authentication
* Sports prediction leagues
* NFL and NBA pick'em competitions
* Friend-based competitions
* Elo-inspired rating system
* Dynamic leaderboards
* Season rankings
* Playoff tournaments
* Advanced prediction scoring
* Real-time game result processing

---

# Technology Stack

## Frontend

* Angular
* TypeScript
* HTML/CSS

## Backend

* ASP.NET Core Web API
* C#
* Entity Framework Core
* Clean Architecture principles

## Database

* PostgreSQL

## Development Tools

* Docker
* JetBrains Rider
* Visual Studio Code
* GitHub
* Azure DevOps

---

# Architecture

PickDuel follows Clean Architecture principles with a focus on separation of concerns, maintainability, and testability.

High-level architecture:

```
Frontend
   |
   |
ASP.NET Core Web API
   |
   |
Application Layer
   |
   |
Domain Layer
   |
   |
Infrastructure Layer
   |
   |
Database
```

## Layer Responsibilities

### Domain Layer

Contains the core business logic and rules of the application.

Examples:

* Users
* Leagues
* Games
* Picks
* Predictions
* Scoring models
* Playoff structures

The domain layer has no dependency on external frameworks or infrastructure.

---

### Application Layer

Contains application workflows and business operations.

Examples:

* Pick creation and management
* Prediction processing
* Scoring services
* Score event generation
* Application interfaces

The application layer coordinates domain behavior while remaining independent of database implementation details.

---

### Infrastructure Layer

Contains external concerns and implementations.

Examples:

* Database access
* Repository implementations
* Entity Framework Core configuration
* External service integrations

---

### API Layer

Provides HTTP endpoints for client applications.

Responsibilities include:

* Request handling
* Validation
* Authentication
* Response formatting

---

# Repository Structure

```
PickDuel

├── src
│   ├── frontend
│   │
│   └── backend
│       ├── PickDuel.Api
│       ├── PickDuel.Application
│       ├── PickDuel.Domain
│       ├── PickDuel.Infrastructure
│       └── PickDuel.Tests
│
├── docs
│
├── docker
│
└── scripts
```

---

# Backend Structure

The backend follows a layered Clean Architecture organization:

```
PickDuel.Domain
│
├── Entities
├── ValueObjects
├── Enums
└── Common


PickDuel.Application
│
├── Services
├── Interfaces
├── Scoring
└── Application Workflows


PickDuel.Infrastructure
│
├── Repositories
├── Database
└── External Integrations


PickDuel.Api
│
├── Controllers
├── DTOs
└── Configuration
```

---

# Testing

The project uses automated unit testing to verify domain and application behavior.

Testing stack:

* NUnit
* NSubstitute

Current testing coverage includes:

* Domain entity validation
* Prediction workflows
* Scoring rules
* Scoring services
* Application services
* Playoff logic

Testing goals:

* Protect business rules
* Prevent regressions
* Maintain confidence while expanding features

---

# Documentation

Project documentation can be found in:

```
docs/
```

Current documentation includes:

* Product vision
* Architecture decisions
* Development setup
* Coding standards
* Git workflow
* Feature roadmap
* Sprint notes
* Learning logs

---

# Development Setup

Development setup instructions:

```
docs/02-Engineering/Development Setup.md
```

The development environment uses:

* Docker-based services
* .NET tooling
* Angular tooling
* PostgreSQL database infrastructure

---

# Engineering Principles

PickDuel is built with the following principles:

* Clean Architecture
* Domain-driven design
* SOLID principles
* Dependency inversion
* Interface-driven development
* Automated testing
* Maintainable and scalable code structure

---

# Project Goals

This project is being built to demonstrate:

* Full-stack software development
* Professional backend architecture
* Modern API development
* Database-driven application design
* Cloud-ready engineering practices
* Software testing methodology
* Product-focused development

---

# Current Status

PickDuel is currently in active backend development.

Completed:

* Domain model foundation
* Prediction system foundation
* Scoring architecture
* Playoff domain models
* Application service foundation

Next development milestones:

* Database integration
* Repository implementations
* API controllers
* Authentication
* Frontend application development
* Production deployment workflow
