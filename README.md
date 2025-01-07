# XperienceCommunity.DataRepository

XperienceCommunity.DataRepository is a library designed to simplify data access and management within applications built on Xperience by Kentico. This library provides a repository pattern implementation, enabling developers to perform CRUD (Create, Read, Update, Delete) operations in a more structured and maintainable way.

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

This project has **Full support by 7-day bug-fix policy**.

See [`SUPPORT.md`](https://github.com/Kentico/.github/blob/main/SUPPORT.md#full-support) for more information.

For any security issues see [`SECURITY.md`](https://github.com/Kentico/.github/blob/main/SECURITY.md).
---Select the correct badge for the support policy and update the GitHub Action pipeline badge to point to this repository (replace `repo-template`)---

[![7-day bug-fix policy](https://img.shields.io/badge/-7--days_bug--fixing_policy-grey?labelColor=orange&logo=data:image/svg+xml;base64,PHN2ZyBjbGFzcz0ic3ZnLWljb24iIHN0eWxlPSJ3aWR0aDogMWVtOyBoZWlnaHQ6IDFlbTt2ZXJ0aWNhbC1hbGlnbjogbWlkZGxlO2ZpbGw6IGN1cnJlbnRDb2xvcjtvdmVyZmxvdzogaGlkZGVuOyIgdmlld0JveD0iMCAwIDEwMjQgMTAyNCIgdmVyc2lvbj0iMS4xIiB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciPjxwYXRoIGQ9Ik04ODguNDkgMjIyLjY4NnYtMzEuNTRsLTY1LjY3Mi0wLjk1NWgtMC4yMDVhNDY1LjcxNSA0NjUuNzE1IDAgMCAxLTE0NC4zMTUtMzEuMzM0Yy03Ny4wMDUtMzEuMTk4LTEyNi4yOTQtNjYuNzY1LTEyNi43MDMtNjcuMTA3bC0zOS44LTI4LjY3Mi0zOS4xODUgMjguNDY4Yy0yLjA0OCAxLjUwMS00OS45MDMgMzYuMDQ0LTEyNi45MDggNjcuMzFhNDQ3LjQyIDQ0Ny40MiAwIDAgMS0xNDQuNTIgMzEuMzM1bC02NS44NzcgMC45NTZ2Mzc4Ljg4YzAgODcuMDQgNDkuODM0IDE4NC42NjEgMTM3LjAxIDI2Ny44MSAzNy41NDcgMzUuODQgNzkuMjU4IDY2LjM1NSAxMjAuODMzIDg4LjIgNDMuMjggMjIuNzMzIDg0LjI0IDM0LjYxMiAxMTguODUyIDM0LjYxMiAzNC40MDYgMCA3NS43NzYtMTIuMTUyIDExOS42MDMtMzUuMTU4YTU0Ny45NzcgNTQ3Ljk3NyAwIDAgMCAxMjAuMDEzLTg3LjY1NCA1MTUuMjA5IDUxNS4yMDkgMCAwIDAgOTYuMTg4LTEyMi44OGMyNy4xMDItNDkuNTYyIDQwLjgyMy05OC4zMDQgNDAuODIzLTE0NC45OTlsLTAuMTM2LTM0Ny4yMDR6TTUxMC4wOSAxNDMuNDI4bDEuNzA2LTEuMzY1IDEuNzc1IDEuMzY1YzUuODAzIDQuMTY1IDU5LjUyOSA0MS44NDggMTQwLjM1NiA3NC43NTIgNzkuMTkgMzIuMDg2IDE1My42IDM1LjYzNSAxNjcuNjYzIDM2LjA0NWwyLjU5NCAwLjA2OCAwLjIwNSAzMTUuNzM0YzAuMTM3IDY5LjQ5NS00Mi41OTggMTUwLjE4Ni0xMTcuMDc3IDIyMS40NTdDNjQxLjU3IDg1NC4yODkgNTYzLjEzIDg5Ni40NzggNTEyIDg5Ni40NzhjLTIzLjY4OSAwLTU1LjU3LTkuODk5LTg5LjcwMi0yNy43ODVhNDc4LjgyMiA0NzguODIyIDAgMCAxLTEwNS42MDktNzcuMjc4QzI0Mi4yMSA3MjAuMjEzIDE5OS40NzUgNjM5LjUyMiAxOTkuNDc1IDU2OS44OVYyNTQuMjI1bDIuNzMtMC4xMzZjMy4yNzggMCA4Mi42MDQtMS41MDIgMTY3LjY2NC0zNS45NzdhNzM5Ljk0MiA3MzkuOTQyIDAgMCAwIDE0MC4yMi03NC42MTV2LTAuMDY5eiIgIC8+PHBhdGggZD0iTTcxMy4zMTggMzY4LjY0YTMyLjIyMiAzMi4yMjIgMCAwIDAtNDUuMzI5IDBMNDQ5LjE5NSA1ODcuNDM1bC05My4xODQtOTMuMTE2YTMyLjIyMiAzMi4yMjIgMCAwIDAtNDUuMzMgMCAzMi4yMjIgMzIuMjIyIDAgMCAwIDAgNDUuMjZsMTE1Ljg1IDExNS44NWEzMi4yOSAzMi4yOSAwIDAgMCA0NS4zMjggMEw3MTMuMzIgNDEzLjlhMzIuMjIyIDMyLjIyMiAwIDAgMCAwLTQ1LjMzeiIgIC8+PC9zdmc+)](https://github.com/Kentico/.github/blob/main/SUPPORT.md#full-support) [![Kentico Labs](https://img.shields.io/badge/Kentico_Labs-grey?labelColor=orange&logo=data:image/svg+xml;base64,PHN2ZyBjbGFzcz0ic3ZnLWljb24iIHN0eWxlPSJ3aWR0aDogMWVtOyBoZWlnaHQ6IDFlbTt2ZXJ0aWNhbC1hbGlnbjogbWlkZGxlO2ZpbGw6IGN1cnJlbnRDb2xvcjtvdmVyZmxvdzogaGlkZGVuOyIgdmlld0JveD0iMCAwIDEwMjQgMTAyNCIgdmVyc2lvbj0iMS4xIiB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciPjxwYXRoIGQ9Ik05NTYuMjg4IDgwNC40OEw2NDAgMjc3LjQ0VjY0aDMyYzE3LjYgMCAzMi0xNC40IDMyLTMycy0xNC40LTMyLTMyLTMyaC0zMjBjLTE3LjYgMC0zMiAxNC40LTMyIDMyczE0LjQgMzIgMzIgMzJIMzg0djIxMy40NEw2Ny43MTIgODA0LjQ4Qy00LjczNiA5MjUuMTg0IDUxLjIgMTAyNCAxOTIgMTAyNGg2NDBjMTQwLjggMCAxOTYuNzM2LTk4Ljc1MiAxMjQuMjg4LTIxOS41MnpNMjQxLjAyNCA2NDBMNDQ4IDI5NS4wNFY2NGgxMjh2MjMxLjA0TDc4Mi45NzYgNjQwSDI0MS4wMjR6IiAgLz48L3N2Zz4=)](https://github.com/Kentico/.github/blob/main/SUPPORT.md#labs-limited-support) [![CI: Build and Test](https://github.com/Kentico/repo-template/actions/workflows/ci.yml/badge.svg)](https://github.com/Kentico/repo-template/actions/workflows/ci.yml)

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

---This matrix explains which versions of the library are compatible with different versions of Xperience by Kentico / Kentico Xperience 13---

| Xperience Version | Library Version |
| ----------------- | --------------- |
| >= 29.6.0         | 1.0.0           |

### Dependencies

---These are all the dependencies required to use (not build) the library---

- [ASP.NET Core 8.0](https://dotnet.microsoft.com/en-us/download)
- [Xperience by Kentico](https://docs.kentico.com)
