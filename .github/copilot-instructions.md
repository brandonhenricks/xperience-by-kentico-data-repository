# Copilot Instructions

- Use latest syntax compatible with **.NET 8**.
- Adhere to [Microsoft's coding conventions for C#](https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions).
- Include XML documentation comments for all public classes, methods, and properties.
- Use **PascalCase** for type and method names, **camelCase** for local variables and parameters.
- Optimize code for readability, maintainability, and performance.
- Azure Functions should target the latest Azure Function tooling, and target .NET 8.
- Follow **SOLID** principles and clean architecture patterns.
- Prefer async and await over synchronous code. [Async Guidance](https://github.com/davidfowl/AspNetCoreDiagnosticScenarios/blob/master/AsyncGuidance.md)
- Prefer var over explicit types.
- Employ dependency injection using the built-in DI container.
- Handle exceptions gracefully and use `Microsoft.Extensions.Logging` for logging.
- This is a Xperience By Kentico library, and as such should leverage built in api as much as possible.

## Testing Guidelnes

- Use the AAA pattern (Arrange, Act, Assert)
- Avoid infrastructure dependencies.
- Name tests clearly.
- Write minimally passing tests.
- Avoid magic strings.
- Avoid logic in tests.
- Prefer helper methods for setup and teardown.
- Avoid multiple acts in a single test.
- Write unit tests using **NUnit** and aim for high code coverage.