# DependencyInjection.SourceGenerators

[![MIT License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)
[![GitHub issues](https://img.shields.io/github/issues/jimmy-mll/Microsoft.Extensions.DependencyInjection.SourceGenerators)](https://github.com/jimmy-mll/Microsoft.Extensions.DependencyInjection.SourceGenerators/issues)
[![GitHub stars](https://img.shields.io/github/stars/jimmy-mll/Microsoft.Extensions.DependencyInjection.SourceGenerators)](https://github.com/jimmy-mll/Microsoft.Extensions.DependencyInjection.SourceGenerators/stargazers)
[![GitHub forks](https://img.shields.io/github/forks/jimmy-mll/Microsoft.Extensions.DependencyInjection.SourceGenerators)](https://github.com/jimmy-mll/Microsoft.Extensions.DependencyInjection.SourceGenerators/network)
[![GitHub pull requests](https://img.shields.io/github/issues-pr/jimmy-mll/Microsoft.Extensions.DependencyInjection.SourceGenerators)](https://github.com/jimmy-mll/Microsoft.Extensions.DependencyInjection.SourceGenerators/pulls)
[![GitHub contributors](https://img.shields.io/github/contributors/jimmy-mll/Microsoft.Extensions.DependencyInjection.SourceGenerators)](https://github.com/jimmy-mll/Microsoft.Extensions.DependencyInjection.SourceGenerators/graphs/contributors)
[![Github workflows status](https://github.com/jimmy-mll/Microsoft.Extensions.DependencyInjection.SourceGenerators/actions/workflows/dotnet.yml/badge.svg?branch=main)](https://github.com/jimmy-mll/Microsoft.Extensions.DependencyInjection.SourceGenerators/actions/workflows/dotnet.yml)
![GitHub code size in bytes](https://img.shields.io/github/languages/code-size/jimmy-mll/Microsoft.Extensions.DependencyInjection.SourceGenerators)

## Overview

**DependencyInjection.SourceGenerators** is an advanced C# source generator specifically crafted for the **Microsoft.Extensions.DependencyInjection** library. It automates the process of service registration, eliminating the need for manual or reflection-based registration in .NET applications.

## Key Features
- **Automatic Registration**: ARI generates code to automatically register services in the DI container using custom attributes.
- **Compile-Time Efficiency**: Operates at compile time, offering a more efficient alternative to reflection or manual updates in ServiceCollection.
- **Simplified Dependency Management**: Greatly reduces the complexity of managing dependencies, especially in larger projects.
- **Attribute-Based Configuration**: Services are registered through easy-to-use attributes, streamlining the process.
- **Interface Registration Support**: Allows for clean and concise registration of services through their interfaces.
- **Multi-Assembly Support**: Facilitates service registration across multiple assemblies, enhancing modularity.
- **Keyed Service Support**: Enables registration of multiple implementations of the same interface with unique keys, allowing for more nuanced dependency resolution.

## Installation

**DependencyInjection.SourceGenerators** is available as a NuGet package. You can install it using the following command:

```shell
dotnet add package DependencyInjection.SourceGenerators
```

Or, add the package reference manually in your project file:

```xml
<PackageReference Include="DependencyInjection.SourceGenerators" Version="1.0.0" />
```

## Usage

### Basic Registration

Decorate your classes with one of the following attributes to register them in the DI container:

- **`[Singleton]`**
- **`[Scoped]`**
- **`[Transient]`**

Example: 

```csharp
[Singleton]
public class MyService;
```

This will generate:

```csharp
services.AddSingleton<MyService>();
```

### Registering Interfaces

You can also register services through their interfaces:

```csharp
[Singleton]
public class MyService : IMyService;
```

This will generate:

```csharp
services.AddSingleton<IMyService, MyService>();
```

### Keyed Service Registration

To use Keyed Services, you can annotate your service implementations with a special attribute indicating the key:

```csharp
[Singleton("myKey")]
public class MyService : IMyService;
```

This will generate:

```csharp
services.AddSingleton<IMyService, MyService>("myKey");
```

## Multi-Assembly Configuration

**DependencyInjection.SourceGenerators** supports multi-assembly scenarios. For example, with assemblies **MyProject.Main**, **MyProject.Services**, and **MyProject.Data**, configure your ServiceCollection as follows:

```csharp
var serviceCollection = new ServiceCollection();
serviceCollection.AddServicesFromMainAssembly();
serviceCollection.AddServicesFromServicesAssembly();
serviceCollection.AddServicesFromDataAssembly();
serviceCollection.BuildServiceProvider();
```

## Enhanced Service Registration with Assembly-Level Attributes

**DependencyInjection.SourceGenerators** introduces an innovative and streamlined approach to service registration in .NET applications by leveraging assembly-level attributes. This feature allows developers to define their dependency injection mappings at a single, centralized location, enhancing readability and maintainability.

### Key Advantages

- **Centralized Configuration**: Define all your DI mappings in one place, making it easier to manage and review.
- **Declarative Syntax**: A clear and concise way to express service registrations, improving code clarity.
- **Reduced Boilerplate**: Minimizes repetitive code across different parts of the application.

### Usage

You can register your services directly at the assembly level using attributes like **Singleton**, **Transient**, and **Scoped**. This method is particularly useful for large projects with numerous services, as it centralizes the DI configuration.

Example:

```csharp
[assembly: Singleton<One>, Transient<ITwo, Two>, Scoped<Three>("myKey")]
```

In this example:

- **`One`** is registered as a singleton. This means a single instance of One will be created and shared across the application.
- **`Two`** is registered as a transient implementation of the ITwo interface. This means a new instance of Two will be created each time ITwo is requested.
- **`Three`** is registered as a scoped service with a key "myKey". This means an instance of Three will be created for each scope (like a web request in ASP.NET Core) and it can be uniquely identified or resolved using the key "myKey".
This configuration is declared at the assembly level, applying to the entire assembly where it's defined. It provides a centralized and streamlined way to manage dependency injection across your application.

### Implementation

To utilize this feature, simply place the assembly attribute in any file within your project, typically at the top of the file for visibility. The **DependencyInjection.SourceGenerators** tool will scan these attributes during compilation and generate the necessary DI registration code.

This approach, as described, offers a more organized and efficient way to handle dependency injection in .NET applications, especially beneficial in complex projects with a large number of services.

## License

**DependencyInjection.SourceGenerators** is released under the MIT License, offering freedom and flexibility for both personal and commercial use under the terms of MIT.