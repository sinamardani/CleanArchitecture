# Unit Tests

این پوشه شامل پروژه‌های Unit Test برای لایه‌های مختلف پروژه است که به صورت **Feature-Based** سازماندهی شده‌اند.

## ساختار پروژه‌ها

### Domain.UnitTests
پروژه تست برای لایه Domain که شامل:
- تست Entity ها
- تست Value Objects
- تست Domain Events
- تست Domain Logic

**پکیج‌های استفاده شده:**
- xUnit
- FluentAssertions

**ساختار Feature-Based:**
```
Domain.UnitTests/
├── TodoItems/
│   ├── TodoItemTests.cs
│   └── Events/          # تست‌های Domain Events مربوط به TodoItem
├── TodoLists/
│   ├── TodoListTests.cs
│   └── ValueObjects/    # تست‌های Value Objects مربوط به TodoList
└── [FeatureName]/       # برای Feature های جدید
```

### Application.UnitTests
پروژه تست برای لایه Application که شامل:
- تست Command Handlers
- تست Query Handlers
- تست Validators
- تست Event Handlers

**پکیج‌های استفاده شده:**
- xUnit
- FluentAssertions
- Moq (برای Mock کردن وابستگی‌ها)

**ساختار Feature-Based:**
```
Application.UnitTests/
├── TodoItems/
│   ├── Commands/         # تست‌های Command Handlers مربوط به TodoItem
│   ├── Queries/          # تست‌های Query Handlers مربوط به TodoItem
│   ├── Validators/       # تست‌های Validators مربوط به TodoItem
│   └── EventHandlers/    # تست‌های Domain Event Handlers مربوط به TodoItem
├── TodoLists/
│   ├── Commands/         # تست‌های Command Handlers مربوط به TodoList
│   ├── Queries/          # تست‌های Query Handlers مربوط به TodoList
│   └── Validators/       # تست‌های Validators مربوط به TodoList
└── [FeatureName]/        # برای Feature های جدید
```

### Infrastructure.UnitTests
پروژه تست برای لایه Infrastructure که شامل:
- تست Infrastructure Services
- تست External Service Integrations

**پکیج‌های استفاده شده:**
- xUnit
- FluentAssertions
- Moq

**ساختار:**
```
Infrastructure.UnitTests/
├── Services/
│   └── LogServiceTests.cs
└── [ServiceName]/        # برای سرویس‌های جدید
```

### Persistence.UnitTests
پروژه تست برای لایه Persistence که شامل:
- تست DbContext
- تست Entity Configurations
- تست Interceptors
- تست Repository Pattern (در صورت استفاده)

**پکیج‌های استفاده شده:**
- xUnit
- FluentAssertions
- Moq
- Microsoft.EntityFrameworkCore.InMemory (برای تست‌های In-Memory)

**ساختار:**
```
Persistence.UnitTests/
├── Data/
│   ├── ApplicationDbContextTests.cs
│   ├── Configurations/  # تست‌های Entity Configurations
│   └── Interceptors/    # تست‌های EF Core Interceptors
└── [FeatureName]/       # برای Feature های جدید
```

## نحوه اجرای تست‌ها

### اجرای همه تست‌ها
```bash
dotnet test CleanArchitecture/CleanArchitecture.sln
```

### اجرای تست‌های Domain
```bash
dotnet test CleanArchitecture/tests/Domain.UnitTests/Domain.UnitTests.csproj
```

### اجرای تست‌های Application
```bash
dotnet test CleanArchitecture/tests/Application.UnitTests/Application.UnitTests.csproj
```

### اجرای تست‌های Infrastructure
```bash
dotnet test CleanArchitecture/tests/Infrastructure.UnitTests/Infrastructure.UnitTests.csproj
```

### اجرای تست‌های Persistence
```bash
dotnet test CleanArchitecture/tests/Persistence.UnitTests/Persistence.UnitTests.csproj
```

### اجرای با جزئیات بیشتر
```bash
dotnet test CleanArchitecture/tests/Application.UnitTests/Application.UnitTests.csproj --verbosity normal
```

## نوشتن تست جدید

### مثال تست Domain (Feature-Based)
```csharp
using FluentAssertions;

namespace Domain.UnitTests.TodoItems;

public class TodoItemTests
{
    [Fact]
    public void SetDone_ToTrue_ShouldRaiseTodoItemCompletedEvent()
    {
        var todoItem = new TodoItem
        {
            ListId = 1,
            Title = "Test Item",
            Done = false
        };

        todoItem.Done = true;

        todoItem.Done.Should().BeTrue();
        todoItem.DomainEvents.Should().ContainSingle();
    }
}
```

### مثال تست Application با Mock (Feature-Based)
```csharp
using Moq;
using FluentAssertions;

namespace Application.UnitTests.TodoItems.Commands;

public class CreateTodoItemCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldCreateTodoItem_AndReturnSuccessResult()
    {
        var mockContext = new Mock<IApplicationDbContext>();
        var handler = new CreateTodoItemCommandHandler(mockContext.Object);
        
        var command = new CreateTodoItemCommand
        {
            ListId = 1,
            Title = "Test Item"
        };
        
        var result = await handler.Handle(command, CancellationToken.None);
        
        result.Should().NotBeNull();
        result.Status.Should().Be(CrudStatus.Succeeded);
    }
}
```

### مثال تست Validator (Feature-Based)
```csharp
using FluentValidation.TestHelper;

namespace Application.UnitTests.TodoItems.Validators;

public class CreateTodoItemCommandValidatorTests
{
    private readonly CreateTodoItemCommandValidator _validator = new();

    [Fact]
    public void Validate_WithValidCommand_ShouldPass()
    {
        var command = new CreateTodoItemCommand
        {
            ListId = 1,
            Title = "Valid Title"
        };

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }
}
```

## Best Practices

1. هر تست باید یک چیز را تست کند
2. از نام‌گذاری واضح استفاده کنید: `MethodName_Scenario_ExpectedBehavior`
3. از AAA Pattern استفاده کنید: Arrange, Act, Assert
4. از FluentAssertions برای خوانایی بیشتر استفاده کنید
5. از Moq برای Mock کردن وابستگی‌ها استفاده کنید
6. تست‌ها را در پوشه Feature مربوطه قرار دهید

## ساختار Feature-Based

برای هر Feature جدید (مثل Products, Orders و غیره):
1. در `Domain.UnitTests/[FeatureName]/` تست‌های Domain را بنویسید
2. در `Application.UnitTests/[FeatureName]/Commands/` تست‌های Commands را بنویسید
3. در `Application.UnitTests/[FeatureName]/Queries/` تست‌های Queries را بنویسید
4. در `Application.UnitTests/[FeatureName]/Validators/` تست‌های Validators را بنویسید

این ساختار باعث می‌شود تست‌های مربوط به هر Feature در کنار هم باشند و نگهداری و توسعه راحت‌تر شود.

