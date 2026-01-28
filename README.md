# Assessment

A full-stack **User Management** application:

- **Backend:** ASP.NET Core Web API (.NET 8) + Entity Framework Core
- **Frontend:** React + TypeScript + Vite

The API manages **Users**, **Groups**, and **Permissions** (CRUD + many-to-many relationships) and exposes a small **Dashboard** endpoint for aggregated counts.

## Solution structure


.
![Solution structure](docs/solution-structure.svg)


## Key technical decisions

- **Backend: ASP.NET Core (.NET 8)**
  - Chosen for fast development of REST endpoints, strong ecosystem, and easy testing.
- **Data access: Entity Framework Core**
  - Used to model relationships (including many-to-many) and keep persistence logic consistent.
  - Configured to use SQL Server by default via `DefaultConnection`.
- **Service layer + interfaces**
  - Controllers delegate to `IUserService`, `IGroupService`, `IPermissionService`, `IDashboardService` to keep controllers thin and testable.
- **API key protection (simple auth)**
  - A lightweight middleware (`ApiKeyMiddleware`) checks the `Authorization` header:
    - `Authorization: Bearer Assessment2026key`
- **Centralized exception handling**
  - `ExceptionMiddleware` catches unhandled exceptions and returns a JSON error response.
- **Frontend: React + TypeScript + Vite**
  - Vite for fast dev server/build.
  - Axios instance (`Frontend/src/service/api.ts`) uses `.env` for base URL and token.
  - Tailwind CSS for styling using Shadcn UI components.
- **CORS explicitly configured**
  - Backend allows the configured frontend origin (`Frontend:Origin` in `appsettings.json`).

------------------------------------------------------------------------------


### Prerequisites

- **.NET SDK 8**
- **Node.js** (recommended: latest LTS)
- **SQL Server / LocalDB** (the default config uses `(localdb)\\MSSQLLocalDB`)

### Packages

#### Backend (`UserManagement.API`)

```
FluentAssertions 8.8.0
Microsoft.AspNetCore.Mvc.Testing 8.0.23
Microsoft.EntityFrameworkCore.Design 8.0.23
Microsoft.EntityFrameworkCore.InMemory 8.0.23
Microsoft.EntityFrameworkCore.Sqlite 8.0.23
Microsoft.EntityFrameworkCore.SqlServer 8.0.23
Microsoft.EntityFrameworkCore.Tools 8.0.23
Moq 4.20.72
```

#### Backend tests (`UserManagment.Test`)

```
coverlet.collector 6.0.0
Microsoft.EntityFrameworkCore 8.0.23
Microsoft.NET.Test.Sdk 17.8.0
Moq 4.20.72
xunit 2.5.3
xunit.runner.visualstudio 2.5.3
```

#### Frontend (`Frontend`)

```
react ^19.2.0
react-dom ^19.2.0
react-router-dom ^7.13.0
axios ^1.13.4
vite ^7.2.4
tailwindcss ^4.1.18
```


## How to run locally


### Database migrations

Entity Framework Core migrations are used to keep the database schema in sync with the code in `Models/` and the `AppDbContext`.

An initial migration already exists in `Backend/UserManagement.API/Migrations` (e.g. `20260126202452_InitialCreate`).

Before running migrations:

- Make sure your connection string is correct in `Backend/UserManagement.API/appsettings.json` (`ConnectionStrings:DefaultConnection`).
- If the `dotnet ef` command is not available, install the EF CLI tool:

```bash
dotnet tool install --global dotnet-ef
```

#### Apply migrations / create the database (CLI)

Run this from the repository root:

```bash
dotnet ef database update --project .\Backend\UserManagement.API\UserManagement.API.csproj --startup-project .\Backend\UserManagement.API\UserManagement.API.csproj
```

#### Create a new migration (CLI)

Run this from the repository root:

```bash
dotnet ef migrations add <MigrationName> --project .\Backend\UserManagement.API\UserManagement.API.csproj --startup-project .\Backend\UserManagement.API\UserManagement.API.csproj
```

#### Visual Studio (Backend)

Running the API:

- Set **UserManagement.API** as the **Startup Project**.
- Select the **https** (or **http**) run profile.
- Press **F5** (or **Ctrl+F5**) to start the API.

Running migrations in Visual Studio (Package Manager Console):

- Open **Tools** -> **NuGet Package Manager** -> **Package Manager Console**.
- Set **Default project** to **UserManagement.API**.
- Run:

```powershell
Update-Database
```

To create a migration:

```powershell
Add-Migration <MigrationName>
```

### 1) Run the backend (API)

From the repository root:

```bash
dotnet restore .\Backend\UserManagment.Test\UserManagment.Test.sln
dotnet run --project .\Backend\UserManagement.API\UserManagement.API.csproj
```

Default URLs (from `launchSettings.json`):

- `http://localhost:5211`
- `https://localhost:7240`

All endpoints require the API key header:

```http
Authorization: Bearer Assessment2026key
```

### 2) Run the frontend

The frontend reads configuration from `Frontend/.env`:

- `VITE_API_BASE_URL=http://localhost:5211/api`
- `VITE_API_TOKEN=Assessment2026key`

Run:

```bash
cd Frontend
npm install
npm run dev
```

The Vite dev server will run on:

- `http://localhost:5173`

## How to test

### Backend tests (xUnit)

```bash
dotnet test .\Backend\UserManagment.Test\UserManagment.Test.csproj
```

### Frontend checks

```bash
cd Frontend
npm run lint
npm run build
```

## API overview (high level)

- `GET /api/users`
- `GET /api/groups`
- `GET /api/permissions`
- `GET /api/dashboard`


## NB! postman files are located in the Backend/Postman Collection for testing

