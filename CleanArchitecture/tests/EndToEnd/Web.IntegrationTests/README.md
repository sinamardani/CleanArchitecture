# Web Integration Tests (End-to-End)

These tests verify the complete application flow through the API using `WebApplicationFactory`.

## Setup Requirements

The E2E tests require proper authentication configuration. The Web project uses JWT authentication with ECDSA keys.

### Option 1: Provide Test Keys

Create test JWT keys in the `Web/Keys` directory:
- `private-key.pem`
- `public-key.pem`

### Option 2: Modify Authentication for Tests

The `CustomWebApplicationFactory` attempts to override authentication with a test scheme, but the Web project's `AddWebServices` method configures JWT authentication before the test override can take effect.

To make E2E tests work without real keys, you can:
1. Make authentication setup conditional based on environment
2. Provide test configuration that skips JWT validation
3. Use a separate test configuration file

## Current Status

The E2E tests are set up but may fail due to authentication requirements. This is expected for a template project where proper authentication setup is required.

## Running E2E Tests

```bash
dotnet test tests/EndToEnd/Web.IntegrationTests/
```

