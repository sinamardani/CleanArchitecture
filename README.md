# Clean Architecture Template for .NET 9.0

A comprehensive, production-ready Clean Architecture template for building scalable and maintainable .NET applications. This template follows Clean Architecture principles with CQRS pattern, Domain-Driven Design (DDD), and modern .NET best practices.

## 🏗️ Architecture

This project implements Clean Architecture with clear separation of concerns across multiple layers:

- **Domain**: Core business logic, entities, value objects, and domain events (no dependencies on other layers)
- **Application**: Application use cases, commands, queries, and business rules (depends on Domain and Shared)
- **Shared**: Shared concerns including behaviors, interfaces, and models (no dependencies on other layers)
- **Infrastructure**: External services integration (Redis, caching, etc.) (depends on Application, Domain, and Shared)
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
- ✅ **JWT Authentication** - ECDSA-based JWT tokens (ES256 algorithm) with cookie-based storage
- ✅ **Refresh Token Support** - Secure refresh token mechanism with configurable expiration
- ✅ **Cookie Service** - Secure HTTP-only cookie management for tokens
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
- ✅ **Endpoint Groups** - Organized API endpoints using endpoint groups with automatic discovery
- ✅ **Database Initialization** - Automatic database seeding and migration support
- ✅ **Custom Logging Service** - Structured logging to SQL Server with Serilog, including HTTP context, IP address, user agent, and more
- ✅ **Unit Testing** - Comprehensive unit test infrastructure with xUnit, FluentAssertions, and Moq, organized in Feature-Based structure

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

### Testing
- **xUnit 2.9.2** - Unit testing framework
- **FluentAssertions 7.0.0** - Fluent assertion library for readable test assertions
- **Moq 4.20.72** - Mocking framework for unit tests
- **Microsoft.EntityFrameworkCore.InMemory 9.0.11** - In-memory database provider for testing
- **coverlet.collector 6.0.2** - Code coverage collection

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
├── Application/                      # Application layer (depends on Domain and Shared)
│   ├── Common/                       # Shared application concerns
│   │   ├── Interfaces/              # Application interfaces
│   │   │   ├── Data/                # Data access interfaces
│   │   │   │   └── IApplicationDbContext.cs
│   │   │   └── Messaging/           # CQRS messaging interfaces
│   │   │       ├── Command/         # Command interfaces
│   │   │       │   ├── ICommand.cs
│   │   │       │   └── ICommandHandler.cs
│   │   │       └── Query/           # Query interfaces
│   │   │           ├── IQuery.cs
│   │   │           └── IQueryHandler.cs
│   │   └── Mappings/                # Mapster configurations
│   │       └── MappingExtensions.cs
│   ├── Features/                     # Feature-based organization
│   │   ├── TodoLists/               # TodoList use cases
│   │   │   ├── Commands/            # CQRS commands
│   │   │   │   ├── CreateTodoList/
│   │   │   │   │   ├── CreateTodoList.cs
│   │   │   │   │   └── CreateTodoListCommandValidator.cs
│   │   │   │   ├── UpdateTodoList/
│   │   │   │   │   ├── UpdateTodoList.cs
│   │   │   │   │   └── UpdateTodoListCommandValidator.cs
│   │   │   │   ├── DeleteTodoList/
│   │   │   │   │   └── DeleteTodoList.cs
│   │   │   │   └── PurgeTodoLists/
│   │   │   │       └── PurgeTodoLists.cs
│   │   │   └── Queries/             # CQRS queries
│   │   │       └── GetTodos/
│   │   │           ├── GetTodos.cs
│   │   │           ├── TodosVm.cs
│   │   │           ├── TodoListDto.cs
│   │   │           ├── TodoItemDto.cs
│   │   │           └── LookupDto.cs
│   │   └── TodoItems/               # TodoItem use cases
│   │       ├── Commands/            # CQRS commands
│   │       │   ├── CreateTodoItem/
│   │       │   │   ├── CreateTodoItem.cs
│   │       │   │   └── CreateTodoItemCommandValidator.cs
│   │       │   ├── UpdateTodoItem/
│   │       │   │   ├── UpdateTodoItem.cs
│   │       │   │   └── UpdateTodoItemCommandValidator.cs
│   │       │   ├── UpdateTodoItemDetail/
│   │       │   │   └── UpdateTodoItemDetail.cs
│   │       │   └── DeleteTodoItem/
│   │       │       └── DeleteTodoItem.cs
│   │       ├── Queries/             # CQRS queries
│   │       │   └── GetTodoItemsWithPagination/
│   │       │       ├── GetTodoItemsWithPagination.cs
│   │       │       ├── GetTodoItemsWithPaginationQueryValidator.cs
│   │       │       └── TodoItemBriefDto.cs
│   │       └── EventHandlers/       # Domain event handlers
│   │           ├── TodoItemCreatedEventHandler.cs
│   │           └── TodoItemCompletedEventHandler.cs
│   └── DependencyInjection.cs       # Application DI configuration
│
├── Shared/                           # Shared layer (no dependencies)
│   ├── Behaviors/                    # MediatR pipeline behaviors
│   │   ├── ValidationBehavior.cs    # Automatic validation
│   │   └── PerformanceBehavior.cs   # Performance monitoring
│   ├── Interfaces/                   # Shared interfaces
│   │   ├── Authentication/          # Authentication interfaces
│   │   │   ├── IJwtService.cs       # JWT service interface
│   │   │   └── ICookieService.cs    # Cookie service interface
│   │   ├── ICurrentUserService.cs   # Current user service interface
│   │   └── ILogService.cs           # Logging service interface
│   └── Models/                       # Shared models
│       ├── AppSettings/             # Application settings models
│       │   └── JwtSettings.cs
│       ├── CustomResult/            # Result pattern implementation
│       │   ├── BaseResult.cs
│       │   ├── CrudResult.cs
│       │   └── CrudMessage.cs
│       └── PaginatedList.cs         # Pagination model
│
├── Infrastructure/                   # Infrastructure layer
│   ├── Services/                     # Infrastructure services
│   │   ├── LogService.cs           # Custom logging service with Serilog
│   │   └── Authentication/         # Authentication services
│   │       └── JwtService.cs       # JWT token generation and validation
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
│   │   └── WebApplicationExtensions.cs # Automatic endpoint discovery
│   ├── Services/                    # Web services
│   │   ├── CurrentUserService.cs  # Current user service
│   │   └── CookieService.cs       # Cookie management for tokens
│   ├── Keys/                        # JWT key storage
│   │   ├── private-key.pem        # ECDSA private key (not in repo)
│   │   ├── public-key.pem         # ECDSA public key (not in repo)
│   │   └── README.md              # Key generation instructions
│   ├── Extensions/                  # Extension methods
│   │   ├── ApplicationInitializerExtensions.cs
│   │   └── ExceptionHandlerExtensions.cs # Global exception handler
│   └── DependencyInjection.cs      # Web DI configuration
│
├── ServiceDefaults/                  # Shared service defaults
│   └── Extensions.cs                # Service configuration extensions
│
├── AppHost/                          # .NET Aspire orchestration
│   ├── Program.cs                   # Aspire host configuration
│   └── appsettings.json            # Aspire configuration
│
└── tests/                            # Unit test projects (Feature-Based structure)
    ├── Domain.UnitTests/            # Domain layer unit tests
    │   ├── TodoItems/               # TodoItem domain tests
    │   │   └── TodoItemTests.cs
    │   └── TodoLists/               # TodoList domain tests
    │       ├── TodoListTests.cs
    │       └── ValueObjects/        # Value object tests
    │           └── ColourTests.cs
    ├── Application.UnitTests/        # Application layer unit tests
    │   ├── TodoItems/               # TodoItem application tests
    │   │   ├── Commands/            # Command handler tests
    │   │   ├── Queries/             # Query handler tests
    │   │   └── Validators/          # Validator tests
    │   └── TodoLists/               # TodoList application tests
    │       ├── Commands/            # Command handler tests
    │       ├── Queries/             # Query handler tests
    │       └── Validators/          # Validator tests
    ├── Infrastructure.UnitTests/     # Infrastructure layer unit tests
    │   └── Services/                # Service tests
    │       └── LogServiceTests.cs
    ├── Persistence.UnitTests/        # Persistence layer unit tests
    │   └── Data/                    # DbContext tests
    │       ├── ApplicationDbContextTests.cs
    │       ├── Configurations/      # Configuration tests
    │       └── Interceptors/        # Interceptor tests
    └── README.md                     # Unit tests documentation
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

4. **Generate JWT Keys**

   Generate ECDSA keys for JWT authentication (see `Web/Keys/README.md` for details):
   ```bash
   cd CleanArchitecture/Web/Keys
   openssl ecparam -genkey -name prime256v1 -noout -out private-key.pem
   openssl ec -in private-key.pem -pubout -out public-key.pem
   ```

   **Important:** Never commit these keys to the repository. The `Keys/` directory is already gitignored.

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

### JWT Authentication Configuration

The application uses ECDSA (ES256) algorithm for JWT token signing. Configuration is in `appsettings.json`:

```json
{
  "JwtSettings": {
    "PrivateKeyPath": "Keys/private-key.pem",
    "PublicKeyPath": "Keys/public-key.pem",
    "Issuer": "CleanArchitecture",
    "Audience": "CleanArchitectureUsers",
    "ExpirationMinutes": 60
  },
  "CookieSettings": {
    "Secure": true,
    "RefreshTokenExpirationDays": 7
  }
}
```

**JWT Key Setup:**

1. Generate ECDSA keys using OpenSSL:
```bash
# Generate private key
openssl ecparam -genkey -name prime256v1 -noout -out private-key.pem

# Generate public key from private key
openssl ec -in private-key.pem -pubout -out public-key.pem
```

2. Place the keys in `Web/Keys/` directory (this directory is gitignored for security)

3. The keys are automatically loaded at startup

**Token Storage:**

- Access tokens are stored in HTTP-only cookies (`access_token`)
- Refresh tokens are stored in HTTP-only cookies (`refresh_token`)
- Cookies are configured with `Secure`, `HttpOnly`, and `SameSite=Strict` flags
- Token can also be sent via Authorization header: `Bearer {token}`

For detailed key generation instructions, see `Web/Keys/README.md`.

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

## 🔐 Authentication & Authorization

### JWT Authentication Flow

The application implements JWT-based authentication using ECDSA (ES256) algorithm:

1. **Login**: User authenticates with credentials
   - Credentials are validated against ASP.NET Core Identity
   - JWT access token is generated using `IJwtService`
   - Refresh token is generated (if implemented)
   - Tokens are stored in HTTP-only cookies via `ICookieService`

2. **Token Validation**: 
   - Tokens can be sent via:
     - HTTP-only cookie (`access_token`)
     - Authorization header (`Bearer {token}`)
   - JWT Bearer middleware validates tokens on each request
   - Public key is loaded from `Keys/public-key.pem`

3. **Refresh Token**:
   - Refresh tokens are stored in HTTP-only cookies (`refresh_token`)
   - Configurable expiration (default: 7 days)
   - Used to obtain new access tokens without re-authentication

4. **Logout**:
   - Removes both access and refresh token cookies
   - Invalidates session

### Authorization Commands

The project includes authorization command structure (endpoints can be implemented as needed):

- **CreateUser**: Register new users
- **UpdateUser**: Update user information
- **DeleteUser**: Delete user accounts
- **Login**: Authenticate users and generate tokens
- **Logout**: Sign out users
- **RefreshToken**: Refresh access tokens

### Security Features

- **HTTP-only Cookies**: Prevents XSS attacks
- **Secure Flag**: Cookies only sent over HTTPS (configurable)
- **SameSite=Strict**: CSRF protection
- **ECDSA Keys**: Asymmetric encryption for token signing
- **Token Expiration**: Configurable token lifetime
- **Clock Skew**: Set to zero for strict time validation

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

Pipeline behaviors are located in the `Shared/Behaviors` project:

- **ValidationBehavior**: Automatically validates commands/queries using FluentValidation before execution
- **PerformanceBehavior**: Logs performance metrics for requests taking longer than 500ms

These behaviors are registered in `Application/DependencyInjection.cs` and apply to all MediatR requests.

### Result Pattern

The application uses a custom result pattern (`CrudResult<T>`) for consistent API responses. The result pattern is implemented in the `Shared/Models/CustomResult` namespace:

```csharp
public class CrudResult<T> : BaseResult
{
    public T? Data { get; set; }
    public CrudStatus Status { get; set; }
    public List<CrudMessage> Messages { get; set; }
}
```

All result models are located in the `Shared` project to ensure consistency across all layers.

### Endpoint Groups

Endpoints are organized using endpoint groups that inherit from `EndpointGroupBase`. Endpoints are automatically discovered and registered:

```csharp
public class TodoLists : EndpointGroupBase
{
    public override void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.RequireAuthorization().MapPost(CreateTodoList, nameof(CreateTodoList));
        // ...
    }
}
```

**Automatic Discovery:**
- All classes inheriting from `EndpointGroupBase` are automatically discovered
- Endpoints are mapped to `/api/{GroupName}` route
- Authorization can be applied at the group level using `RequireAuthorization()`

**Current Endpoint Groups:**
- `/api/TodoLists` - Todo list management endpoints
- `/api/TodoItems` - Todo item management endpoints

## 🧪 Example Domain

The template includes a complete Todo application as an example:

- **TodoLists**: Manage todo lists with colors (value object)
- **TodoItems**: Manage individual todo items within lists with priorities and reminders
- **Authentication**: JWT-based authentication with ECDSA keys and cookie management

This demonstrates:
- Aggregate roots (`TodoList`)
- Value objects (`Colour`)
- Domain events (`TodoItemCreatedEvent`, `TodoItemCompletedEvent`, `TodoItemDeletedEvent`)
- CQRS commands and queries
- API endpoints with endpoint groups
- Pagination support
- Soft delete functionality
- JWT authentication with ECDSA keys
- Cookie-based token storage
- Refresh token mechanism
- **Comprehensive unit testing** with Feature-Based organization (51 tests covering all layers)

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

The `ICurrentUserService` interface (located in `Shared/Interfaces/`) provides access to the current user context:
- `UserId`: Current user ID
- Used by `AuditTableEntityInterceptor` to set audit fields

Implementation is in `Web/Services/CurrentUserService.cs`.

### JWT Service

The `IJwtService` interface (located in `Shared/Interfaces/Authentication/`) provides JWT token operations:
- `GenerateToken(int userId)`: Generates a JWT access token using ECDSA (ES256)
- `ValidateToken(string token)`: Validates and parses a JWT token

Implementation is in `Infrastructure/Services/Authentication/JwtService.cs`.

### Cookie Service

The `ICookieService` interface (located in `Shared/Interfaces/Authentication/`) provides secure cookie management for tokens:
- `SetTokenCookie(string token)`: Sets access token in HTTP-only cookie
- `GetTokenFromCookie()`: Retrieves access token from cookie
- `RemoveTokenCookie()`: Removes access token cookie
- `SetRefreshTokenCookie(string refreshToken)`: Sets refresh token in HTTP-only cookie
- `GetRefreshTokenFromCookie()`: Retrieves refresh token from cookie
- `RemoveRefreshTokenCookie()`: Removes refresh token cookie

All cookies are configured with:
- `HttpOnly`: Prevents JavaScript access
- `Secure`: Only sent over HTTPS (configurable)
- `SameSite=Strict`: CSRF protection

Implementation is in `Web/Services/CookieService.cs`.

### Logging Service

The `ILogService` interface (located in `Shared/Interfaces/`) provides structured logging capabilities:
- `DbLog(string message, LogLevel level)`: Writes logs to SQL Server database with automatic context enrichment
- `ConsoleLog(string message, LogLevel level)`: Writes logs to console

The service automatically captures:
- HTTP request information (path, method, IP address, user agent)
- Source class/method name
- Machine name and environment
- User ID (when available)

Logs are stored in the `Logs` table in the `LoggingDb` database. The table is automatically created on first use.

Implementation is in `Infrastructure/Services/LogService.cs`.

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
- **Generate and securely store JWT keys** (use Azure Key Vault, AWS Secrets Manager, or environment variables)
- Set up Redis for production caching
- Configure OpenTelemetry endpoints for observability
- Set up proper logging (Application Insights, Seq, etc.)
- Configure HTTPS certificates
- Set up database backups
- Configure CORS policies
- Set up rate limiting
- Configure authentication and authorization policies
- Set up monitoring and alerting
- Ensure `CookieSettings:Secure` is set to `true` in production
- Rotate JWT keys periodically
- Implement proper refresh token storage (consider database storage for production)

## 🧪 Development

### Adding a New Feature

1. **Create Domain Entity** in `Domain/{Feature}/`
2. **Create Domain Events** (if needed) in `Domain/{Feature}/Events/`
3. **Create Feature Folder** in `Application/Features/{Feature}/`
4. **Create Commands** in `Application/Features/{Feature}/Commands/{CommandName}/`
   - Each command should be in its own folder with handler and validator
5. **Create Queries** in `Application/Features/{Feature}/Queries/{QueryName}/`
   - Each query should be in its own folder with handler, validator, and DTOs
6. **Create Event Handlers** (if needed) in `Application/Features/{Feature}/EventHandlers/`
7. **Create Validators** for commands/queries using FluentValidation (in same folder as command/query)
8. **Create Mappings** in `Application/Common/Mappings/` (if needed)
9. **Create EF Core Configuration** in `Persistence/Data/Configurations/`
10. **Create Endpoints** in `Web/Endpoints/{Feature}.cs` (inherit from `EndpointGroupBase`)
11. **Endpoints are automatically discovered** - no manual registration needed
12. **Create Unit Tests** following the Feature-Based structure in `tests/`

**Example Endpoint Group:**

```csharp
public class MyFeature : EndpointGroupBase
{
    public override void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.RequireAuthorization().MapPost(Create, nameof(Create));
        groupBuilder.RequireAuthorization().MapGet(Get, nameof(Get));
    }

    public async Task<CrudResult<int>> Create(ISender sender, CreateCommand command)
    {
        return await sender.Send(command);
    }

    public async Task<CrudResult<MyDto>> Get(ISender sender, [AsParameters] GetQuery query)
    {
        return await sender.Send(query);
    }
}
```

### Running Migrations

```bash
cd CleanArchitecture/Web
dotnet ef migrations add MigrationName --project ../Persistence
dotnet ef database update --project ../Persistence
```

### Unit Testing

The project includes comprehensive unit test infrastructure organized in a **Feature-Based** structure. Tests are separated by layer:

#### Test Projects

- **Domain.UnitTests** - Tests for domain entities, value objects, and domain logic
- **Application.UnitTests** - Tests for command handlers, query handlers, validators, and event handlers
- **Infrastructure.UnitTests** - Tests for infrastructure services
- **Persistence.UnitTests** - Tests for DbContext, configurations, and interceptors

#### Running Tests

```bash
# Run all tests
dotnet test CleanArchitecture/CleanArchitecture.sln

# Run specific test project
dotnet test CleanArchitecture/tests/Domain.UnitTests/Domain.UnitTests.csproj
dotnet test CleanArchitecture/tests/Application.UnitTests/Application.UnitTests.csproj
dotnet test CleanArchitecture/tests/Infrastructure.UnitTests/Infrastructure.UnitTests.csproj
dotnet test CleanArchitecture/tests/Persistence.UnitTests/Persistence.UnitTests.csproj

# Run with detailed output
dotnet test CleanArchitecture/CleanArchitecture.sln --verbosity normal
```

#### Test Statistics

The project currently includes:
- **Domain Tests**: 16 tests covering entities, value objects, and domain events
- **Application Tests**: 26 tests covering commands, queries, and validators
- **Infrastructure Tests**: 4 tests covering infrastructure services
- **Persistence Tests**: 5 tests covering DbContext operations

**Total: 51 unit tests** with comprehensive coverage of core functionality.

#### Writing Tests

Tests follow a Feature-Based organization where each feature has its own folder structure:

```
tests/
├── Domain.UnitTests/
│   └── {FeatureName}/
│       ├── {Entity}Tests.cs
│       └── ValueObjects/
│           └── {ValueObject}Tests.cs
└── Application.UnitTests/
    └── {FeatureName}/
        ├── Commands/
        ├── Queries/
        └── Validators/
```

For detailed testing guidelines and examples, see `tests/README.md`.

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
