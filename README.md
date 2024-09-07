# Design Patterns

- [Creational Patterns](#creational-patterns)
  - [Singleton](#singleton)
  - [Factory Method](#factory-method)
  - [Abstract Factory](#abstract-factory)
  - [Builder](#builder)
  - [Prototype](#prototype)
- [Structural Patterns](#structural-patterns)
  - [Adapter](#adapter)
  - [Bridge](#bridge)
  - [Decorator](#decorator)
  - [Composite](#composite)
  - [Facade](#facade)
  - [Proxy](#proxy)
  - [Flyweight](#flyweight)
- [Behavioral Patterns](#behavioral-patterns)
  - [Template](#template)
  - [Strategy](#strategy)
  - [Command](#command)
  - [Momento](#momento)
  - [Mediator](#mediator)
  - [Chain of Responsibility](#chain-of-responsibility)
  - [Observer](#observer)
  - [State](#state)
  - [Iterator](#iterator)
  - [Visitor](#visitor)
  - [Interpreter](#interpreter)
  - [Repository](#repository)
  - [Unit of Work](#unit-of-work)
 

## Creational Patterns

These patterns deal with object creation.
- Abstract the object instantiation process.
- Help us make your system independent of how its object is created, composed, and represented.

### Singleton

Singleton lets you access your object from anywhere in your application. It guarantees that only one instance of this class will be available a time.

Holding the class instance in a global variable doesn’t prevent clients from creating other instances of the class. We need to make the class responsible for ensuring only one instance of itself exists.

**Use Cases for the Singleton Pattern**

- Should be used when a class must have a single instance available, and it must be accessible to clients from a well-known access point.

- When the sole instance should be extensible by subclassing. and clients should be able to use an extended instance without modifying their code.

**Examples**

- Managing a database connection pool.

- Caching frequently accessed data (Single instance to evict the cache easily).

- Managing application configuration settings.

Violated the single responsibility principle as the object creating and the lifetime of the object is maintained by the class.

```csharp
// Program.cs

using Singleton;

Console.Title = "Singleton";

// call the property getter twice
var instance1 = Logger.Instance;
var instance2 = Logger.Instance;

if (instance1 == instance2)
{
    Console.WriteLine("Instances are the same.");
}

instance1.Log($"Message from {nameof(instance1)}");
// or
instance2.Log($"Message from {nameof(instance2)}");

Console.ReadLine();
```

```csharp
// Implementation.cs
// without handling the multithreading

namespace Singleton
{
    // Singleton
    public class Logger
    {
        private static Logger? _instance = null;
        // Instance
        public static Logger Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Logger();
                }
                return _instance;
            }
        }

        protected Logger() { }

        // SingletonOperation
        public void Log(string message)
        {
            Console.WriteLine($"Message to log: {message}");
        }
    }
}
```

```csharp
// Implementation.cs
// with handling the multithreading by lazy loading

namespace Singleton
{
    // Singleton
    public class Logger
    {
        // Lazy<T> for lazy initialization
        private static readonly Lazy<Logger> _lazyLogger = new(() => new Logger());

        // Instance
        public static Logger Instance
        {
            get
            {
                return _lazyLogger.Value;
            }
        }

        protected Logger() { }

        // SingletonOperation
        public void Log(string message)
        {
            Console.WriteLine($"Message to log: {message}");
        }
    }
}

```

### Factory Method

### Abstract Factory

## Structural Patterns

### Adapter

### Decorator

### Facade

## Behavioral Patterns

### Command

### Observer

