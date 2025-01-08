# XperienceCommunity.DataRepository

XperienceCommunity.DataRepository is a library designed to simplify data access and management within applications built on Xperience by Kentico. This library provides a repository pattern implementation, enabling developers to perform data access operations in a more structured and maintainable way.

## Quick Start

### Step 1: Add the NuGet Package

Add the package to your application using the .NET CLI:
View the [Usage Guide](./docs/Usage-Guide.md) for more detailed instructions.

```powershell
dotnet add package XperienceCommunity.DataRepository
```

Alternatively, you can add the package using the NuGet Package Manager in Visual Studio:

1. Right-click on your project in the Solution Explorer.
2. Select "Manage NuGet Packages..."
3. Search for `XperienceCommunity.DataRepository`.
4. Click "Install" to add the package to your project.

### Step 2: Configure Services

In your `Startup.cs` or `Program.cs` file, configure the services to use the repository pattern. Add the following code to the `ConfigureServices` method:

```csharp
// Register the repository services
services.AddXperienceDataRepositories(options=> { options.CacheMinutes = 60;});
```

## Full Instructions

To install the `XperienceCommunity.DataRepository` library, follow these steps:

### Step 1: Add the NuGet Package

Add the package to your application using the .NET CLI:
View the [Usage Guide](./docs/Usage-Guide.md) for more detailed instructions.

```powershell
dotnet add package XperienceCommunity.DataRepository
```

Alternatively, you can add the package using the NuGet Package Manager in Visual Studio:

1. Right-click on your project in the Solution Explorer.
2. Select "Manage NuGet Packages..."
3. Search for `XperienceCommunity.DataRepository`.
4. Click "Install" to add the package to your project.

### Step 2: Configure Services

In your `Startup.cs` or `Program.cs` file, configure the services to use the repository pattern. Add the following code to the `ConfigureServices` method:

```csharp
// Register the repository services
services.AddXperienceDataRepositories(options=> { options.CacheMinutes = 60;});
```

## Contributing

To see the guidelines for Contributing to Kentico open source software, please see [Kentico's `CONTRIBUTING.md`](https://github.com/Kentico/.github/blob/main/CONTRIBUTING.md) for more information and follow the [Kentico's `CODE_OF_CONDUCT`](https://github.com/Kentico/.github/blob/main/CODE_OF_CONDUCT.md).

Instructions and technical details for contributing to **this** project can be found in [Contributing Setup](./docs/Contributing-Setup.md).

## License

Distributed under the MIT License. See [`LICENSE.md`](./LICENSE.md) for more information.

## Support

This contribution is not directly supported or maintained by Kentico. It serves to illustrate some specific functionality or is a proof of concept for a specific version of the product. Feel free to contribute yourself, file an issue, fix bugs, but Kentico does not guarantee any assistance nor support.

[![Kentico Labs](https://img.shields.io/badge/Kentico_Labs-grey?labelColor=orange&logo=data:image/svg+xml;base64,PHN2ZyBjbGFzcz0ic3ZnLWljb24iIHN0eWxlPSJ3aWR0aDogMWVtOyBoZWlnaHQ6IDFlbTt2ZXJ0aWNhbC1hbGlnbjogbWlkZGxlO2ZpbGw6IGN1cnJlbnRDb2xvcjtvdmVyZmxvdzogaGlkZGVuOyIgdmlld0JveD0iMCAwIDEwMjQgMTAyNCIgdmVyc2lvbj0iMS4xIiB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciPjxwYXRoIGQ9Ik05NTYuMjg4IDgwNC40OEw2NDAgMjc3LjQ0VjY0aDMyYzE3LjYgMCAzMi0xNC40IDMyLTMycy0xNC40LTMyLTMyLTMyaC0zMjBjLTE3LjYgMC0zMiAxNC40LTMyIDMyczE0LjQgMzIgMzIgMzJIMzg0djIxMy40NEw2Ny43MTIgODA0LjQ4Qy00LjczNiA5MjUuMTg0IDUxLjIgMTAyNCAxOTIgMTAyNGg2NDBjMTQwLjggMCAxOTYuNzM2LTk4Ljc1MiAxMjQuMjg4LTIxOS41MnpNMjQxLjAyNCA2NDBMNDQ4IDI5NS4wNFY2NGgxMjh2MjMxLjA0TDc4Mi45NzYgNjQwSDI0MS4wMjR6IiAgLz48L3N2Zz4=)](https://github.com/Kentico/.github/blob/main/SUPPORT.md#labs-limited-support) [![CI: Build and Test](https://github.com/brandonhenricks/xperience-by-kentico-data-repository/actions/workflows/ci.yml/badge.svg)](https://github.com/brandonhenricks/xperience-by-kentico-data-repository/actions/workflows/ci.yml)

## Description

XperienceCommunity.DataRepository is a library designed to simplify data access and management within applications built on Xperience by Kentico. This library provides a repository pattern implementation, enabling developers to perform CRUD (Create, Read, Update, Delete) operations in a more structured and maintainable way.

### Key Features:

- **Repository Pattern**: Provides a consistent way to access data, promoting separation of concerns and testability.
- **Support for .NET 8 and .NET 9**: Leverages the latest features and improvements in the .NET ecosystem.
- **Integration with Xperience by Kentico**: Seamlessly integrates with Xperience by Kentico, allowing for efficient data management within the CMS.
- **Asynchronous Operations**: Supports async/await patterns for non-blocking data operations.
- **Dependency Injection**: Designed to work with the built-in dependency injection container in ASP.NET Core.

### Scenarios Fulfilled:

- Simplified data access for applications using Xperience by Kentico.
- Improved code maintainability and testability through the repository pattern.
- Efficient handling of data operations with support for asynchronous programming.

### Limitations:

- The library is designed specifically for use with Xperience by Kentico and may not be suitable for other CMS or data management systems.
- Certain advanced data operations may require custom implementations beyond the provided repository pattern.

## Requirements

### Library Version Matrix

- This matrix explains which versions of the library are compatible with different versions of Xperience by Kentico / Kentico Xperience 13---

| Xperience Version | Library Version |
| ----------------- | --------------- |
| >= 29.6.0         | 1.0.0           |

### Dependencies

- [ASP.NET Core 8.0](https://dotnet.microsoft.com/en-us/download)
- [Xperience by Kentico](https://docs.kentico.com)
