# Pick Duel - Development Setup

## 1. Overview

This document describes how to configure a local development environment.

The goal is to ensure developers can reproduce the same environment consistently.

---

# 2. Required Tools

## Operating System

Recommended:

- macOS
- Linux
- Windows with WSL2

---

## Required Software

### Git

Used for:

- Version control
- Branch management
- Collaboration

---

### Node.js

Used for:

- Angular development
- Package management

Recommended version:

Node.js 24 LTS

Verify installation:

```
node --version

npm --version
```

---

### Angular CLI

Used for:

- Creating Angular applications
- Running development servers
- Building production applications

Verify:

```
ng version
```

---

### .NET SDK

Used for:

- ASP.NET Core development
- Building backend services

Verify:

```
dotnet --version
```

---

### Docker

Used for:

- Running containers
- Local databases
- Development environments

Verify:

```
docker --version
```

---

### IDEs

Recommended:

Frontend:

- Visual Studio Code

Backend:

- JetBrains Rider

---

# 3. Repository Setup

Clone the repository:

```
git clone <repository-url>
```

Navigate into the project:

```
cd sports-prediction-game
```

---

# 4. Project Structure

```
sports-prediction-game

├── src
│   ├── frontend
│   └── backend
│
├── docs
├── tests
├── docker
└── scripts
```

---

# 5. Frontend Setup

Navigate:

```
cd src/frontend
```

Install dependencies:

```
npm install
```

Run development server:

```
ng serve
```

---

# 6. Backend Setup

Navigate:

```
cd src/backend
```

Restore dependencies:

```
dotnet restore
```

Run API:

```
dotnet run
```

---

# 7. Database Setup

The database will be provided through Docker.

Development environments should not require manually installing database software.

---

# 8. Environment Variables

Sensitive configuration should not be committed.

Examples:

- Database credentials
- API keys
- Authentication secrets

Environment files should remain local.

---

# 9. Development Philosophy

Developers should prioritize:

- Reproducible environments
- Small changes
- Clear commits
- Automated testing
- Documentation