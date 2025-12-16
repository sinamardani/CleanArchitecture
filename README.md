# Clean Architecture Template for .NET 9.0

A comprehensive, production-ready Clean Architecture template for building scalable and maintainable .NET applications. This template follows Clean Architecture principles with CQRS pattern, Domain-Driven Design (DDD), and modern .NET best practices.

## 🏗️ Architecture

This project implements Clean Architecture with clear separation of concerns across multiple layers:

- **Domain**: Core business logic, entities, value objects, and domain events (no dependencies on other layers)
- **Application**: Application use cases, commands, queries, and business rules (depends only on Domain)
- **Infrastructure**: External services integration (Redis, caching, etc.) (depends on Application and Domain)
- **Persistence**: Data access layer with Entity Framework Core (depends on Application and Domain)
- **Web**: API endpoints, controllers, and presentation logic (depends on all layers)
- **AppHost**: .NET Aspire orchestration for local development
- **ServiceDefaults**: Shared service configuration and OpenTelemetry setup

## ✨ Features

- ✅ **Clean Architecture** - Separation of concerns with dependency inversion
- ✅ **CQRS Pattern** - Command Query Responsibility Segregation using MediatR
- ✅ **Domain-Driven Design** - Rich domain models with value objects and domain events
- ✅ **Minimal APIs** - Modern ASP.NET Core Minimal APIs with endpoint groups
- ✅ **Entity Framework Core 9.0** - Code-first approach with migrations
- ✅ **ASP.NET Core Identity** - Built-in authentication and authorization with custom ApplicationUser
- ✅ **FluentValidation** - Fluent validation for commands and queries with automatic validation pipeline
- ✅ **Mapster** - High-performance object mapping with dependency injection support
- ✅ **Redis Caching** - Distributed caching support (optional)
- ✅ **OpenTelemetry** - Comprehensive observability and monitoring with OTLP export
- ✅ **Health Checks** - Application health monitoring with readiness and liveness probes
- ✅ **Swagger/OpenAPI** - API documentation with NSwag (Swagger UI and ReDoc)
- ✅ **Domain Events** - Event-driven architecture with automatic event dispatching
- ✅ **Audit Interceptors** - Automatic audit trail for entities (CreatedBy, CreatedOn, ModifiedBy, ModifiedOn, DeletedBy, DeletedOn)
- ✅ **Soft Delete** - Global query filter for soft-deleted entities
- ✅ **.NET Aspire** - Cloud-ready orchestration and tooling with containerized services
- ✅ **Exception Handling** - Global exception handling middleware with ProblemDetails
- ✅ **Performance Monitoring** - Built-in performance behavior tracking (logs requests > 500ms)
- ✅ **Result Pattern** - Consistent API response pattern with CrudResult
- ✅ **Endpoint Groups** - Organized API endpoints using endpoint groups
- ✅ **Database Initialization** - Automatic database seeding and migration support
- ✅ **Custom Logging Service** - Structured logging to SQL Server with Serilog, including HTTP context, IP address, user agent, and more

## 🛠️ Technologies

### Core Framework
- **.NET 9.0** - Latest .NET framework
- **ASP.NET Core 9.0** - Web framework
- **Entity Framework Core 9.0** - ORM

### Key Libraries
- **MediatR 12.4.1** - CQRS implementation and domain event dispatching
- **FluentValidation 11.9.0** - Validation framework with dependency injection extensions
- **Mapster 7.4.0** - High-performance object mapping
- **Mapster.DependencyInjection 1.0.1** - Mapster DI support
- **Mapster.EFCore 5.1.1** - EF Core integration for Mapster
- **NSwag.AspNetCore 14.6.3** - OpenAPI/Swagger generation
- **Ardalis.GuardClauses 5.0.0** - Guard clauses for defensive programming
- **MediatR.Contracts 2.0.1** - MediatR contracts

### Infrastructure
- **SQL Server** - Primary database (via .NET Aspire or standalone)
- **Redis** - Caching and session storage (optional, via .NET Aspire or standalone)
- **OpenTelemetry** - Observability with OTLP export
- **.NET Aspire 13.0.2** - Cloud-native orchestration
- **AspNetCore.HealthChecks.Redis 9.0.0** - Redis health checks
- **Microsoft.Extensions.Caching.StackExchangeRedis 9.0.0** - Redis caching
- **Serilog 4.3.0** - Structured logging framework
- **Serilog.Sinks.Console 6.1.1** - Console logging sink
- **Serilog.Sinks.MSSqlServer 9.0.2** - SQL Server logging sink
- **Microsoft.AspNetCore.Http.Abstractions** - HTTP context access

### ServiceDefaults
- **OpenTelemetry.Exporter.OpenTelemetryProtocol 1.9.0** - OTLP exporter
- **OpenTelemetry.Extensions.Hosting 1.9.0** - Hosting extensions
- **OpenTelemetry.Instrumentation.AspNetCore 1.9.0** - ASP.NET Core instrumentation
- **OpenTelemetry.Instrumentation.EntityFrameworkCore 1.14.0-beta.2** - EF Core instrumentation
- **OpenTelemetry.Instrumentation.Http 1.9.0** - HTTP client instrumentation
- **OpenTelemetry.Instrumentation.Runtime 1.9.0** - Runtime metrics
- **OpenTelemetry.Instrumentation.StackExchangeRedis 1.14.0-beta.1** - Redis instrumentation
- **Microsoft.Extensions.Http.Resilience 8.10.0** - HTTP resilience patterns
- **Microsoft.Extensions.ServiceDiscovery 8.2.2** - Service discovery

## 📁 Project Structure

```
CleanArchitecture/
├── Domain/                           # Core domain layer (no dependencies)
│   ├── Commons/                      # Base entities, value objects, events, interfaces
│   │   ├── BaseEntity.cs            # Base entity with domain events support
│   │   ├── BaseAuditTableEntity.cs  # Base entity with audit fields
│   │   ├── BaseValueObject.cs       # Base value object
│   │   ├── BaseEvent.cs             # Base domain event
│   │   ├── Constants/               # Domain constants
│   │   ├── Enums/                   # Domain enums
│   │   ├── Exceptions/              # Domain exceptions
│   │   └── Interfaces/              # Domain interfaces
│   ├── Identity/                     # Identity domain entities
│   │   └── ApplicationUser.cs       # Custom identity user
│   ├── TodoLists/                   # TodoList aggregate
│   │   ├── TodoList.cs             # Aggregate root
│   │   └── ValueObjects/           # Value objects
│   └── TodoItems/                   # TodoItem aggregate
│       ├── TodoItem.cs             # Entity with domain events
│       └── Events/                 # Domain events
│           ├── TodoItemCreatedEvent.cs
│           ├── TodoItemCompletedEvent.cs
│           └── TodoItemDeletedEvent.cs
│
├── Application/                      # Application layer (depends on Domain)
│   ├── Commons/                      # Shared application concerns
│   │   ├── Behaviours/              # MediatR pipeline behaviors
│   │   │   ├── ValidationBehaviour.cs      # Automatic validation
│   │   │   └── PerformanceBehaviour.cs     # Performance monitoring
│   │   ├── Interfaces/              # Application interfaces
│   │   │   └── Data/               # Data access interfaces
│   │   ├── Models/                  # DTOs and result models
│   │   │   └── CustomResult/       # Result pattern implementation
│   │   └── Mappings/                # Mapster configurations
│   ├── TodoLists/                   # TodoList use cases
│   │   ├── Commands/               # CQRS commands
│   │   │   ├── CreateTodoList/
│   │   │   ├── UpdateTodoList/
│   │   │   ├── DeleteTodoList/
│   │   │   └── PurgeTodoLists/
│   │   └── Queries/                # CQRS queries
│   │       └── GetTodos/
│   └── TodoItems/                   # TodoItem use cases
│       ├── Commands/               # CQRS commands
│       │   ├── CreateTodoItem/
│       │   ├── UpdateTodoItem/
│       │   ├── UpdateTodoItemDetail/
│       │   └── DeleteTodoItem/
│       ├── Queries/                # CQRS queries
│       │   └── GetTodoItemsWithPagination/
│       └── EventHandlers/          # Domain event handlers
│           └── TodoItemCreatedEventHandler.cs
│
├── Infrastructure/                   # Infrastructure layer
│   ├── Services/                     # Infrastructure services
│   │   └── LogService.cs           # Custom logging service with Serilog
│   └── DependencyInjection.cs      # Infrastructure DI configuration
│
├── Persistence/                      # Data access layer
│   ├── Data/                        # DbContext and configurations
│   │   ├── ApplicationDbContext.cs # Main DbContext
│   │   ├── ApplicationDbContextInitializer.cs # Database initialization
│   │   ├── Configurations/         # EF Core entity configurations
│   │   ├── Interceptors/           # EF Core interceptors
│   │   │   ├── AuditTableEntityInterceptor.cs    # Audit field interceptor
│   │   │   └── DispatchDomainEventsInterceptor.cs # Domain events dispatcher
│   │   ├── Extensions/             # EF Core extensions
│   │   └── SeedDatas/              # Database seed data
│   ├── Migrations/                  # EF Core migrations
│   └── DependencyInjection.cs      # Persistence DI configuration
│
├── Web/                              # Presentation layer
│   ├── Endpoints/                   # Minimal API endpoints
│   │   ├── TodoLists.cs           # TodoList endpoints
│   │   └── TodoItems.cs           # TodoItem endpoints
│   ├── Infrastructure/              # Web infrastructure
│   │   ├── EndpointGroupBase.cs    # Base class for endpoint groups
│   │   ├── IEndpointRouteBuilderExtensions.cs # Endpoint extensions
│   │   ├── MethodInfoExtensions.cs
│   │   └── WebApplicationExtensions.cs
│   ├── Services/                    # Web services
│   │   └── CurrentUserService.cs  # Current user service
│   ├── Extensions/                  # Extension methods
│   │   ├── ApplicationInitializerExtensions.cs
│   │   └── ExceptionHandlerExtensions.cs # Global exception handler
│   └── DependencyInjection.cs      # Web DI configuration
│
├── ServiceDefaults/                  # Shared service defaults
│   └── Extensions.cs                # Service configuration extensions
│
└── AppHost/                          # .NET Aspire orchestration
    ├── Program.cs                   # Aspire host configuration
    └── appsettings.json            # Aspire configuration
```

## 🚀 Getting Started

### Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [SQL Server](https://www.microsoft.com/sql-server) or [SQL Server Express](https://www.microsoft.com/sql-server/sql-server-downloads)
- [Redis](https://redis.io/download) (optional, for caching)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/) with C# extension
- [.NET Aspire Workload](https://learn.microsoft.com/dotnet/aspire/fundamentals/setup-tooling) (for AppHost)

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/CleanArchitecture.git
   cd CleanArchitecture
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Configure connection strings**

   Update `appsettings.json` in the `Web` project:
   ```json
   {
     "ConnectionStrings": {
       "CleanArchitectureDb": "Server=localhost;Database=CleanArchitectureDb;User Id=sa;Password=YourPassword;MultipleActiveResultSets=true;TrustServerCertificate=True",
       "LoggingDb": "Server=localhost;Database=LoggingDb;User Id=sa;Password=YourPassword;MultipleActiveResultSets=true;TrustServerCertificate=True",
       "Redis": "localhost:6379"
     }
   }
   ```

   Note: When using .NET Aspire (AppHost), connection strings are automatically configured.

4. **Run database migrations**

   When using AppHost, migrations run automatically. For standalone execution:
   ```bash
   cd CleanArchitecture/Web
   dotnet ef database update --project ../Persistence
   ```

## 🏃 Running the Application

### Option 1: Using .NET Aspire (Recommended for Development)

The AppHost project provides orchestration for all services:

```bash
cd CleanArchitecture/AppHost
dotnet run
```

This will:
- Start SQL Server container (port 63295)
- Start Redis container (port 6379)
- Start the Web API
- Open the Aspire dashboard at `http://localhost:15000`
- Automatically initialize the database

The AppHost manages two databases:
- **CleanArchitectureDb**: Main application database
- **LoggingDb**: Logging database for structured logs (automatically created)

### Option 2: Running Web API Directly

```bash
cd CleanArchitecture/Web
dotnet run
```

The API will be available at:
- **API**: `https://localhost:5001` or `http://localhost:5000`
- **Swagger UI**: `https://localhost:5001/swagger`
- **ReDoc**: `https://localhost:5001/document`
- **Health Check**: `https://localhost:5001/health`
- **Health Check (Ready)**: `https://localhost:5001/health/ready`
- **Health Check (Live)**: `https://localhost:5001/health/live`

## 📚 API Documentation

Once the application is running, you can access:

- **Swagger UI**: Navigate to `/swagger` for interactive API documentation
- **ReDoc**: Navigate to `/document` for alternative API documentation
- **OpenAPI JSON**: Available at `/swagger/v1/swagger.json`

The root URL (`/`) automatically redirects to `/swagger`.

## 🔧 Configuration

### Database Configuration

The application uses Entity Framework Core with SQL Server. Connection strings are configured in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "CleanArchitectureDb": "Server=YOUR_SERVER;Database=CleanArchitectureDb;User Id=YOUR_USER;Password=YOUR_PASSWORD;MultipleActiveResultSets=true;TrustServerCertificate=True"
  }
}
```

When using AppHost, the connection string is automatically injected.

### Redis Configuration

Redis is optional but recommended for caching. If the connection string is not provided, Redis features are disabled:

```json
{
  "ConnectionStrings": {
    "Redis": "localhost:6379"
  }
}
```

### Identity Configuration

Password requirements can be customized in `Persistence/DependencyInjection.cs`:

```csharp
options.Password.RequireDigit = true;
options.Password.RequireLowercase = true;
options.Password.RequireUppercase = true;
options.Password.RequireNonAlphanumeric = false;
options.Password.RequiredLength = 6;
options.User.RequireUniqueEmail = true;
```

### OpenTelemetry Configuration

OpenTelemetry is configured in `ServiceDefaults/Extensions.cs`. The OTLP endpoint can be configured via:

- Environment variable: `DOTNET_DASHBOARD_OTLP_ENDPOINT_URL`
- Configuration: `OTEL_EXPORTER_OTLP_ENDPOINT`

### Logging Configuration

The application includes a custom `LogService` that uses Serilog to write structured logs to SQL Server. The logging service automatically captures:

**Standard Serilog Fields:**
- `Id` - Unique log entry identifier
- `Message` - Log message text
- `MessageTemplate` - Message template
- `Level` - Log level (Verbose, Debug, Information, Warning, Error, Fatal)
- `TimeStamp` - Log timestamp
- `Exception` - Exception details (if applicable)
- `Properties` - Additional properties as JSON

**Custom Fields:**
- `UserId` - Current user ID (if authenticated)
- `RequestPath` - HTTP request path
- `HttpMethod` - HTTP method (GET, POST, etc.)
- `IPAddress` - Client IP address (supports X-Forwarded-For header)
- `UserAgent` - Browser/client user agent
- `Duration` - Request duration in milliseconds
- `Source` - Source class/method name
- `MachineName` - Server/machine name
- `Environment` - Environment name (Development, Production, etc.)

**Configuration:**

The `LogService` is registered as a Singleton in `Infrastructure/DependencyInjection.cs`. The logging table (`Logs`) is automatically created when the first log is written (`AutoCreateSqlTable = true`).

**Usage:**

```csharp
public class MyService
{
    private readonly ILogService _logService;
    
    public MyService(ILogService logService)
    {
        _logService = logService;
    }
    
    public void DoSomething()
    {
        _logService.DbLog("User performed action", LogLevel.Information);
        _logService.ConsoleLog("Debug information", LogLevel.Debug);
    }
}
```

**Connection String:**

Add the `LoggingDb` connection string to `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "LoggingDb": "Server=localhost;Database=LoggingDb;User Id=sa;Password=YourPassword;MultipleActiveResultSets=true;TrustServerCertificate=True"
  }
}
```

If the connection string is not provided or connection fails, the service falls back to console logging.

## 🏛️ Architecture Patterns

### CQRS (Command Query Responsibility Segregation)

Commands and Queries are separated:
- **Commands**: Located in `Application/{Feature}/Commands/` - Handle write operations
- **Queries**: Located in `Application/{Feature}/Queries/` - Handle read operations

All commands and queries use MediatR for processing and return `CrudResult<T>` or `CrudResult` for consistent API responses.

### Domain Events

Domain events are raised from domain entities and automatically dispatched after SaveChanges:

**Available Domain Events:**
- `TodoItemCreatedEvent` - Raised when a TodoItem is created
- `TodoItemCompletedEvent` - Raised when a TodoItem is marked as done
- `TodoItemDeletedEvent` - Raised when a TodoItem is deleted

**Example:**
```csharp
// Domain event
public sealed record TodoItemCreatedEvent(TodoItem Item) : BaseEvent;

// Event handler
public class TodoItemCreatedEventHandler : INotificationHandler<TodoItemCreatedEvent>
{
    public Task Handle(TodoItemCreatedEvent notification, CancellationToken cancellationToken)
    {
        // Handle event
        return Task.CompletedTask;
    }
}
```

Domain events are automatically dispatched by `DispatchDomainEventsInterceptor` after successful SaveChanges.

### MediatR Pipeline Behaviors

- **ValidationBehaviour**: Automatically validates commands/queries using FluentValidation before execution
- **PerformanceBehaviour**: Logs performance metrics for requests taking longer than 500ms

### Result Pattern

The application uses a custom result pattern (`CrudResult<T>`) for consistent API responses:

```csharp
public class CrudResult<T> : BaseResult
{
    public T? Data { get; set; }
    public CrudStatus Status { get; set; }
    public List<CrudMessage> Messages { get; set; }
}
```

### Endpoint Groups

Endpoints are organized using endpoint groups that inherit from `EndpointGroupBase`:

```csharp
public class TodoLists : EndpointGroupBase
{
    public override void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.MapPost(CreateTodoList, nameof(CreateTodoList));
        // ...
    }
}
```

## 🧪 Example Domain

The template includes a complete Todo application as an example:

- **TodoLists**: Manage todo lists with colors (value object)
- **TodoItems**: Manage individual todo items within lists with priorities and reminders

This demonstrates:
- Aggregate roots (`TodoList`)
- Value objects (`Colour`)
- Domain events (`TodoItemCreatedEvent`, `TodoItemCompletedEvent`, `TodoItemDeletedEvent`)
- CQRS commands and queries
- API endpoints with endpoint groups
- Pagination support
- Soft delete functionality

## 📦 Key Components

### Base Entities

- `BaseEntity<TKey>`: Base class for all entities with domain events support
- `BaseEntity`: Base entity with int key
- `BaseAuditTableEntity<TKey>`: Includes audit fields (CreatedBy, CreatedOn, ModifiedBy, ModifiedOn, DeletedBy, DeletedOn)
- `BaseAuditTableEntity`: Audit entity with int key
- `BaseValueObject`: Base class for value objects

### Interceptors

- **AuditTableEntityInterceptor**: Automatically sets audit fields (CreatedBy, CreatedOn, ModifiedBy, ModifiedOn) and handles soft delete (DeletedBy, DeletedOn)
- **DispatchDomainEventsInterceptor**: Publishes domain events after successful SaveChanges

### Soft Delete

Entities inheriting from `BaseAuditTableEntity` support soft delete. When an entity is deleted:
- The `DeletedOn` and `DeletedBy` fields are automatically set
- The entity is not physically deleted from the database
- A global query filter excludes soft-deleted entities from queries

### Current User Service

The `ICurrentUserService` interface provides access to the current user context:
- `UserId`: Current user ID
- Used by `AuditTableEntityInterceptor` to set audit fields

### Logging Service

The `ILogService` interface provides structured logging capabilities:
- `DbLog(string message, LogLevel level)`: Writes logs to SQL Server database with automatic context enrichment
- `ConsoleLog(string message, LogLevel level)`: Writes logs to console

The service automatically captures:
- HTTP request information (path, method, IP address, user agent)
- Source class/method name
- Machine name and environment
- User ID (when available)

Logs are stored in the `Logs` table in the `LoggingDb` database. The table is automatically created on first use.

## 🔍 Health Checks

The application includes comprehensive health checks:

- `/health` - Overall health status (all checks)
- `/health/ready` - Readiness probe (database, Redis) - Used by Kubernetes/Docker
- `/health/live` - Liveness probe - Used by Kubernetes/Docker

Health check responses include:
- Overall status
- Individual check status
- Exception messages (if any)
- Duration for each check

## 🚢 Deployment

### Docker Support

The project includes .NET Aspire which provides Docker support out of the box. The AppHost automatically manages:
- SQL Server container
- Redis container
- Web API container

For custom Docker deployment:

1. Create a `Dockerfile` in the `Web` project
2. Build and run the container
3. Configure connection strings via environment variables

### Production Considerations

- Configure proper connection strings via environment variables or secure configuration
- Set up Redis for production caching
- Configure OpenTelemetry endpoints for observability
- Set up proper logging (Application Insights, Seq, etc.)
- Configure HTTPS certificates
- Set up database backups
- Configure CORS policies
- Set up rate limiting
- Configure authentication and authorization
- Set up monitoring and alerting

## 🧪 Development

### Adding a New Feature

1. **Create Domain Entity** in `Domain/{Feature}/`
2. **Create Domain Events** (if needed) in `Domain/{Feature}/Events/`
3. **Create Commands** in `Application/{Feature}/Commands/`
4. **Create Queries** in `Application/{Feature}/Queries/`
5. **Create Validators** for commands/queries using FluentValidation
6. **Create Mappings** in `Application/Commons/Mappings/` (if needed)
7. **Create EF Core Configuration** in `Persistence/Data/Configurations/`
8. **Create Endpoints** in `Web/Endpoints/{Feature}.cs`
9. **Register Endpoints** in `Web/Infrastructure/WebApplicationExtensions.cs`

### Running Migrations

```bash
cd CleanArchitecture/Web
dotnet ef migrations add MigrationName --project ../Persistence
dotnet ef database update --project ../Persistence
```

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🙏 Acknowledgments

- Based on Clean Architecture principles by Robert C. Martin
- Inspired by various Clean Architecture implementations in .NET community
- Uses modern .NET 9.0 features and best practices
- Built with .NET Aspire for cloud-native development

## 📞 Support

For questions and support, please open an issue in the GitHub repository.

---

**Built with ❤️ using .NET 9.0 and Clean Architecture principles**
