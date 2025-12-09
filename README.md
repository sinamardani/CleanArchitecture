# Clean Architecture Template for .NET 9.0

A comprehensive, production-ready Clean Architecture template for building scalable and maintainable .NET applications. This template follows Clean Architecture principles with CQRS pattern, Domain-Driven Design (DDD), and modern .NET best practices.

## 🏗️ Architecture

This project implements Clean Architecture with clear separation of concerns across multiple layers:

- **Domain**: Core business logic, entities, value objects, and domain events
- **Application**: Application use cases, commands, queries, and business rules
- **Infrastructure**: External services integration (Redis, caching, etc.)
- **Persistence**: Data access layer with Entity Framework Core
- **Web**: API endpoints, controllers, and presentation logic
- **AppHost**: .NET Aspire orchestration for local development

## ✨ Features

- ✅ **Clean Architecture** - Separation of concerns with dependency inversion
- ✅ **CQRS Pattern** - Command Query Responsibility Segregation using MediatR
- ✅ **Domain-Driven Design** - Rich domain models with value objects and domain events
- ✅ **Minimal APIs** - Modern ASP.NET Core Minimal APIs with endpoint groups
- ✅ **Entity Framework Core 9.0** - Code-first approach with migrations
- ✅ **ASP.NET Core Identity** - Built-in authentication and authorization
- ✅ **FluentValidation** - Fluent validation for commands and queries
- ✅ **Mapster** - High-performance object mapping
- ✅ **Redis Caching** - Distributed caching support
- ✅ **OpenTelemetry** - Observability and monitoring
- ✅ **Health Checks** - Application health monitoring
- ✅ **Swagger/OpenAPI** - API documentation with NSwag
- ✅ **Domain Events** - Event-driven architecture support
- ✅ **Audit Interceptors** - Automatic audit trail for entities
- ✅ **Soft Delete** - Global query filter for soft-deleted entities
- ✅ **.NET Aspire** - Cloud-ready orchestration and tooling
- ✅ **Exception Handling** - Global exception handling middleware
- ✅ **Performance Monitoring** - Built-in performance behavior tracking

## 🛠️ Technologies

### Core Framework
- **.NET 9.0** - Latest .NET framework
- **ASP.NET Core 9.0** - Web framework
- **Entity Framework Core 9.0** - ORM

### Key Libraries
- **MediatR 12.4.1** - CQRS implementation
- **FluentValidation 11.9.0** - Validation framework
- **Mapster 7.4.0** - Object mapping
- **NSwag 14.6.3** - OpenAPI/Swagger generation
- **Ardalis.GuardClauses 5.0.0** - Guard clauses for defensive programming

### Infrastructure
- **SQL Server** - Primary database
- **Redis** - Caching and session storage
- **OpenTelemetry** - Observability
- **.NET Aspire** - Cloud-native orchestration

## 📁 Project Structure

```
CleanArchitecture/
├── Domain/                    # Core domain layer
│   ├── Commons/              # Base entities, value objects, events
│   ├── Identity/             # Identity domain entities
│   ├── TodoLists/            # TodoList aggregate
│   └── TodoItems/            # TodoItem aggregate
│
├── Application/               # Application layer
│   ├── Commons/              # Shared application concerns
│   │   ├── Behaviours/      # MediatR pipeline behaviors
│   │   ├── Interfaces/     # Application interfaces
│   │   ├── Models/          # DTOs and result models
│   │   └── Mappings/        # Mapster configurations
│   ├── TodoLists/            # TodoList use cases
│   │   ├── Commands/        # CQRS commands
│   │   └── Queries/         # CQRS queries
│   └── TodoItems/            # TodoItem use cases
│
├── Infrastructure/            # Infrastructure layer
│   └── DependencyInjection.cs
│
├── Persistence/               # Data access layer
│   ├── Data/                 # DbContext and configurations
│   │   ├── Configurations/  # EF Core configurations
│   │   ├── Interceptors/    # EF Core interceptors
│   │   └── SeedDatas/        # Database seed data
│   └── Migrations/           # EF Core migrations
│
├── Web/                       # Presentation layer
│   ├── Endpoints/            # Minimal API endpoints
│   ├── Infrastructure/       # Web infrastructure
│   ├── Services/             # Web services
│   └── Extensions/           # Extension methods
│
├── ServiceDefaults/           # Shared service defaults
│   └── Extensions.cs         # Service configuration extensions
│
└── AppHost/                   # .NET Aspire orchestration
    └── Program.cs            # Aspire host configuration
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
       "Redis": "localhost:6379"
     }
   }
   ```

4. **Run database migrations**
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
- Start SQL Server container
- Start Redis container
- Start the Web API
- Open the Aspire dashboard

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

## 📚 API Documentation

Once the application is running, you can access:

- **Swagger UI**: Navigate to `/swagger` for interactive API documentation
- **ReDoc**: Navigate to `/document` for alternative API documentation
- **OpenAPI JSON**: Available at `/swagger/v1/swagger.json`

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

### Redis Configuration

Redis is optional but recommended for caching:

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
```

## 🏛️ Architecture Patterns

### CQRS (Command Query Responsibility Segregation)

Commands and Queries are separated:
- **Commands**: Located in `Application/{Feature}/Commands/`
- **Queries**: Located in `Application/{Feature}/Queries/`

### Domain Events

Domain events are raised from domain entities and handled by event handlers:

```csharp
// Domain event
public class TodoItemCreatedEvent : BaseEvent
{
    public TodoItem Item { get; }
}

// Event handler
public class TodoItemCreatedEventHandler : INotificationHandler<TodoItemCreatedEvent>
{
    // Handle event
}
```

### MediatR Pipeline Behaviors

- **ValidationBehaviour**: Automatically validates commands/queries using FluentValidation
- **PerformanceBehaviour**: Logs performance metrics for requests

## 🧪 Example Domain

The template includes a complete Todo application as an example:

- **TodoLists**: Manage todo lists with colors
- **TodoItems**: Manage individual todo items within lists

This demonstrates:
- Aggregate roots
- Value objects
- Domain events
- CQRS commands and queries
- API endpoints

## 📦 Key Components

### Base Entities

- `BaseEntity`: Base class for all entities
- `BaseAuditTableEntity`: Includes audit fields (CreatedBy, CreatedDate, etc.)
- `BaseValueObject`: Base class for value objects

### Result Pattern

The application uses a custom result pattern for operations:

```csharp
public class CrudResult<T> : BaseResult
{
    public T? Data { get; set; }
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
}
```

### Interceptors

- **AuditTableEntityInterceptor**: Automatically sets audit fields
- **DispatchDomainEventsInterceptor**: Publishes domain events after save

## 🔍 Health Checks

The application includes comprehensive health checks:

- `/health` - Overall health status
- `/health/ready` - Readiness probe (database, Redis)
- `/health/live` - Liveness probe

## 🚢 Deployment

### Docker Support

The project includes .NET Aspire which provides Docker support out of the box. For custom Docker deployment:

1. Create a `Dockerfile` in the `Web` project
2. Build and run the container

### Production Considerations

- Configure proper connection strings
- Set up Redis for production
- Configure OpenTelemetry endpoints
- Set up proper logging
- Configure HTTPS certificates
- Set up database backups

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

## 📞 Support

For questions and support, please open an issue in the GitHub repository.

---

**Built with ❤️ using .NET 9.0 and Clean Architecture principles**
