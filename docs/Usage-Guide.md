# Usage Guide

## Introduction

This guide will help you get started with the `XperienceCommunity.DataRepository` library, which simplifies data access and management within applications built on Xperience by Kentico. The library provides a repository pattern implementation, enabling developers to perform CRUD operations in a structured and maintainable way.

## Prerequisites

- .NET 8 or .NET 9
- Xperience by Kentico

## Step-by-Step Guide

### Step 1: Add the NuGet Package

Add the package to your application using the .NET CLI:

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

### Step 3: Create a BaseController

Create a `BaseController` that restricts to `IWebpageFieldSource` and injects `IPageRepository<TEntity>`:

```csharp
using Microsoft.AspNetCore.Mvc; using XperienceCommunity.DataRepository;
public abstract class BaseController<TEntity> : Controller where TEntity : class, IWebpageFieldSource
{
    protected readonly IPageRepository repository;
    protected readonly IWebPageDataContextRetriever PageDataContextRetriever

    protected BaseController(IPageRepository<TEntity> repository, IWebPageDataContextRetriever pageDataContextRetriever)
    {
        this.repository = repository;
        this.PageDataContextRetriever = pageDataContextRetriever;
    }

// Common methods for CRUD operations can be added here
}
```

### Step 4: Create a Specific Controller

```csharp
using Microsoft.AspNetCore.Mvc;
using XperienceCommunity.DataRepository;

public class HomeController : BaseController<HomePage>
{

    public HomeController(IPageTypeRepository<HomePage> repository, IWebPageDataContextRetriever pageDataContextRetriever) : base(repository, pageDataContextRetriever) { }

    public async Task<IActionResult> Index()
    {
        var page = _webPageDataContextRetriever.Retrieve().WebPage;

        if (page == null)
        {
            return NotFound();
        }

        var content =
            await _repository.GetByIdAsync(page.WebPageItemID, page.LanguageName, 1, HttpContext.RequestAborted);


        if (content == null)
        {
            return NotFound();
        }

        return View(content);
    }

// Additional actions can be added here
}
```

### Step 5: Define Your Page Type

Define your `HomePage` class that implements `IWebpageFieldSource`:

```csharp
using XperienceCommunity.DataRepository;
public class HomePage : IWebpageFieldSource
{
    // Define properties and methods for HomePage
}
```

### Step 6: Use the Controller in Your Application

Add the `HomeController` to your application's routing and views as needed.

## Conclusion

By following these steps, you can set up and use the `XperienceCommunity.DataRepository` library to simplify data access and management in your Xperience by Kentico applications. The repository pattern helps promote separation of concerns and improves code maintainability and testability.
