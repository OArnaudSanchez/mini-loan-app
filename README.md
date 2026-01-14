# Mini Loan Management System

This repository contains the implementation of a **Loan Management System**, developed woth .NET.  
The solution includes a **.NET Web API backend** and a **simplified Angular frontend**, with a focus on clean architecture, testability, and maintainability.

---

## Architecture Overview

The backend follows a **Clean Architecture** approach, with clear separation of concerns:

```
/src
/src/Fundo.Applications.Domain         // Domain entities and business rules
/src/Fundo.Applications.Application    // CQRS, MediatR, validators, DTOs
/src/Fundo.Applications.Infrastructure // EF Core, repositories (Repository pattern and Unit of work), persistence
/src/Fundo.Applications.WebApi         // API layer (controllers, middleware)
```

Key architectural decisions:
- CQRS with **MediatR**
- **Entity Framework Core** for persistence
- **FluentValidation** for request validation
- **AutoMapper** for entity-to-DTO mapping
- Global exception handling middleware
- Unit and integration testing with **xUnit**

---

## Running the Project

### Prerequisites

- .NET SDK **8.0+**
- Node.js **20.x (LTS)**
- Docker & Docker Compose**
---

### Run Backend with Docker

From the repository root:

```bash
docker-compose up --build
```

This will start:
- The Web API
- SQL Server (containerized)

---

### Run Frontend (Angular)

```bash
cd frontend
npm install
npm start
```

The frontend will be available at:

```
http://localhost:4200
```

---

## Running Tests

```bash
dotnet test
```

Testing strategy:
- Unit tests for application layer handlers and domain logic
- Integration tests for API endpoints using `WebApplicationFactory`
- Integration tests use **EF Core InMemory** to avoid external dependencies

---

## Validation & Error Handling

- Input validation via **FluentValidation**
- Consistent error responses through a global exception middleware
- Clear separation between validation errors, domain errors, and unexpected failures
- Explicit CORS configuration

---

## Implemented Features

### Backend
- Create loan
- Retrieve loan by ID
- List all loans
- Apply payments
- Seed data for development
- Dockerized backend
- Structured logging
- Unit and integration tests

### Frontend
- Display list of loans
- Basic interaction with backend API
