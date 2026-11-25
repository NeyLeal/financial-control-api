# Financial Control API

Personal finance REST API built with .NET 8 and Clean Architecture.

The frontend is available at [financial-control-web](https://github.com/NeyLeal/financial-control-web).

## Technologies

- .NET 8 and ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- JWT authentication and BCrypt password hashing
- FluentValidation
- xUnit, Moq and FluentAssertions
- Docker
- Swagger

## Architecture

The solution is split into:

- **API**: controllers, middlewares and authentication setup
- **Application**: use cases, DTOs, validators and interfaces
- **Domain**: entities and enums
- **Infrastructure**: EF Core, repositories and JWT generation
- **Tests**: unit tests for use cases and controllers

## Features

- User creation and login with JWT
- Protected category and transaction endpoints
- Category and transaction CRUD
- Financial summary and dashboard
- Request validation, error handling and request logging

## Endpoints

| Method | Route | Description | Auth |
| --- | --- | --- | --- |
| POST | `/Users` | Create a user | No |
| POST | `/Auth/login` | Log in and get a JWT | No |
| GET, POST | `/Categories` | List and create categories | Yes |
| PUT, DELETE | `/Categories/{id}` | Update and delete a category | Yes |
| GET, POST | `/Transactions` | List and create transactions | Yes |
| PUT, DELETE | `/Transactions/{id}` | Update and delete a transaction | Yes |
| GET | `/Transactions/summary` | Financial summary | Yes |
| GET | `/Transactions/dashboard` | Dashboard data | Yes |

## Running the project

### 1. Start SQL Server

```bash
docker compose up -d
```

### 2. Configure the connection string and the JWT key

Both values are empty in `appsettings.json`. Set them with environment variables (PowerShell):

```powershell
$env:ConnectionStrings__DefaultConnection = "<your connection string>"
$env:Jwt__Key = "<a random key with at least 32 characters>"
```

### 3. Apply migrations

```bash
dotnet ef database update --project FinancialControl.Infrastructure --startup-project FinancialControl.API
```

### 4. Run the API

```bash
dotnet run --project FinancialControl.API
```

In development, open `/swagger` on the address shown in the terminal.

## Running the tests

```bash
dotnet test
```
