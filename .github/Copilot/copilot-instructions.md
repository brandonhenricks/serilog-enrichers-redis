# Copilot Instructions

- Use latest syntax compatible with **.NET 8**.
- Adhere to [Microsoft's coding conventions for C#](https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions).
- Include XML documentation comments for all public classes, methods, and properties.
- Optimize code for readability, maintainability, and performance.
- Follow **SOLID** principles and clean architecture patterns.
- Employ dependency injection using the built-in DI container.
- All Serilog enrichers should properly implement `ILogEventEnricher` and be registered for dependency injection.
- Ensure logs are enriched with relevant contextual data.
- Avoid introducing unnecessary dependencies; keep the enricher lightweight.
- Code should be compatible with ASP.NET Core applications using StackExchange.Redis and Serilog.
- Document any required Serilog, or Redis configuration in XML comments or README updates.

## Testing Guidelines

- Use the AAA pattern (Arrange, Act, Assert).
- Avoid infrastructure dependencies; mock Redis/Serilog dependencies as needed.
- Name tests clearly and descriptively.
- Write minimally passing, focused tests—one assertion per test when possible.
- Avoid magic strings and magic numbers in tests.
- Avoid logic in tests; keep assertions simple.
- Prefer helper methods or [SetUp]/[TearDown] for repetitive test setup/cleanup.
- Avoid multiple act steps in a single test.
- Write unit tests using **NUnit** and aim for high code coverage.
- For extension methods or integration points, include tests that verify correct enrichment of `LogEvent` with expected Xperience contextual information.