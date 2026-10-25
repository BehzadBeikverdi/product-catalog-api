# Product Catalog API

A RESTful Product Catalog API built with **ASP.NET Core 10**, **Entity Framework Core**, and **Microsoft SQL Server**. The project follows a layered architecture with the Repository, Unit of Work, and Service patterns to keep the code organized, maintainable, and testable.

## Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Tech Stack](#tech-stack)
- [Architecture](#architecture)
- [Project Structure](#project-structure)
- [Prerequisites](#prerequisites)
- [Getting Started](#getting-started)
- [Database Setup](#database-setup)
- [Configuration](#configuration)
- [Database Migrations](#database-migrations)
- [API Endpoints](#api-endpoints)
- [Example Requests](#example-requests)
- [Error Handling](#error-handling)
- [Development](#development)
- [Git Ignore](#git-ignore)
- [Future Improvements](#future-improvements)
- [License](#license)

## Overview

Product Catalog API is a backend application for managing product catalog data through HTTP endpoints.

The project is designed to practice and demonstrate backend engineering principles, including:

- RESTful API development.
- Separation of concerns through layered architecture.
- Database access with Entity Framework Core.
- Repository and Unit of Work patterns.
- Request and response DTOs.
- Database schema management with EF Core migrations.
- Containerized database development with Docker.

## Features

- Create products.
- Retrieve the product catalog.
- Retrieve individual products by ID.
- Centralized database configuration.
- Standardized API response models.
- Shared base entity for common entity properties.
- UTC timestamps for entity creation and updates.
- SQL Server integration using Entity Framework Core.
- Version property for entity concurrency management.

> The availability of individual endpoints depends on the current implementation.

## Tech Stack

| Technology | Purpose |
|---|---|
| C# | Primary programming language |
| .NET 10 | Application framework |
| ASP.NET Core Web API | HTTP API development |
| Entity Framework Core | Object-relational mapping |
| Microsoft SQL Server 2022 | Relational database |
| Docker | Database containerization |
| SQL | Relational data storage |
| Git | Version control |
| JetBrains Rider | Development environment |

## Architecture

The application follows a layered architecture that separates HTTP handling, business logic, and data access.

```text
Client
  |
  v
API / Controller Layer
  |
  v
Service Layer
  |
  v
Repository Layer
  |
  v
Unit of Work
  |
  v
Entity Framework Core
  |
  v
SQL Server
```

### API / Controller Layer

Responsible for handling HTTP requests, accepting request models, invoking application services, and returning appropriate HTTP responses.

### Service Layer

Contains application and business logic. It coordinates operations, maps entities to response DTOs, and uses the repository and Unit of Work abstractions.

### Repository Layer

Encapsulates data access operations and reduces direct database-querying concerns in the service layer.

### Unit of Work

Coordinates persistence operations and provides a consistent point for committing changes to the database.

### Entity Framework Core

Maps C# entities to relational database tables and manages database operations through `DbContext`.

## Project Structure

The following is a conceptual structure; adjust directory names to match the actual repository.

```text
ProductCatalog/
├── Controllers/
├── Services/
│   ├── Interfaces/
│   └── Implementations/
├── Repositories/
│   ├── Interfaces/
│   └── Implementations/
├── Models/
│   ├── Requests/
│   └── Responses/
├── Data/
│   ├── AppDbContext.cs
│   └── Migrations/
├── UnitOfWork/
├── appsettings.json
├── Program.cs
├── .gitignore
└── ProductCatalog.csproj
```

## Prerequisites

Before running the application, make sure you have:

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Docker](https://www.docker.com/) and Docker Compose support (optional if SQL Server is installed locally)
- [Git](https://git-scm.com/)
- An IDE such as [JetBrains Rider](https://www.jetbrains.com/rider/)

Verify your .NET installation:

```bash
dotnet --version
```

## Getting Started

### 1. Clone the repository

```bash
git clone https://github.com/BehzadBeikverdi/product-catalog-api.git
cd product-catalog-api
```

### 2. Restore dependencies

```bash
dotnet restore
```

### 3. Configure the database connection

Configure the `DefaultConnection` connection string as described in the [Configuration](#configuration) section.

### 4. Apply database migrations

```bash
dotnet ef database update
```

If no migrations exist yet, create the initial migration first:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### 5. Run the application

```bash
dotnet run
```

Alternatively, run the project using the Run configuration in JetBrains Rider.

Use the URLs printed in the terminal to access the API. If Swagger or OpenAPI is configured, use the URL provided by the application.

## Database Setup

The development database runs on Microsoft SQL Server 2022 in Docker.

### Run SQL Server in Docker

If you do not already have a SQL Server container, create one:

```bash
docker run \
  --name productcatalog-sql \
  -e ACCEPT_EULA=Y \
  -e MSSQL_SA_PASSWORD='YOUR_STRONG_PASSWORD' \
  -p 1433:1433 \
  -v productcatalog_sql_data:/var/opt/mssql \
  -d mcr.microsoft.com/mssql/server:2022-latest
```

Replace `YOUR_STRONG_PASSWORD` with a strong password that meets SQL Server's password requirements.

The named volume preserves database files when the container is stopped or recreated.

### Verify the container

```bash
docker ps
```

View SQL Server container logs:

```bash
docker logs productcatalog-sql
```

Stop and start the container:

```bash
docker stop productcatalog-sql
docker start productcatalog-sql
```

If your existing container has been renamed to `sql-server`, use that name in these commands instead.

### Database Details

| Setting | Value |
|---|---|
| Database engine | Microsoft SQL Server |
| Version | SQL Server 2022 |
| Host | `localhost` |
| Port | `1433` |
| Database | `ProductCatalogDb` |
| Authentication | SQL Server authentication |
| Username | `sa` |

The application database is created when the EF Core migrations are successfully applied, provided the SQL Server login has the necessary permissions.

## Configuration

The application reads its connection string from `appsettings.json`.

Example configuration:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=ProductCatalogDb;User Id=sa;Password=YOUR_STRONG_PASSWORD;Encrypt=True;TrustServerCertificate=True;"
  },
  "AllowedHosts": "*"
}
```

Replace `YOUR_STRONG_PASSWORD` with your actual local SQL Server password.

The `ConnectionStrings` section must be at the root of the JSON document, not nested inside `Logging`.

### Database Registration

The `DbContext` is registered in `Program.cs` using the configured connection string:

```csharp
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));
```

### Security

**Never commit real passwords, production connection strings, or other secrets to version control.**

For local development, use .NET User Secrets:

```bash
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "YOUR_CONNECTION_STRING"
```

Alternatively, configure the connection string through an environment variable:

```bash
export ConnectionStrings__DefaultConnection='YOUR_CONNECTION_STRING'
```

ASP.NET Core configuration supports the double underscore (`__`) convention for mapping environment variables to nested configuration keys.

For production deployments, use an appropriate secret-management solution.

## Database Migrations

EF Core migrations track changes to the database schema.

Install the EF Core CLI tool if needed:

```bash
dotnet tool install --global dotnet-ef
```

If it is already installed, update it when appropriate:

```bash
dotnet tool update --global dotnet-ef
```

Install the design-time package with a version compatible with your project's EF Core packages:

```bash
dotnet add package Microsoft.EntityFrameworkCore.Design
```

Useful commands:

| Command | Description |
|---|---|
| `dotnet ef migrations add InitialCreate` | Creates the initial migration |
| `dotnet ef migrations list` | Lists existing migrations |
| `dotnet ef database update` | Applies pending migrations |
| `dotnet ef migrations remove` | Removes the last unapplied migration |
| `dotnet ef database drop` | Deletes the configured database |

**Warning:** `dotnet ef database drop` deletes the database. Use it only when you intentionally want to remove that database.

## API Endpoints

The following endpoints describe the intended product retrieval and creation API. Confirm the route attributes in the controllers before relying on them.

| Method | Endpoint | Description |
|---|---|---|
| `POST` | `/api/products` | Create a product |
| `GET` | `/api/products` | Retrieve all products |
| `GET` | `/api/products/{id}` | Retrieve a product by ID |

The exact base path depends on the controller's route configuration.

## Example Requests

### Create a Product

**Request**

```http
POST /api/products
Content-Type: application/json
```

```json
{
  "name": "Mechanical Keyboard",
  "description": "A mechanical keyboard for everyday use.",
  "price": 89.99,
  "stock": 25
}
```

**Example response**

The response structure depends on the implementation of the response wrapper and controller:

```json
{
  "message": "Product created successfully",
  "payload": {
    "id": "00000000-0000-0000-0000-000000000001",
    "name": "Mechanical Keyboard",
    "description": "A mechanical keyboard for everyday use.",
    "price": 89.99,
    "stock": 25,
    "createdAtUtc": "2026-01-01T10:00:00Z",
    "updatedAtUtc": "2026-01-01T10:00:00Z"
  }
}
```

The ID and timestamps above are illustrative, not actual values returned by the API.

### Retrieve All Products

**Request**

```http
GET /api/products
```

This endpoint returns the products available in the catalog, according to the configured response model.

### Retrieve a Product by ID

**Request**

```http
GET /api/products/00000000-0000-0000-0000-000000000001
```

Replace the example GUID with a product ID returned by the API.

## Data Model

The shared `BaseEntity` provides common properties for entities:

| Property | Type | Purpose |
|---|---|---|
| `Id` | `Guid` | Entity identifier |
| `Version` | `int` | Version or concurrency-related metadata |
| `CreatedAtUtc` | `DateTime` | Creation timestamp in UTC |
| `UpdatedAtUtc` | `DateTime` | Last-update timestamp in UTC |

The `Product` entity inherits from `BaseEntity` and includes product-specific fields such as name, description, price, and stock.

Request and response DTOs separate the external API contract from the internal database entity.

## Error Handling

The API should return appropriate HTTP status codes for different outcomes.

| Status code | Meaning |
|---|---|
| `200 OK` | Request completed successfully |
| `201 Created` | Resource created successfully |
| `400 Bad Request` | Request data is invalid |
| `404 Not Found` | Requested product does not exist |
| `500 Internal Server Error` | Unexpected server-side failure |

Actual status codes depend on the controller and service implementation. Validation and centralized exception handling can be added to make error responses consistent.

## Development

Build the application:

```bash
dotnet build
```

Run the application:

```bash
dotnet run
```

Run automated tests, if a test project is available:

```bash
dotnet test
```

Check installed package versions:

```bash
dotnet list package
```

Review dependency vulnerabilities, including transitive dependencies:

```bash
dotnet list package --include-transitive
```

## Git Ignore

The repository should exclude generated build files and IDE-specific configuration.

Example `.gitignore` entries:

```gitignore
**/bin/
**/obj/
.idea/
```

These rules keep .NET build artifacts and JetBrains IDE project files out of version control.

## Future Improvements

Potential enhancements for the project include:

- Request validation using data annotations or FluentValidation.
- Global exception handling and consistent error responses.
- Pagination, filtering, and sorting.
- Product update and deletion endpoints.
- Automated unit and integration tests.
- Structured logging and health checks.
- API documentation with Swagger/OpenAPI.
- Optimistic concurrency handling using the entity version.
- CI/CD with GitHub Actions.
- Production-ready configuration and secret management.

## License

No license has been specified yet. Add a `LICENSE` file and update this section when a license has been chosen.
