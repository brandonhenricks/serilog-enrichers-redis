# Copilot Project Instructions

## What would you like Copilot to know about your project?

This project is a C# Serilog enricher library that enhances log events for StackExchange.Redis exceptions. It extracts key diagnostic values from Redis timeout messages and enriches logs with structured properties. It includes:

- A parser (`RedisTimeoutParser`) that maps abbreviated diagnostic fields (e.g., `qu`, `aw`, `rs`) to long-form names (e.g., `queue`, `active-writer`, `read-state`) per Redis timeout documentation.
- A Serilog enricher (`RedisExceptionEnricher`) that conditionally logs structured Redis metadata (command type, failure type, connection source, and parsed diagnostics) for all `RedisException` types.
- Unit tests for both components using xUnit.
- The project follows clean architecture, SOLID principles, and aims for structured observability.

## How would you like Copilot to respond?

- Prefer minimal, well-structured code changes that preserve the existing architecture.
- When extending the enricher, automatically include both abbreviation and mapped long-form fields in logs.
- Always use `ILogEventEnricher`, `ILogEventPropertyFactory`, and `AddPropertyIfAbsent` for property injection.
- When updating the parser, always check and extend the abbreviation-to-long-name dictionary.
- For new Redis diagnostic keys, first check if the key is documented. If it is, map it in `AbbreviationToLongName`; otherwise, log only the abbreviated form.
- Use xUnit for tests. Prefer `Fact` over `Theory` unless testing multiple values.
- When adding new diagnostic abbreviations or handling new Redis exception types, always update or add corresponding unit tests to ensure coverage.
- When logging properties, prefer using structured property names that are consistent and descriptive (e.g., `Redis.QueueLength` instead of just `queue`).
- If a Redis diagnostic abbreviation is ambiguous or undocumented, add a comment in the code and in the mapping dictionary for future maintainers.
- When refactoring, ensure that public APIs remain backward compatible unless a major version bump is intended.
- For error handling in the parser, prefer returning partial results with a warning property rather than throwing exceptions, to maximize observability.
- Document any new abbreviations or exception types handled in the project’s README or internal documentation for clarity.
- When extending the enricher, ensure that performance is not significantly impacted, especially in high-throughput logging scenarios.

---

## Example Tasks Copilot Should Be Able to Help With

- Add support for a new diagnostic abbreviation (e.g., `rz`) found in future Redis updates.
- Extend the enricher to handle a new `RedisMovedException` by enriching the target endpoint.
- Add a test case for malformed Redis messages with unexpected delimiters.
- Refactor shared logic in `RedisExceptionEnricher` into reusable methods.