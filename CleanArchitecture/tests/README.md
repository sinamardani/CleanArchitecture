# Test Structure

This directory contains all tests for the Clean Architecture solution, organized by test type and layer.

## Test Structure Overview

```
tests/
├── Unit/                          # Fast, isolated unit tests (no external dependencies)
│   ├── Domain.UnitTests/         # Pure domain logic tests (no mocks)
│   ├── Application.UnitTests/    # Application use cases with mocked dependencies
│   └── Infrastructure.UnitTests/ # Infrastructure services with mocked dependencies
│
├── Integration/                   # Integration tests (real dependencies, slower)
│   ├── Application.IntegrationTests/  # Application layer with real database
│   ├── Persistence.IntegrationTests/  # EF Core with real database
│   └── Infrastructure.IntegrationTests/ # Real external services (Redis, etc.)
│
├── EndToEnd/                      # End-to-end tests (full application stack)
│   └── Web.IntegrationTests/     # API tests using WebApplicationFactory
│
└── Shared/                        # Shared test utilities and helpers
    └── TestUtilities/             # TestBase classes, Fixtures, Helpers
```

## Test Types Explained

### Unit Tests (`Unit/`)
**Purpose:** Test individual components in isolation with mocked dependencies.

**Characteristics:**
- Fast execution (< 1ms per test typically)
- No external dependencies (database, file system, network)
- Use mocks/stubs for dependencies
- Test business logic, validation, and transformations
- Should run in parallel

**When to use:**
- Domain entities and value objects
- Application command/query handlers (with mocked IApplicationDbContext)
- Infrastructure services (with mocked external dependencies)
- Validators and business rules

### Integration Tests (`Integration/`)
**Purpose:** Test components with real dependencies (database, external services).

**Characteristics:**
- Slower execution (10-100ms per test)
- Use real database (InMemory or TestContainers)
- Test data persistence and retrieval
- Test EF Core configurations and interceptors
- May require test isolation (database cleanup)

**When to use:**
- Application handlers with real database
- EF Core DbContext operations
- Entity configurations
- Database interceptors
- Infrastructure services with real external APIs

### End-to-End Tests (`EndToEnd/`)
**Purpose:** Test the complete application flow through the API.

**Characteristics:**
- Slowest execution (100ms-1s per test)
- Use WebApplicationFactory to bootstrap the application
- Test HTTP requests/responses
- Test authentication/authorization
- Test complete user workflows

**When to use:**
- API endpoint testing
- Authentication flows
- Complete business workflows
- Integration between all layers

## Running Tests

### Run all tests
```bash
dotnet test
```

### Run only unit tests (fast)
```bash
dotnet test --filter "FullyQualifiedName~Unit"
```

### Run only integration tests
```bash
dotnet test --filter "FullyQualifiedName~Integration"
```

### Run tests for a specific layer
```bash
dotnet test tests/Unit/Domain.UnitTests/
dotnet test tests/Unit/Application.UnitTests/
dotnet test tests/Integration/Application.IntegrationTests/
```

### Run with coverage
```bash
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
```

## Best Practices

1. **Unit Tests:**
   - Keep tests fast (< 1ms)
   - Use mocks for all dependencies
   - Test one thing per test
   - Use descriptive test names: `MethodName_Scenario_ExpectedBehavior`

2. **Integration Tests:**
   - Use unique database names per test to avoid conflicts
   - Clean up test data after each test
   - Consider using TestContainers for real database testing
   - Test realistic scenarios, not edge cases

3. **End-to-End Tests:**
   - Test complete user workflows
   - Use realistic test data
   - Test error scenarios and edge cases
   - Keep tests independent and isolated

4. **Test Organization:**
   - Follow feature-based structure within each test project
   - Mirror the production code structure
   - Group related tests in the same file
   - Use test fixtures for shared setup

## Test Utilities

Shared test utilities are located in `Shared/TestUtilities/`:
- `TestBase` - Base classes for common test setup
- `DatabaseFixture` - Database setup and cleanup
- `WebApplicationFactoryFixture` - Web application factory setup
- `TestDataBuilder` - Fluent builders for test data
