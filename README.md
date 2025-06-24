# Serilog.Enrichers.Redis

A Serilog enricher that adds Redis-specific context to your log events, providing detailed information about Redis exceptions, commands, and connections to help you diagnose and monitor Redis-related issues in your .NET applications.

[![CI](.github/workflows/ci.yml/badge.svg)](.github/workflows/ci.yml)
[![NuGet](https://img.shields.io/nuget/v/Serilog.Enrichers.Redis.svg)](https://www.nuget.org/packages/Serilog.Enrichers.Redis/)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)

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

Usage
Add the Redis enricher to your Serilog configuration:

```csharp
using Serilog;
using Serilog.Enrichers.Redis;

Log.Logger = new LoggerConfiguration()
    .Enrich.WithRedisEnricher()
    .WriteTo.Console()
    .CreateLogger();
```

Now, any Redis-related exceptions will include additional context in your logs.


## License
This project is licensed under the MIT License. See the LICENSE file for details.