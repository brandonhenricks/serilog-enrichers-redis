# Serilog.Enrichers.Redis

Serilog enricher for StackExchange.Redis exceptions. Adds structured diagnostic properties from Redis timeout and connection errors to help troubleshoot and monitor Redis behavior in distributed .NET applications.

[![Build Status](https://github.com/brandonhenricks/serilog-enrichers-redis/actions/workflows/ci.yml/badge.svg)](https://github.com/brandonhenricks/serilog-enrichers-redis/actions)
[![NuGet](https://img.shields.io/nuget/v/Serilog.Enrichers.Redis.svg)](https://www.nuget.org/packages/Serilog.Enrichers.Redis/)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)

---

## Features

- Enriches log events with Redis exception details
- Captures Redis connection and command information
- Seamless integration with Serilog

## Installation

Install via NuGet:

```sh
dotnet add package Serilog.Enrichers.Redis
```

Or using the Package Manager:

```sh
Install-Package Serilog.Enrichers.Redis
```

---

## Usage

Add the Redis enricher to your Serilog configuration:

```csharp
using Serilog;
using Serilog.Enrichers.Redis;

Log.Logger = new LoggerConfiguration()
    .Enrich.WithRedisExceptionEnricher()
    .WriteTo.Console()
    .CreateLogger();
```

Now, any Redis-related exceptions will include additional context in your logs.

### Using appsettings.json

You can also configure Serilog and the Redis enricher via `appsettings.json`:

**appsettings.json**

```json
{
  "Serilog": {
    "Using": [ "Serilog.Enrichers.Redis" ],
    "Enrich": [ "WithRedisExceptionEnricher" ],
    "WriteTo": [
      { "Name": "Console" }
    ]
  }
}
```

**Program.cs**

```csharp
using Serilog;

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration) // assumes 'configuration' is your IConfiguration instance
    .CreateLogger();
```

Make sure to add the necessary Serilog and enricher packages, and call `ReadFrom.Configuration` with your loaded configuration.

---

## Contributing

Contributions are welcome! Please open issues or submit pull requests.  
If you have feature requests or questions, open a discussion or issue on GitHub.

**To contribute:**

- Fork this repository
- Create a feature branch
- Open a pull request describing your changes

---

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
