# Financial Control API

Financial control API built with .NET 8 using Clean Architecture principles.

## Technologies

- .NET 8
- ASP.NET Core
- Entity Framework Core
- SQL Server
- Docker
- Swagger

## Architecture

The project follows Clean Architecture with separation between:

- API
- Application
- Domain
- Infrastructure

## Running the project

### Docker SQL Server

```bash
docker compose up -d
```

### Apply migrations

```bash
dotnet ef database update --project FinancialControl.Infrastructure --startup-project FinancialControl.API
```

### Run API

```bash
dotnet run --project FinancialControl.API
```

## Features

- User creation
- Financial transactions
- JWT Authentication (coming soon)