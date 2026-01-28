# Assesment
A RESTful API to manage Users, Groups, and Permissions. Supports full CRUD operations, many-to-many relationships between entities, and endpoints to retrieve user counts and group details. Designed for testing, development, and integration with a simple frontend



📦 Solution Structure
UserManagement.API/
│
├── Controllers/        # API endpoints (HTTP layer)
├── DTOs/               # Request/Response models
├── Models/             # Domain entities (EF Core models)
├── Data/               # DbContext and migrations
├── Services/           # Business logic
├── Repositories/       # Data access logic
├── Middleware/         # Custom middleware (logging, error handling)
├── Program.cs          # App startup & DI configuration
└── appsettings.json    # Environment configuration


Overview:
The solution follows a layered architecture to separate concerns between API, business logic, and data access. This improves maintainability, testability, and scalability.

🧠 Key Technical Decisions
1. Clean Architecture & Separation of Concerns

Controllers handle only HTTP concerns.

Services contain business logic.

Repositories handle database access using EF Core.
➡️ This makes the codebase easier to maintain and test.

2. Use of DTOs

DTOs are used to prevent over-posting and control exactly what data is exposed or accepted by the API.

3. Entity Framework Core with Code-First Migrations

EF Core is used for ORM.

Migrations are used to version and evolve the database schema.

4. Centralized Error Handling & Logging

Custom middleware is used to catch unhandled exceptions.

Logs are written for debugging and production monitoring.

5. Environment Configuration

Sensitive values are stored using environment variables or appsettings.*.json.

Supports Development and Production environments cleanly.
