# Pick Duel - Git Workflow

## 1. Overview

This document defines the Git workflow used for the Pick Duel project.

The goal is to maintain a clean, organized development process similar to professional software teams.

---

# 2. Branch Strategy

The project uses a simplified Git Flow approach.

Primary branches:

```
main
|
|-- Production-ready code

dev
|
|-- Active development branch
```

---

# 3. Main Branch

The `main` branch represents stable code.

Rules:

- Code should always build successfully
- Only completed features should be merged
- Changes should come through pull requests

The main branch represents code that could be deployed.

---

# 4. Development Branch

The `dev` branch is used for ongoing development.

Purpose:

- Combine completed features
- Test changes together
- Prepare future releases

Developers normally branch from `dev`.

---

# 5. Feature Branches

New work should be completed in feature branches.

Naming format:

```
feature/<description>
```

Examples:

```
feature/create-league

feature/add-authentication

feature/prediction-ranking-system
```

---

# 6. Bug Fix Branches

Bug fixes use:

```
fix/<description>
```

Examples:

```
fix/incorrect-rating-calculation

fix/login-validation-error
```

---

# 7. Commit Standards

Commits should follow:

```
type: description
```

Common types:

```
feat:
fix:
docs:
refactor:
test:
chore:
```

Examples:

```
feat: add league creation API

fix: correct Elo rating calculation

docs: update backend architecture

test: add prediction service tests
```

---

# 8. Commit Guidelines

Good commits:

- Are small
- Represent one logical change
- Explain what changed

Avoid:

```
updated stuff

fixed things

changes
```

Prefer:

```
feat: create league database model
```

---

# 9. Pull Request Workflow

The workflow:

```
Create Feature Branch

        ↓

Develop Feature

        ↓

Commit Changes

        ↓

Push Branch

        ↓

Create Pull Request

        ↓

Review Changes

        ↓

Merge Into Dev

        ↓

Release To Main
```

---

# 10. Azure DevOps Integration

Azure DevOps work items should be connected to development changes.

Example:

User Story:

```
Create League Creation Feature
```

Branch:

```
feature/create-league
```

Commit:

```
feat: add league creation endpoint
```

Pull Request:

```
Complete league creation workflow
```

This creates traceability between planning and implementation.

---

# 11. Merge Strategy

Preferred approach:

- Pull requests required
- Review before merging
- Resolve conflicts before merge

Avoid directly committing to:

```
main
```

or

```
dev
```

---

# 12. Release Workflow

Future releases:

```
dev

↓

Testing

↓

main

↓

Deployment
```

---

# 13. Development Philosophy

The Git workflow exists to:

- Protect stable code
- Make changes easier to review
- Maintain project history
- Simulate professional team practices