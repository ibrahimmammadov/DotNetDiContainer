# Custom Dependency Injection (DI) Container in .NET 8

## Overview

This project implements a custom Dependency Injection (DI) container in .NET 8, supporting Singleton, Transient, and Scoped lifetimes. The DI container provides a simple yet flexible way to manage service lifetimes and dependencies within your .NET applications.

## Features

- **Singleton**: Ensures a single instance of the service is created and shared throughout the application's lifetime.
- **Transient**: Creates a new instance of the service every time it is requested.
- **Scoped**: Creates a new instance of the service per scope, ensuring the same instance is used within the same scope but different instances across different scopes.
- **Flexible Initialization**: Supports multiple ways to register services, including direct registration, using `ServiceDescriptor`, and through factory functions.

## Usage

### Registering Services

You can register your services using the `ServiceCollection` class. The DI container supports various ways to initialize and register services:

1. **Direct Registration**: Register a service directly by specifying the interface and implementation:

    ```csharp
    var services = new ServiceCollection();
    services.AddSingleton<IMyService, MyService>();
    ```

2. **Using `ServiceDescriptor`**: Register a service using a `ServiceDescriptor` object:

    ```csharp
    services.AddService(new ServiceDescriptor
    {
        ImplementationType = typeof(MyService),
        ServiceType = typeof(IMyService),
        Lifetime = ServiceLifetime.Singleton
    });
    ```

3. **Using Factory Functions**: Register a service using a factory function, which is particularly useful for transient services:

    ```csharp
    services.AddTransient(provider => new MyService(provider.GetService<AnotherService>()!));
    ```

4. **Registering Pre-Initialized Instances**: Register services that are already initialized:

    ```csharp
    services.AddSingleton(new IdGenerator(new ConsoleWriter()));
    ```

5. **Registering Concrete Types Without Interfaces**: Register a concrete type without specifying an interface:

    ```csharp
    services.AddSingleton<ConsoleWriter>();
    ```

### Resolving Services

To resolve a service, use the `GetService<T>()` method:

```csharp
var myService = serviceProvider.GetService<IMyService>();


