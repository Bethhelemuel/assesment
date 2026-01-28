# Assessment

A full-stack **User Management** application:

- **Backend:** ASP.NET Core Web API (.NET 8) + Entity Framework Core
- **Frontend:** React + TypeScript + Vite

The API manages **Users**, **Groups**, and **Permissions** (CRUD + many-to-many relationships) and exposes a small **Dashboard** endpoint for aggregated counts.

## Solution structure


.
├─ Backend/
│  ├─ UserManagement.API/               # ASP.NET Core Web API
│  │  ├─ Controllers/                   # HTTP endpoints (Users, Groups, Permissions, Dashboard)
│  │  ├─ Data/                          # EF Core DbContext
│  │  ├─ Models/                        # Domain entities
│  │  ├─ DTOs/                          # API contracts
│  │  ├─ Services/                      # Business logic
│  │  ├─ Interfaces/                    # Service abstractions
│  │  └─ Middleware/                    # Exception + API key middleware
│  └─ UserManagment.Test/               # xUnit tests
└─ Frontend/                            # React + Vite client
   ├─ src/
   │  ├─ pages/                         # Route pages
   │  ├─ components/                    # UI components
   │  └─ service/                       # Axios client + API calls
   └─ .env                              # Frontend API configuration


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
- **CORS explicitly configured**
  - Backend allows the configured frontend origin (`Frontend:Origin` in `appsettings.json`).

------------------------------------------------------------------------------
## How to run locally

### Prerequisites

- **.NET SDK 8**
- **Node.js** (recommended: latest LTS)
- **SQL Server / LocalDB** (the default config uses `(localdb)\\MSSQLLocalDB`)

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


## NB postman files are located in the Backend/Postman Collection for testing

