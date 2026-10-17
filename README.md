<div align="center">

# 📐 C# Design Patterns

### Comprehensive, practical guide and reference implementation of Design Patterns in C# and .NET 8

[![Language](https://img.shields.io/badge/Language-C%23%2012-239120?style=flat-square&logo=csharp&logoColor=white)](https://learn.microsoft.com/en-us/dotnet/csharp/)
[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=flat-square&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![Patterns](https://img.shields.io/badge/Patterns-25%20Implemented-blue?style=flat-square)](https://github.com/akazad13/csharp-design-patterns)
[![License](https://img.shields.io/badge/License-MIT-blue?style=flat-square)](LICENSE)

[Overview](#-overview) •
[Creational](#creational-patterns) •
[Structural](#structural-patterns) •
[Behavioral](#behavioral-patterns) •
[Architecture Patterns](#repository) •
[Table of Contents](#table-of-contents)

</div>

---

## 📌 Overview

**C# Design Patterns** is a comprehensive, production-oriented reference guide to Gang of Four (GoF) and enterprise software design patterns implemented in **C# (.NET 8)**. Every pattern features an intuitive real-world analogy, production software engineering scenarios, trade-off analysis, and runnable implementations.

### Key Highlights
- 🌍 **Real-Life Analogy**: Intuitive non-technical everyday metaphors making patterns stick.
- 💻 **Software Engineering Scenarios**: Real production use cases (logging, caching, connection pooling, UI pipelines).
- ❓ **Problem It Solves**: Clear comparison of code *without* the pattern vs. *with* the pattern.
- 🛠️ **Runnable C# Examples**: Tested, idiomatic C# matching modern language features (pattern matching, records, lazy loading).

---

## Table of Contents

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

---

## Creational Patterns

These patterns deal with object creation mechanisms, abstracting the instantiation process and making systems independent of how objects are created, composed, and represented.

---

### Singleton

> **Intent**: Ensure a class has only one instance and provide a global point of access to it.

#### 🌍 Real-Life Analogy: The President of a Country
A country can have only one official President at any given time. Regardless of who needs to address the President — diplomats, ministers, or citizens — they all refer to the exact same person. Creating multiple presidents simultaneously would cause administrative chaos and conflicting commands.

#### 💻 Software Engineering Scenario
- **Logging System**: A central logging service (`Logger`) that writes entries to a single disk file. If multiple logger instances existed, file write locks would conflict, or log lines would become interleaved and corrupted.
- **Application Configuration**: A central manager holding application configuration loaded from `appsettings.json`.
- **Database Connection Pool**: A shared manager governing finite network database connections.

#### ❓ The Problem It Solves
- **Without Singleton**: Different parts of the app instantiate their own loggers or cache managers. Memory is duplicated, cache synchronization is lost, and concurrent file-write conflicts occur.
- **With Singleton**: The class itself controls its own instantiation, providing thread-safe lazy loading and a single shared instance throughout the application lifecycle.

```csharp
// Implementation.cs (Thread-safe with Lazy<T>)
namespace Singleton
{
    public class Logger
    {
        private static readonly Lazy<Logger> _lazyLogger = new(() => new Logger());

        public static Logger Instance => _lazyLogger.Value;

        protected Logger() { }

        public void Log(string message)
        {
            Console.WriteLine($"Message to log: {message}");
        }
    }
}
```

```csharp
// Program.cs
using Singleton;

Console.Title = "Singleton";

var instance1 = Logger.Instance;
var instance2 = Logger.Instance;

if (instance1 == instance2)
{
    Console.WriteLine("Instances are the same.");
}

instance1.Log($"Message from {nameof(instance1)}");
instance2.Log($"Message from {nameof(instance2)}");
```

---

### Factory Method

> **Intent**: Define an interface or abstract class for creating an object, but let subclasses decide which class to instantiate.

#### 🌍 Real-Life Analogy: Fast Food Kitchen Franchise
A restaurant chain headquarters (`Restaurant`) defines the standard workflow for every branch: take customer payment, prepare the burger, and serve it (`OrderBurger()`). However, a Beef Burger branch cooks a `BeefBurger`, while a Vegan branch cooks a `VeggieBurger`. The order procedure is uniform, but each specific branch decides *which* product to instantiate.

#### 💻 Software Engineering Scenario
- **Discount Services**: An e-commerce system where discount calculations vary by country (`CountryDiscountService`) or coupon code (`CodeDiscountService`). The order processing logic relies on a base `DiscountFactory` without knowing the exact discount rules at compile time.
- **Cross-Platform UI Frameworks**: A windowing framework where `CreateButton()` renders a `WindowsButton` on Windows and a `MacButton` on macOS.

#### ❓ The Problem It Solves
- **Without Factory Method**: The client code is littered with tight coupling and `switch/case` statements (`if (type == "Beef") new BeefBurger() else if ...`). Adding a new burger requires changing and recompiling the main ordering code.
- **With Factory Method**: The creator class is decoupled from product creation. Adding a `ChickenBurger` simply requires adding a new `ChickenBurgerRestaurant` subclass, following the Open/Closed Principle.

```csharp
// BurgerFactoryMethod.cs
namespace FactoryMethod
{
    public abstract class Burger
    {
        public int ProductId { get; set; }
        public abstract void Prepare();
    }

    public class BeefBurger : Burger
    {
        public bool Angus { get; set; }
        public override void Prepare() => Console.WriteLine("Preparing Beef Burger");
    }

    public class VeggieBurger : Burger
    {
        public bool Combo { get; set; }
        public override void Prepare() => Console.WriteLine("Preparing Veggie Burger");
    }

    public abstract class Restaurant
    {
        public Burger OrderBurger()
        {
            Burger burger = CreateBurger();
            burger.Prepare();
            return burger;
        }

        public abstract Burger CreateBurger();
    }

    public class BeefBurgerRestaurant : Restaurant
    {
        public override Burger CreateBurger() => new BeefBurger();
    }

    public class VeggieBurgerRestaurant : Restaurant
    {
        public override Burger CreateBurger() => new VeggieBurger();
    }
}
```

```csharp
// Program.cs
using FactoryMethod;

Restaurant beefBurgerFactory = new BeefBurgerRestaurant();
Burger beefBurger = beefBurgerFactory.OrderBurger();

Restaurant veggieBurgerFactory = new VeggieBurgerRestaurant();
Burger veggieBurger = veggieBurgerFactory.OrderBurger();
```

---

### Abstract Factory

> **Intent**: Provide an interface for creating families of related or dependent objects without specifying their concrete classes.

#### 🌍 Real-Life Analogy: Themed Furniture Store
Imagine you are furnishing your living room. You can choose a **Modern Style** set or a **Victorian Style** set. If you pick the Modern Style Factory, it gives you a *Modern Chair*, a *Modern Sofa*, and a *Modern Coffee Table*. If you choose the Victorian Factory, it gives you a *Victorian Chair*, *Victorian Sofa*, and *Victorian Coffee Table*. The factory guarantees that all pieces match each other in style, preventing you from awkwardly pairing a Victorian carved armchair with a neon minimalist table.

#### 💻 Software Engineering Scenario
- **Regional E-Commerce Checkout**: In international e-commerce, checkout requires both shipping calculation and tax/discount calculation. An `IndiaShoppingCartPurchaseFactory` creates an `IndiaShippingCostsService` and `IndiaDiscountService`. A `ChinaShoppingCartPurchaseFactory` creates `ChinaShippingCostsService` and `ChinaDiscountService`. The checkout client never mixes Indian shipping rates with Chinese discount percentages.

#### ❓ The Problem It Solves
- **Without Abstract Factory**: Client classes manually instantiate individual services (`new ChinaShipping()` and `new IndiaDiscount()`). Incompatible combinations lead to billing and calculation errors.
- **With Abstract Factory**: Client code receives a factory interface and creates mutually compatible product families without knowing the specific concrete classes.

```csharp
// ShoppingCartFactory.cs
namespace AbstractFactory
{
    public interface IShoppingCartPurchaseFactory
    {
        IShippingCostsService CreateShippingCostsService();
        IDiscountService CreateDiscountService();
    }

    public interface IDiscountService
    {
        int DiscountPercentage { get; set; }
    }

    public interface IShippingCostsService
    {
        decimal ShippingCost { get; set; }
    }

    public class IndiaDiscountService : IDiscountService
    {
        public int DiscountPercentage { get; set; } = 10;
    }

    public class IndiaShippingCostsService : IShippingCostsService
    {
        public decimal ShippingCost { get; set; } = 20;
    }

    public class IndiaShoppingCartPurchaseFactory : IShoppingCartPurchaseFactory
    {
        public IShippingCostsService CreateShippingCostsService() => new IndiaShippingCostsService();
        public IDiscountService CreateDiscountService() => new IndiaDiscountService();
    }

    public class ShoppingCart
    {
        private readonly IShippingCostsService _shippingCostsService;
        private readonly IDiscountService _discountService;
        private decimal _costOfOrders = 300;

        public ShoppingCart(IShoppingCartPurchaseFactory factory)
        {
            _discountService = factory.CreateDiscountService();
            _shippingCostsService = factory.CreateShippingCostsService();
        }

        public void CalculateCost()
        {
            var discountAmount = (decimal)_discountService.DiscountPercentage / 100 * _costOfOrders;
            var totalCost = _costOfOrders - discountAmount + _shippingCostsService.ShippingCost;
            Console.WriteLine($"Total Cost: {totalCost}");
        }
    }
}
```

```csharp
// Program.cs
using AbstractFactory;

var cart = new ShoppingCart(new IndiaShoppingCartPurchaseFactory());
cart.CalculateCost();
```

---

### Builder

> **Intent**: Separate the construction of a complex object from its representation so that the same construction process can create different representations.

#### 🌍 Real-Life Analogy: Ordering at Subway or Custom PC Assembly
When you order a sandwich at Subway, you don't pick a pre-made sandwich from a bin. You tell the sandwich artist step-by-step: choose bread $\rightarrow$ select protein $\rightarrow$ add cheese $\rightarrow$ choose veggies $\rightarrow$ add dressings $\rightarrow$ toast. The same step-by-step construction process can assemble a Veggie Sub, a Tuna Sub, or a Steak & Cheese Sub.

#### 💻 Software Engineering Scenario
- **Vehicle Configuration**: Assembling cars with various combinations of engine, GPS, sunroof, seats, and doors.
- **SQL Query / HTTP Request Construction**: Constructing complex SQL queries (`queryBuilder.Select("name").From("users").Where("age > 18").OrderBy("name").Build()`) or fluent API builders.

#### ❓ The Problem It Solves
- **Without Builder**: You end up with a constructor with 10–15 arguments (`new Car("Tesla", "Model S", 4, "Electric", true, true, false, "White", 21, true...)`), known as the "telescoping constructor" anti-pattern. Most arguments are optional or `null`, making code unreadable and error-prone.
- **With Builder**: Object construction is step-by-step and readable. Optional features are set fluently, and a `Director` can encapsulate pre-configured popular templates.

```csharp
// Implementation.cs
namespace Builder
{
    public class Car
    {
        public string Make { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int NumDoors { get; set; }
        public string Engine { get; set; } = string.Empty;
        public bool HasGps { get; set; }
        public bool HasSunroof { get; set; }

        public override string ToString() =>
            $"{Make} {Model} [Doors: {NumDoors}, Engine: {Engine}, GPS: {HasGps}, Sunroof: {HasSunroof}]";
    }

    public interface ICarBuilder
    {
        ICarBuilder SetMake(string make);
        ICarBuilder SetModel(string model);
        ICarBuilder SetNumDoors(int numDoors);
        ICarBuilder SetEngine(string engine);
        ICarBuilder SetGps(bool hasGps);
        ICarBuilder SetSunroof(bool hasSunroof);
        Car Build();
    }

    public class CarBuilder : ICarBuilder
    {
        private Car _car = new();

        public ICarBuilder SetMake(string make) { _car.Make = make; return this; }
        public ICarBuilder SetModel(string model) { _car.Model = model; return this; }
        public ICarBuilder SetNumDoors(int doors) { _car.NumDoors = doors; return this; }
        public ICarBuilder SetEngine(string engine) { _car.Engine = engine; return this; }
        public ICarBuilder SetGps(bool gps) { _car.HasGps = gps; return this; }
        public ICarBuilder SetSunroof(bool sunroof) { _car.HasSunroof = sunroof; return this; }

        public Car Build()
        {
            var result = _car;
            _car = new Car();
            return result;
        }
    }

    public class CarDirector
    {
        private readonly ICarBuilder _builder;
        public CarDirector(ICarBuilder builder) => _builder = builder;

        public Car ConstructSportsCar() => _builder
            .SetMake("Ferrari").SetModel("488 GTB").SetNumDoors(2)
            .SetEngine("V8 Twin Turbo").SetGps(true).SetSunroof(false).Build();
    }
}
```

```csharp
// Program.cs
using Builder;

var builder = new CarBuilder();
var director = new CarDirector(builder);

var sportsCar = director.ConstructSportsCar();
Console.WriteLine($"Sports Car: {sportsCar}");

var customCar = builder.SetMake("Tesla").SetModel("Model S Plaid")
                       .SetNumDoors(4).SetEngine("Tri-Motor Electric").SetGps(true).Build();
Console.WriteLine($"Custom Car: {customCar}");
```

---

### Prototype

> **Intent**: Specify the kinds of objects to create using a prototypical instance, and create new objects by copying this prototype.

#### 🌍 Real-Life Analogy: Photocopier / Cell Division (Mitosis)
In biology, when a cell needs to reproduce, it doesn't construct a brand-new cell atom by atom from raw chemical broth; it duplicates its existing DNA and splits into two identical daughter cells. Similarly, if you need 50 copies of a standard legal contract, you don't retype the entire 10-page document 50 times; you photocopy the master draft (prototype) and customize the signatures.

#### 💻 Software Engineering Scenario
- **Document / Template Duplication**: Creating new invoices, resumes, or contract documents initialized with standard company clauses and layout headers.
- **Game Development (Enemy Spawning)**: Spawning 200 Goblin monsters in a game dungeon. Instead of re-parsing 3D meshes, skeleton animations, and audio effects from disk for each enemy, you clone an in-memory `Goblin` prototype and randomize health and position.

#### ❓ The Problem It Solves
- **Without Prototype**: Instantiating a complex object requires repeating expensive initialization logic, disk I/O, database queries, and private state configuration.
- **With Prototype**: The object itself implements cloning (shallow or deep copy), producing an independent duplicate instantaneously without coupling the client to concrete construction logic.

```csharp
// Implementation.cs
namespace Prototype
{
    public interface IPrototype<T>
    {
        T Clone(bool deepClone = true);
    }

    public class Author
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public Author(string name, string email) { Name = name; Email = email; }
    }

    public class Document : IPrototype<Document>
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public Author Author { get; set; }
        public List<string> Tags { get; set; }

        public Document(string title, string content, Author author, List<string>? tags = null)
        {
            Title = title;
            Content = content;
            Author = author;
            Tags = tags ?? [];
        }

        public Document Clone(bool deepClone = true)
        {
            if (deepClone)
            {
                // Deep copy: duplicate reference objects
                return new Document(Title, Content, new Author(Author.Name, Author.Email), new List<string>(Tags));
            }
            // Shallow copy: shares reference types
            return (Document)MemberwiseClone();
        }
    }
}
```

```csharp
// Program.cs
using Prototype;

var original = new Document("Design Patterns Handbook", "C# Architecture", new Author("Abul Kalam", "dev@example.com"), ["C#", "OOP"]);
var copy = original.Clone(deepClone: true);
copy.Title = "Design Patterns Handbook - 2nd Edition";
copy.Author.Name = "John Doe"; // Modifying copy does not mutate original!

Console.WriteLine($"Original Document Author: {original.Author.Name}"); // Abul Kalam
Console.WriteLine($"Cloned Document Author: {copy.Author.Name}");     // John Doe
```

---

## Structural Patterns

Structural patterns explain how to assemble objects and classes into larger structures while keeping these structures flexible and efficient.

---

### Adapter

> **Intent**: Convert the interface of a class into another interface clients expect. Adapter lets classes work together that couldn't otherwise because of incompatible interfaces.

#### 🌍 Real-Life Analogy: International Power Plug Adapter
When you travel from the US to the UK, your American laptop charger has two flat prongs, but the UK hotel wall socket expects three rectangular prongs. You don't rewire the hotel wall, nor do you buy a new laptop. You plug a compact UK-to-US power adapter between your plug and the wall socket. The adapter bridges the mismatch seamlessly.

#### 💻 Software Engineering Scenario
- **Third-Party / Legacy API Integration**: Your internal domain expects an `ICityAdapter` returning `City(FullName, Inhabitants)`. However, a legacy third-party service returns an external object `CityFromExternalSystem` with separate fields `Name`, `KnownAs`, and `Inhabitants`. The adapter wraps the legacy service and translates the data.

#### ❓ The Problem It Solves
- **Without Adapter**: You must rewrite your core domain code to match every external vendor's idiosyncratic data format, creating tight coupling to third-party models.
- **With Adapter**: Your domain code depends only on your clean interface. Adapters translate between external libraries and your application, preserving separation of concerns.

```csharp
// ObjectAdapterImplementation.cs
namespace ObjectAdapter
{
    public class CityFromExternalSystem
    {
        public string Name { get; set; }
        public string KnownAs { get; set; }
        public int Inhabitants { get; set; }
    }

    public class ExternalSystem
    {
        public CityFromExternalSystem GetCity() => new("Dhaka", "Capital of Bangladesh", 100);
    }

    public record City(string FullName, long Inhabitants);

    public interface ICityAdapter
    {
        City GetCity();
    }

    public class CityAdapter : ICityAdapter
    {
        private readonly ExternalSystem _externalSystem = new();

        public City GetCity()
        {
            var ext = _externalSystem.GetCity();
            return new City($"{ext.Name}, {ext.KnownAs}", ext.Inhabitants);
        }
    }
}
```

```csharp
// Program.cs
using ObjectAdapter;

ICityAdapter adapter = new CityAdapter();
var city = adapter.GetCity();
Console.WriteLine($"City: {city.FullName}, Population: {city.Inhabitants}");
```

---

### Bridge

> **Intent**: Decouple an abstraction from its implementation so that the two can vary independently.

#### 🌍 Real-Life Analogy: Universal Remote Control & Electronic Appliances
Imagine you have 2 remotes (Basic Remote with Power/Volume, and Advanced Remote with Mute/Favorites) and 2 appliances (TV and Radio). If you used inheritance, you would need 4 classes: `BasicTvRemote`, `AdvancedTvRemote`, `BasicRadioRemote`, `AdvancedRadioRemote`. Adding a third appliance (e.g. Smart Projector) forces you to create 2 more remote classes ($N \times M$ class explosion). With Bridge, the remote control (Abstraction) and the appliance (Implementation) are separate hierarchies. Any remote can hold a reference to any device.

#### 💻 Software Engineering Scenario
- **Cross-Platform Graphics Rendering**: High-level shape abstractions (`Circle`, `Rectangle`) separated from low-level rendering implementations (`DirectXRenderer`, `OpenGLRenderer`, `MetalRenderer`).

#### ❓ The Problem It Solves
- **Without Bridge**: Subclassing creates a combinatorial explosion of classes when abstractions and implementations evolve together.
- **With Bridge**: You switch or extend implementations at runtime via composition without touching the abstraction hierarchy.

```csharp
// Implementation.cs
namespace Bridge
{
    public interface IDevice
    {
        bool IsEnabled { get; }
        void Enable();
        void Disable();
        int Volume { get; set; }
        int Channel { get; set; }
        void PrintStatus();
    }

    public class Tv : IDevice
    {
        public bool IsEnabled { get; private set; }
        public int Volume { get; set; } = 30;
        public int Channel { get; set; } = 1;
        public void Enable() => IsEnabled = true;
        public void Disable() => IsEnabled = false;
        public void PrintStatus() => Console.WriteLine($"TV [Power: {IsEnabled}, Volume: {Volume}]");
    }

    public class RemoteControl
    {
        protected readonly IDevice _device;
        public RemoteControl(IDevice device) => _device = device;
        public void TogglePower() { if (_device.IsEnabled) _device.Disable(); else _device.Enable(); }
        public void VolumeUp() => _device.Volume += 10;
    }

    public class AdvancedRemoteControl : RemoteControl
    {
        public AdvancedRemoteControl(IDevice device) : base(device) { }
        public void Mute() => _device.Volume = 0;
    }
}
```

```csharp
// Program.cs
using Bridge;

var tv = new Tv();
var remote = new AdvancedRemoteControl(tv);

remote.TogglePower();
remote.VolumeUp();
tv.PrintStatus();
remote.Mute();
tv.PrintStatus();
```

---

### Decorator

> **Intent**: Attach additional responsibilities to an object dynamically. Decorators provide a flexible alternative to subclassing for extending functionality.

#### 🌍 Real-Life Analogy: Dressing in Layers / Custom Coffee Drinks
When it gets cold, you put on a t-shirt. If it gets colder, you put on a sweater over the t-shirt. If it rains, you put on a raincoat over the sweater. Each layer "decorates" you with new capabilities (warmth, water resistance) without changing who you are. Similarly, at a coffee shop, you start with base black coffee and dynamically add milk, whipped cream, or caramel drizzle.

#### 💻 Software Engineering Scenario
- **Mail Services**: Starting with a core `CloudMailService`, you wrap it with a `StatisticsDecorator` to track email delivery metrics, and wrap it further with a `MessageDatabasesDecorator` to archive copies of all sent emails.
- **I/O Streams in .NET**: `new GZipStream(new CryptoStream(new FileStream(...)))`.

#### ❓ The Problem It Solves
- **Without Decorator**: Extending behavior with inheritance leads to rigid, exploding class combinations: `LoggingCloudMailService`, `ArchivingLoggingCloudMailService`, `ArchivingOnPremiseMailService`.
- **With Decorator**: Behaviors are modular wrappers that can be stacked in any order at runtime.

```csharp
// MailService.cs
namespace Decorator
{
    public interface IMailService
    {
        bool SendMail(string message);
    }

    public class CloudMailService : IMailService
    {
        public bool SendMail(string message)
        {
            Console.WriteLine($"Message \"{message}\" sent via CloudMailService.");
            return true;
        }
    }

    public abstract class MailServiceDecoratorBase : IMailService
    {
        private readonly IMailService _mailService;
        public MailServiceDecoratorBase(IMailService mailService) => _mailService = mailService;
        public virtual bool SendMail(string message) => _mailService.SendMail(message);
    }

    public class StatisticsDecorator : MailServiceDecoratorBase
    {
        public StatisticsDecorator(IMailService mailService) : base(mailService) { }
        public override bool SendMail(string message)
        {
            Console.WriteLine("Collecting statistics in StatisticsDecorator.");
            return base.SendMail(message);
        }
    }
}
```

```csharp
// Program.cs
using Decorator;

IMailService mailService = new CloudMailService();
var decoratedService = new StatisticsDecorator(mailService);
decoratedService.SendMail("Welcome to our service!");
```

---

### Composite

> **Intent**: Compose objects into tree structures to represent part-whole hierarchies. Composite lets clients treat individual objects and compositions of objects uniformly.

#### 🌍 Real-Life Analogy: Delivery Boxes / Military Hierarchy
In a warehouse, an order might consist of small individual items (pens, cables) and shipping boxes. A shipping box can hold individual items AND smaller boxes. When calculating the total weight for postage, the warehouse scales treat an individual item (Leaf) and a nested box (Composite) identically: both have a `GetWeight()` method.

#### 💻 Software Engineering Scenario
- **File System**: Directories containing files and subdirectories. Calculating the size of a folder recursively sums the sizes of all contained files and child folders.
- **GUI Layout Components**: A window containing panels, which in turn contain buttons, labels, and nested panels.

#### ❓ The Problem It Solves
- **Without Composite**: Client code must constantly check types (`if (item is Directory) ... else if (item is File)`), writing complex conditional recursion everywhere.
- **With Composite**: Clients invoke `GetSize()` or `Display()` on the root component, and the tree traverses itself uniformly.

```csharp
// Implementation.cs
namespace Composite
{
    public abstract class FileSystemItem
    {
        public string Name { get; set; }
        protected FileSystemItem(string name) => Name = name;
        public abstract long GetSize();
        public abstract void Display(int depth = 0);
    }

    public class FileItem : FileSystemItem
    {
        public long Size { get; }
        public FileItem(string name, long size) : base(name) => Size = size;
        public override long GetSize() => Size;
        public override void Display(int depth = 0) =>
            Console.WriteLine($"{new string(' ', depth * 2)}- File: {Name} ({Size} KB)");
    }

    public class DirectoryItem : FileSystemItem
    {
        private readonly List<FileSystemItem> _children = [];
        public DirectoryItem(string name) : base(name) { }
        public void Add(FileSystemItem item) => _children.Add(item);
        public override long GetSize() => _children.Sum(c => c.GetSize());
        public override void Display(int depth = 0)
        {
            Console.WriteLine($"{new string(' ', depth * 2)}+ Directory: {Name} ({GetSize()} KB)");
            foreach (var child in _children) child.Display(depth + 1);
        }
    }
}
```

```csharp
// Program.cs
using Composite;

var root = new DirectoryItem("Root");
var docs = new DirectoryItem("Documents");
docs.Add(new FileItem("Resume.pdf", 250));
root.Add(docs);
root.Add(new FileItem("readme.txt", 5));

root.Display();
Console.WriteLine($"Total Size: {root.GetSize()} KB");
```

---

### Facade

> **Intent**: Provide a unified interface to a set of interfaces in a subsystem. Facade defines a higher-level interface that makes the subsystem easier to use.

#### 🌍 Real-Life Analogy: Hotel Concierge
When staying at a luxury hotel, if you want dinner reservations, museum tickets, and airport transport, you don't call the restaurant kitchen, the museum box office, and the city taxi dispatcher yourself. You walk up to the hotel Concierge desk and make one request. The concierge coordinates all behind-the-scenes subsystems for you.

#### 💻 Software Engineering Scenario
- **Order Discount Calculation**: An e-commerce pricing engine where calculating a customer discount requires querying the `OrderService` (has customer ordered enough?), `CustomerDiscountBaseService` (tier calculation), and `DayOfTheWeekFactorService` (weekend multipliers). The client simply calls `DiscountFacade.CalculateDiscountPercentage(customerId)`.

#### ❓ The Problem It Solves
- **Without Facade**: Every controller or client class must directly instantiate and coordinate 5–10 interconnected subsystem services, creating high coupling.
- **With Facade**: A clean, single entry point simplifies usage and shields clients from internal subsystem complexity.

```csharp
// Implementation.cs
namespace Facade
{
    public class OrderService
    {
        public bool HasEnoughOrders(int customerId) => customerId > 5;
    }

    public class CustomerDiscountBaseService
    {
        public double CalculateDiscountBase(int customerId) => customerId > 8 ? 20 : 30;
    }

    public class DayOfTheWeekFactorService
    {
        public double CalculateDayOfTheWeekFactor() =>
            DateTime.UtcNow.DayOfWeek switch { DayOfWeek.Saturday or DayOfWeek.Sunday => 0.8, _ => 1.2 };
    }

    public class DiscountFacade
    {
        private readonly OrderService _orderService = new();
        private readonly CustomerDiscountBaseService _baseService = new();
        private readonly DayOfTheWeekFactorService _factorService = new();

        public double CalculateDiscountPercentage(int customerId)
        {
            if (!_orderService.HasEnoughOrders(customerId)) return 0;
            return _baseService.CalculateDiscountBase(customerId) * _factorService.CalculateDayOfTheWeekFactor();
        }
    }
}
```

```csharp
// Program.cs
using Facade;

var facade = new DiscountFacade();
Console.WriteLine($"Discount: {facade.CalculateDiscountPercentage(10)}%");
```

---

### Proxy

> **Intent**: Provide a surrogate or placeholder for another object to control access to it.

#### 🌍 Real-Life Analogy: Debit Card as a Proxy for Cash
A debit card is a proxy for the funds in your bank account. You don't walk into a store carrying \$5,000 cash in an envelope; you present a plastic card. The card implements the same payment interface as cash, but adds security (PIN verification), caching, and fraud protection before touching the actual money.

#### 💻 Software Engineering Scenario
- **Caching Video Streams**: When downloading video streams from an external API (`YouTubeService`), making network calls repeatedly wastes bandwidth and adds latency. A `CachedYouTubeServiceProxy` intercepts calls, returns cached videos instantly, and forwards requests to the real service only on cache misses.
- **Lazy Loading**: Postponing the initialization of large database objects until they are actually accessed.

#### ❓ The Problem It Solves
- **Without Proxy**: Clients call expensive remote services repeatedly, leading to poor performance, unnecessary network costs, and lack of access control.
- **With Proxy**: The proxy acts transparently with the same interface, adding caching, logging, or authentication without altering client or service code.

```csharp
// Implementation.cs
namespace Proxy
{
    public record Video(string Id, string Title, byte[] Data);

    public interface IVideoService
    {
        Video GetVideo(string videoId);
    }

    public class YouTubeService : IVideoService
    {
        public Video GetVideo(string videoId)
        {
            Console.WriteLine($"[YouTubeService] Downloading video '{videoId}'...");
            return new Video(videoId, $"Title for {videoId}", [0x01, 0x02]);
        }
    }

    public class CachedYouTubeServiceProxy : IVideoService
    {
        private readonly IVideoService _realService;
        private readonly Dictionary<string, Video> _cache = [];

        public CachedYouTubeServiceProxy(IVideoService realService) => _realService = realService;

        public Video GetVideo(string videoId)
        {
            if (_cache.TryGetValue(videoId, out var cached))
            {
                Console.WriteLine($"[Proxy] Returning '{videoId}' from cache.");
                return cached;
            }
            var video = _realService.GetVideo(videoId);
            _cache[videoId] = video;
            return video;
        }
    }
}
```

```csharp
// Program.cs
using Proxy;

IVideoService proxy = new CachedYouTubeServiceProxy(new YouTubeService());
proxy.GetVideo("design-patterns"); // Downloads via real service
proxy.GetVideo("design-patterns"); // Served from memory cache instantly
```

---

### Flyweight

> **Intent**: Use sharing to support large numbers of fine-grained objects efficiently.

#### 🌍 Real-Life Analogy: Printing Stamp / Video Game Forest
Imagine a video game that renders a forest of 1,000,000 oak trees. If each tree stored its own 3D mesh model, high-resolution bark textures, and leaf shaders (intrinsic state), the game would consume gigabytes of RAM and crash. Instead, the game stores **one single** `OakTreeType` in memory (Flyweight), and creates 1,000,000 small tree instances that only store `(X, Y)` coordinates (extrinsic state) pointing to that shared tree type.

#### 💻 Software Engineering Scenario
- **Document Text Editors**: Rendering 500,000 characters in a word processor. Instead of creating a heavy object for every letter, characters share flyweights representing font, size, and glyph graphics, storing only their position in the document.

#### ❓ The Problem It Solves
- **Without Flyweight**: Creating hundreds of thousands of objects exhausts available RAM, triggering frequent garbage collection spikes and out-of-memory crashes.
- **With Flyweight**: Reusable, immutable intrinsic state is cached and shared across millions of lightweight context instances.

```csharp
// Implementation.cs
namespace Flyweight
{
    public class TreeType
    {
        public string Name { get; }
        public string Color { get; }
        public string Texture { get; }
        public TreeType(string name, string color, string texture)
        { Name = name; Color = color; Texture = texture; }

        public void Draw(int x, int y) =>
            Console.WriteLine($"Drawing tree '{Name}' [{Color}] at ({x}, {y})");
    }

    public class TreeFactory
    {
        private static readonly Dictionary<string, TreeType> _types = [];

        public static TreeType GetTreeType(string name, string color, string texture)
        {
            string key = $"{name}_{color}_{texture}";
            if (!_types.TryGetValue(key, out var treeType))
            {
                treeType = new TreeType(name, color, texture);
                _types[key] = treeType;
            }
            return treeType;
        }

        public static int TotalFlyweights => _types.Count;
    }

    public class Tree
    {
        public int X { get; }
        public int Y { get; }
        public TreeType Type { get; }
        public Tree(int x, int y, TreeType type) { X = x; Y = y; Type = type; }
        public void Draw() => Type.Draw(X, Y);
    }
}
```

```csharp
// Program.cs
using Flyweight;

var type = TreeFactory.GetTreeType("Oak", "Dark Green", "Rough Bark");
var tree1 = new Tree(10, 20, type);
var tree2 = new Tree(50, 80, type);

tree1.Draw();
tree2.Draw();
Console.WriteLine($"Total Flyweights in Memory: {TreeFactory.TotalFlyweights}"); // 1
```

---

## Behavioral Patterns

Behavioral patterns identify common communication patterns between objects and realize these patterns to increase flexibility.

---

### Template

> **Intent**: Define the skeleton of an algorithm in an operation, deferring some steps to subclasses. Template Method lets subclasses redefine certain steps of an algorithm without changing the algorithm's structure.

#### 🌍 Real-Life Analogy: Brewing Hot Beverages (Tea vs. Coffee)
The recipe for brewing tea and coffee follows an identical sequence:
1. Boil water $\rightarrow$ 2. Brew ingredient in hot water $\rightarrow$ 3. Pour into cup $\rightarrow$ 4. Add condiments.
Step 1 and Step 3 are identical. Step 2 differs (steeping tea leaves vs. filtering coffee grounds) and Step 4 differs (adding lemon vs. adding milk and sugar). The overall recipe structure is fixed, but specific steps vary.

#### 💻 Software Engineering Scenario
- **Data Mining Pipeline**: A `DataMiner` class defining the strict execution workflow: `OpenFile()` $\rightarrow$ `ExtractRawData()` $\rightarrow$ `ParseData()` $\rightarrow$ `AnalyzeData()` $\rightarrow$ `CloseFile()`. Concrete subclasses `CsvDataMiner` and `PdfDataMiner` implement the specific format parsing while preserving the guaranteed order of operations.

#### ❓ The Problem It Solves
- **Without Template Method**: Multiple classes duplicate the overall workflow logic. Modifying the workflow requires updating every parser class, risking inconsistencies.
- **With Template Method**: The invariant algorithm is defined once in the base class, while variant steps are cleanly overridden in subclasses.

```csharp
// Implementation.cs
namespace Template
{
    public abstract class DataMiner
    {
        public void Mine(string path)
        {
            OpenFile(path);
            var raw = ExtractRawData();
            var parsed = ParseData(raw);
            AnalyzeData(parsed);
            CloseFile();
        }

        protected abstract void OpenFile(string path);
        protected abstract string ExtractRawData();
        protected abstract List<string> ParseData(string raw);
        protected virtual void AnalyzeData(List<string> data) =>
            Console.WriteLine($"Analyzed {data.Count} records.");
        protected abstract void CloseFile();
    }

    public class CsvDataMiner : DataMiner
    {
        protected override void OpenFile(string path) => Console.WriteLine($"Opened CSV: {path}");
        protected override string ExtractRawData() => "id,name\n1,Alice";
        protected override List<string> ParseData(string raw) => raw.Split('\n').Skip(1).ToList();
        protected override void CloseFile() => Console.WriteLine("Closed CSV stream.");
    }
}
```

```csharp
// Program.cs
using Template;

DataMiner miner = new CsvDataMiner();
miner.Mine("employees.csv");
```

---

### Strategy

> **Intent**: Define a family of algorithms, encapsulate each one, and make them interchangeable. Strategy lets the algorithm vary independently from clients that use it.

#### 🌍 Real-Life Analogy: Traveling to the Airport
When you need to travel to the airport, your goal is the same, but your transport strategy varies depending on budget and traffic: you can take a **Taxi** (fastest, expensive), take the **Subway** (cheap, fixed schedule), or **Bicycle** (free, energetic). You can swap your strategy right before leaving your house without changing your flight booking.

#### 💻 Software Engineering Scenario
- **E-Commerce Checkout Payment Methods**: Allowing customers to choose between `CreditCardPaymentStrategy`, `PayPalPaymentStrategy`, and `CryptoPaymentStrategy` at checkout.
- **Compression and Export**: Exporting a report as PDF, Excel, or CSV using interchangeable export strategies.

#### ❓ The Problem It Solves
- **Without Strategy**: The shopping cart checkout method has massive `if-else` or `switch` statements checking payment types. Adding a new payment provider requires editing and risking regressions in core checkout logic.
- **Without Strategy**: Violates Open/Closed Principle.
- **With Strategy**: Each algorithm is encapsulated in its own class implementing `IPaymentStrategy`. Adding a new payment method is as simple as creating a new class.

```csharp
// Implementation.cs
namespace Strategy
{
    public interface IPaymentStrategy
    {
        void Pay(decimal amount);
    }

    public class CreditCardPaymentStrategy : IPaymentStrategy
    {
        private readonly string _card;
        public CreditCardPaymentStrategy(string card) => _card = card;
        public void Pay(decimal amount) =>
            Console.WriteLine($"Paid ${amount:F2} with card ending in {_card[^4..]}.");
    }

    public class PayPalPaymentStrategy : IPaymentStrategy
    {
        private readonly string _email;
        public PayPalPaymentStrategy(string email) => _email = email;
        public void Pay(decimal amount) =>
            Console.WriteLine($"Paid ${amount:F2} via PayPal ({_email}).");
    }

    public class ShoppingCartContext
    {
        private IPaymentStrategy? _strategy;
        public void SetStrategy(IPaymentStrategy strategy) => _strategy = strategy;
        public void Checkout(decimal amount) => _strategy?.Pay(amount);
    }
}
```

```csharp
// Program.cs
using Strategy;

var cart = new ShoppingCartContext();
cart.SetStrategy(new CreditCardPaymentStrategy("4111111111111234"));
cart.Checkout(149.99m);

cart.SetStrategy(new PayPalPaymentStrategy("user@example.com"));
cart.Checkout(49.00m);
```

---

### Command

> **Intent**: Encapsulate a request as an object, thereby letting you parameterize clients with different requests, queue or log requests, and support undoable operations.

#### 🌍 Real-Life Analogy: Waiter Taking an Order in a Restaurant
In a restaurant, the customer tells the waiter what they want. The waiter doesn't shout immediately into the kitchen or cook the meal. Instead, the waiter writes the request onto an **order ticket** (Command object). The ticket contains everything needed: the table number, items, and modifications. The ticket is queued on a order rack in the kitchen, can be reassigned to another chef, saved for the cashier, or torn up if the customer cancels the order before cooking.

#### 💻 Software Engineering Scenario
- **Undo / Redo Support in Applications**: Wrapping modifications (such as adding an employee to a manager list) into command objects with both `Execute()` and `Undo()` methods.
- **Job Queues & Task Scheduling**: Serializing background job requests to RabbitMQ or Azure Service Bus to be processed asynchronously by worker services.

#### ❓ The Problem It Solves
- **Without Command**: Direct coupling between UI buttons and business logic; no clean mechanism to queue commands or reverse executed operations.
- **With Command**: Commands can be stored in a stack for multi-level undo, delayed execution, or remote queuing.

```csharp
// Implementation.cs
namespace Command
{
    public interface ICommand
    {
        void Execute();
        bool CanExecute();
        void Undo();
    }

    public class AddEmployeeToManagerList : ICommand
    {
        private readonly IEmployeeManagerRepository _repo;
        private readonly int _managerId;
        private readonly Employee _employee;

        public AddEmployeeToManagerList(IEmployeeManagerRepository repo, int managerId, Employee employee)
        { _repo = repo; _managerId = managerId; _employee = employee; }

        public bool CanExecute() => !_repo.HasEmployee(_managerId, _employee.Id);
        public void Execute() => _repo.AddEmployee(_managerId, _employee);
        public void Undo() => _repo.RemoveEmployee(_managerId, _employee);
    }

    public class CommandManager
    {
        private readonly Stack<ICommand> _commands = new();

        public void Invoke(ICommand command)
        {
            if (command.CanExecute())
            {
                command.Execute();
                _commands.Push(command);
            }
        }

        public void Undo() { if (_commands.Count > 0) _commands.Pop()?.Undo(); }
    }
}
```

```csharp
// Program.cs
using Command;

var manager = new CommandManager();
var repo = new EmployeeManagerRepository();

manager.Invoke(new AddEmployeeToManagerList(repo, 1, new Employee(111, "Kevin")));
repo.WriteDataStore();

manager.Undo(); // Automatically reverts the addition!
repo.WriteDataStore();
```

---

### Momento

> **Intent**: Without violating encapsulation, capture and externalize an object's internal state so that the object can be restored to this state later.

#### 🌍 Real-Life Analogy: Video Game Checkpoint / Save State
Before facing a difficult boss in a video game, your character reaches a checkpoint bonfire. The game saves a complete snapshot of your health, inventory, and location (Memento). If you perish in battle, the game restores your exact state from that checkpoint without exposing the internal game engine mechanics to the outside world.

#### 💻 Software Engineering Scenario
- **Text Editor History**: A text editor snapshotting document content before major user edits. If the user presses `Ctrl+Z`, the editor restores the previous snapshot from the caretaker history stack.
- **Form Wizard Drafts**: Saving intermediate stages of a multi-step user registration wizard so users can cancel back to a clean state.

#### ❓ The Problem It Solves
- **Without Memento**: Attempting to save state requires making all private fields public, breaking encapsulation and coupling the history manager to internal details.
- **With Memento**: The originator produces an opaque memento that only it can read and restore, preserving strict encapsulation.

```csharp
// Implementation.cs
namespace Momento
{
    public class TextEditorMemento
    {
        public string Content { get; }
        public DateTime SavedAt { get; }
        public TextEditorMemento(string content) { Content = content; SavedAt = DateTime.UtcNow; }
    }

    public class TextEditor
    {
        public string Content { get; private set; } = string.Empty;

        public void Type(string words) =>
            Content = string.IsNullOrEmpty(Content) ? words : $"{Content} {words}";

        public TextEditorMemento Save() => new(Content);
        public void Restore(TextEditorMemento memento) => Content = memento.Content;
    }

    public class EditorHistory
    {
        private readonly Stack<TextEditorMemento> _history = new();
        public void Push(TextEditorMemento memento) => _history.Push(memento);
        public TextEditorMemento? Pop() => _history.Count > 0 ? _history.Pop() : null;
    }
}
```

```csharp
// Program.cs
using Momento;

var editor = new TextEditor();
var history = new EditorHistory();

editor.Type("Hello");
history.Push(editor.Save());

editor.Type("world!");
Console.WriteLine(editor.Content); // "Hello world!"

var snapshot = history.Pop();
if (snapshot != null) editor.Restore(snapshot);
Console.WriteLine(editor.Content); // Restored back to "Hello"
```

---

### Mediator

> **Intent**: Define an object that encapsulates how a set of objects interact. Mediator promotes loose coupling by keeping objects from referring to each other explicitly.

#### 🌍 Real-Life Analogy: Air Traffic Control (ATC) Tower
At an international airport, if 50 airplanes arriving and departing all had to communicate directly with one another over radio to negotiate runway usage, the sky would be chaos ($N \times (N-1)$ connections). Instead, pilots talk only to the **Air Traffic Control Tower** (Mediator). The tower coordinates all flight paths, takeoffs, and landings centrally.

#### 💻 Software Engineering Scenario
- **Team Collaboration Chat Room**: Team members (`DeveloperUser`, `TesterUser`) send messages to a central `TeamChatRoom` mediator, which broadcasts to other members without individuals keeping references to every peer.
- **UI Dialog Boxes**: A dialog form mediator where changing a checkbox automatically enables or disables multiple textboxes and submit buttons.

#### ❓ The Problem It Solves
- **Without Mediator**: Components are tightly coupled in a tangled web of dependencies; changing one component breaks multiple others.
- **With Mediator**: Components communicate only with the mediator, centralizing complex communication rules and making colleagues reusable.

```csharp
// Implementation.cs
namespace Mediator
{
    public interface IChatRoomMediator
    {
        void SendMessage(string message, User sender);
        void RegisterUser(User user);
    }

    public abstract class User
    {
        protected readonly IChatRoomMediator _mediator;
        public string Name { get; }
        protected User(IChatRoomMediator mediator, string name) { _mediator = mediator; Name = name; }
        public void Send(string message) => _mediator.SendMessage(message, this);
        public abstract void Receive(string message, string senderName);
    }

    public class DeveloperUser : User
    {
        public DeveloperUser(IChatRoomMediator m, string name) : base(m, name) { }
        public override void Receive(string msg, string from) =>
            Console.WriteLine($"Developer {Name} received from {from}: \"{msg}\"");
    }

    public class TeamChatRoom : IChatRoomMediator
    {
        private readonly List<User> _users = [];
        public void RegisterUser(User user) => _users.Add(user);
        public void SendMessage(string message, User sender)
        {
            foreach (var u in _users) if (u != sender) u.Receive(message, sender.Name);
        }
    }
}
```

```csharp
// Program.cs
using Mediator;

var chat = new TeamChatRoom();
var alice = new DeveloperUser(chat, "Alice");
var bob = new DeveloperUser(chat, "Bob");

chat.RegisterUser(alice);
chat.RegisterUser(bob);

alice.Send("Build deployed to staging!");
```

---

### Chain of Responsibility

> **Intent**: Avoid coupling the sender of a request to its receiver by giving more than one object a chance to handle the request. Chain the receiving objects and pass the request along the chain until an object handles it.

#### 🌍 Real-Life Analogy: Corporate Expense Approvals
If an employee requests \$400 for office stationery, the **Team Lead** can approve it directly. If the request is \$3,000 for a new laptop, the Team Lead's \$1,000 limit is exceeded, so the request escalates to the **Department Manager**. If it's \$15,000 for cloud servers, it escalates to the **Vice President**. Each authority either approves the request or forwards it to the next link in the chain.

#### 💻 Software Engineering Scenario
- **ASP.NET Core Middleware Pipeline**: HTTP requests flow through a chain of middleware components (`Authentication` $\rightarrow$ `Authorization` $\rightarrow$ `Cors` $\rightarrow$ `Routing`). Each middleware can either handle/short-circuit the request or pass it to `next()`.
- **Support Ticket Escalation**: First-line bot $\rightarrow$ Tier 1 Agent $\rightarrow$ Tier 2 Senior Engineer.

#### ❓ The Problem It Solves
- **Without Chain of Responsibility**: The sender must know which specific person or service handles each condition, creating brittle nested `if` statements.
- **With Chain of Responsibility**: The sender dispatches the request to the head of the chain; handlers can be dynamically added, removed, or reordered.

```csharp
// Implementation.cs
namespace ChainOfResponsibility
{
    public record PurchaseRequest(int Id, string Purpose, decimal Amount);

    public interface IApprover
    {
        IApprover SetNext(IApprover next);
        void Process(PurchaseRequest request);
    }

    public abstract class ApproverBase : IApprover
    {
        private IApprover? _next;
        public IApprover SetNext(IApprover next) { _next = next; return next; }
        public virtual void Process(PurchaseRequest req) => _next?.Process(req);
    }

    public class TeamLeadApprover : ApproverBase
    {
        public override void Process(PurchaseRequest req)
        {
            if (req.Amount <= 1000)
                Console.WriteLine($"[Team Lead] Approved #{req.Id} (${req.Amount})");
            else
                base.Process(req);
        }
    }

    public class ManagerApprover : ApproverBase
    {
        public override void Process(PurchaseRequest req)
        {
            if (req.Amount <= 5000)
                Console.WriteLine($"[Manager] Approved #{req.Id} (${req.Amount})");
            else
                base.Process(req);
        }
    }
}
```

```csharp
// Program.cs
using ChainOfResponsibility;

var teamLead = new TeamLeadApprover();
var manager = new ManagerApprover();
teamLead.SetNext(manager);

teamLead.Process(new PurchaseRequest(1, "Supplies", 450));    // Handled by Team Lead
teamLead.Process(new PurchaseRequest(2, "Hardware", 3200));   // Escalated to Manager
```

---

### Observer

> **Intent**: Define a one-to-many dependency between objects so that when one object changes state, all its dependents are notified and updated automatically.

#### 🌍 Real-Life Analogy: YouTube Channel Subscription & Bell Icon
Instead of checking a creator's channel every hour to see if a new video was uploaded, you click the "Subscribe" button. When the creator publishes a video (State Change), YouTube automatically broadcasts a notification to all 500,000 subscribers simultaneously. Subscribers can unsubscribe anytime without affecting the creator.

#### 💻 Software Engineering Scenario
- **Event-Driven Ticket / Inventory Systems**: When a ticket is purchased via `OrderService`, multiple subscriber services (`TicketStockService`, `TicketResellerService`) are automatically notified of the remaining quantity.
- **UI Event Handling**: Event listeners and data-binding in GUI and web frameworks (WPF `INotifyPropertyChanged`, Rx.NET observables).

#### ❓ The Problem It Solves
- **Without Observer**: The publisher must directly know and call every dependent service, creating tight coupling and making it impossible to add new listeners without editing the publisher.
- **With Observer**: Loose coupling between publisher (Subject) and subscribers (Observers). Observers can attach or detach dynamically at runtime.

```csharp
// Implementation.cs
namespace Observer
{
    public record Ticket(int MovieId, int Amount);

    public interface ITicketChangeListener
    {
        void OnTicketChange(Ticket ticket);
    }

    public abstract class TicketChangeNotifier
    {
        private readonly List<ITicketChangeListener> _observers = [];
        public void AddObserver(ITicketChangeListener obs) => _observers.Add(obs);
        public void RemoveObserver(ITicketChangeListener obs) => _observers.Remove(obs);
        public void NotifyObservers(Ticket ticket)
        {
            foreach (var obs in _observers) obs.OnTicketChange(ticket);
        }
    }

    public class OrderService : TicketChangeNotifier
    {
        public void PlaceOrder(int movieId, int amount)
        {
            var ticket = new Ticket(movieId, amount);
            NotifyObservers(ticket);
        }
    }

    public class TicketStockService : ITicketChangeListener
    {
        public void OnTicketChange(Ticket ticket) =>
            Console.WriteLine($"Stock updated for movie {ticket.MovieId}: {ticket.Amount}");
    }
}
```

```csharp
// Program.cs
using Observer;

var orderService = new OrderService();
var stockService = new TicketStockService();

orderService.AddObserver(stockService);
orderService.PlaceOrder(101, 5); // Automatically triggers stockService
```

---

### State

> **Intent**: Allow an object to alter its behavior when its internal state changes. The object will appear to change its class.

#### 🌍 Real-Life Analogy: Smartphone Screen States
A smartphone behaves completely differently to a tap depending on its current state:
- When **Locked**, tapping wakes the screen to show the lock clock.
- When **Unlocked**, tapping launches an app.
- When on an **Active Phone Call**, tapping brings up audio controls and the keypad.
The physical hardware screen is unchanged, but the response to user actions completely changes based on the internal state.

#### 💻 Software Engineering Scenario
- **Order Lifecycle Fulfillment**: An e-commerce order transitions through states: `PendingPayment` $\rightarrow$ `Paid` $\rightarrow$ `Shipped` $\rightarrow$ `Delivered` (or `Cancelled`). You cannot ship an order if it is in `PendingPayment`, and you cannot cancel an order once it is `Delivered`.

#### ❓ The Problem It Solves
- **Without State**: The `Order` class is full of nested condition checks: `if (state == Shipped) ... else if (state == Paid) ...`. Adding a new state requires editing every method in the class.
- **With State**: Each state is encapsulated in its own class implementing `OrderState`. State transitions are explicit and clean.

```csharp
// Implementation.cs
namespace State
{
    public class OrderContext
    {
        public OrderState State { get; private set; }
        public OrderContext() { State = new PendingPaymentState(); State.SetContext(this); }
        public void TransitionTo(OrderState state) { State = state; State.SetContext(this); }
        public void Pay() => State.Pay();
        public void Ship() => State.Ship();
    }

    public abstract class OrderState
    {
        protected OrderContext? _context;
        public void SetContext(OrderContext context) => _context = context;
        public abstract void Pay();
        public abstract void Ship();
    }

    public class PendingPaymentState : OrderState
    {
        public override void Pay()
        {
            Console.WriteLine("Order paid successfully.");
            _context?.TransitionTo(new PaidState());
        }
        public override void Ship() => Console.WriteLine("Cannot ship unpaid order.");
    }

    public class PaidState : OrderState
    {
        public override void Pay() => Console.WriteLine("Already paid.");
        public override void Ship() => Console.WriteLine("Order shipped.");
    }
}
```

```csharp
// Program.cs
using State;

var order = new OrderContext();
order.Ship(); // Fails: cannot ship unpaid order
order.Pay();  // State transitions to PaidState
order.Ship(); // Succeeds: Order shipped
```

---

### Iterator

> **Intent**: Provide a way to access the elements of an aggregate object sequentially without exposing its underlying representation.

#### 🌍 Real-Life Analogy: TV Remote Channel Guide / Music Playlist
Whether a music collection is stored on vinyl records, magnetic tapes, optical CDs, or a cloud streaming service, you don't need to know how the audio tracks are stored physically. You simply press "Next Track" ($\rightarrow$) or "Previous Track" ($\leftarrow$) to traverse songs sequentially.

#### 💻 Software Engineering Scenario
- **Collection Traversal**: Traversing a custom book library or binary tree without exposing internal array structures or pointer networks, matching .NET's `IEnumerator<T>` and `IEnumerable<T>`.

#### ❓ The Problem It Solves
- **Without Iterator**: Clients must know the internal storage details (e.g. accessing private array indices or linked node pointers) to loop through items.
- **With Iterator**: The traversal logic is encapsulated inside an iterator object, allowing multiple independent iterations simultaneously.

```csharp
// Implementation.cs
namespace Iterator
{
    public record Book(string Title, string Author, int Year);

    public interface IIterator<T>
    {
        bool HasNext();
        T Next();
    }

    public class BookCollection
    {
        private readonly List<Book> _books = [];
        public void Add(Book b) => _books.Add(b);
        public int Count => _books.Count;
        public Book this[int i] => _books[i];
        public IIterator<Book> CreateIterator() => new BookIterator(this);
    }

    public class BookIterator : IIterator<Book>
    {
        private readonly BookCollection _collection;
        private int _index = 0;
        public BookIterator(BookCollection collection) => _collection = collection;
        public bool HasNext() => _index < _collection.Count;
        public Book Next() => _collection[_index++];
    }
}
```

```csharp
// Program.cs
using Iterator;

var library = new BookCollection();
library.Add(new Book("Clean Code", "Robert C. Martin", 2008));
library.Add(new Book("Refactoring", "Martin Fowler", 1999));

var iterator = library.CreateIterator();
while (iterator.HasNext())
{
    var book = iterator.Next();
    Console.WriteLine($"Book: {book.Title} by {book.Author}");
}
```

---

### Visitor

> **Intent**: Represent an operation to be performed on elements of an object structure. Visitor lets you define a new operation without changing the classes of the elements on which it operates.

#### 🌍 Real-Life Analogy: City Building Code Inspector
A municipal building inspector visits a house containing plumbing pipes, electrical wiring, and roofing. The pipes, wiring, and roof don't need to know city laws; they simply `Accept(Inspector)`. The inspector enters and executes `InspectPlumbing(pipes)`, `InspectElectrical(wiring)`, and `InspectRoofing(roof)`. Tomorrow, an insurance appraiser can visit the exact same house and calculate financial replacement values without altering a single wall or wire.

#### 💻 Software Engineering Scenario
- **Shopping Cart Pricing Operations**: A shopping cart contains heterogeneous items (`BookItem`, `ElectronicItem`, `GroceryItem`). Applying a `TaxVisitor` calculates varying luxury taxes (18% on electronics, 0% on books). Applying a `DiscountVisitor` applies rebates and bulk food discounts without polluting product entities with tax math.

#### ❓ The Problem It Solves
- **Without Visitor**: Every time tax rules or export operations change, you have to modify and retest every single product entity class.
- **With Visitor**: Operations are decoupled from object structures; adding a new operation is as simple as creating a new `IVisitor` implementation.

```csharp
// Implementation.cs
namespace Visitor
{
    public interface IVisitor
    {
        void VisitBook(BookItem book);
        void VisitElectronic(ElectronicItem electronic);
    }

    public interface IShoppingItem
    {
        void Accept(IVisitor visitor);
    }

    public class BookItem : IShoppingItem
    {
        public string Title { get; }
        public decimal Price { get; }
        public BookItem(string title, decimal price) { Title = title; Price = price; }
        public void Accept(IVisitor visitor) => visitor.VisitBook(this);
    }

    public class ElectronicItem : IShoppingItem
    {
        public string Brand { get; }
        public decimal Price { get; }
        public ElectronicItem(string brand, decimal price) { Brand = brand; Price = price; }
        public void Accept(IVisitor visitor) => visitor.VisitElectronic(this);
    }

    public class TaxVisitor : IVisitor
    {
        public decimal TotalTax { get; private set; }
        public void VisitBook(BookItem book) => Console.WriteLine($"Book {book.Title} is tax-exempt.");
        public void VisitElectronic(ElectronicItem item)
        {
            var tax = item.Price * 0.18m;
            TotalTax += tax;
            Console.WriteLine($"Electronic {item.Brand} Tax: ${tax:F2}");
        }
    }
}
```

```csharp
// Program.cs
using Visitor;

List<IShoppingItem> cart = [new BookItem("Clean Architecture", 35m), new ElectronicItem("Headphones", 150m)];
var taxVisitor = new TaxVisitor();

foreach (var item in cart) item.Accept(taxVisitor);
Console.WriteLine($"Total Tax: ${taxVisitor.TotalTax:F2}");
```

---

### Interpreter

> **Intent**: Given a language, define a representation for its grammar along with an interpreter that uses the representation to interpret sentences in the language.

#### 🌍 Real-Life Analogy: Musical Sheet Notation
A composer writes a musical score: quarter note C, half note G, rest, chord. The notes on the staff represent a formal grammar. A pianist interprets the notes and presses piano keys; a flutist interprets the same sheet and blows into a flute. Both interpret the symbols according to the rules of musical grammar.

#### 💻 Software Engineering Scenario
- **Mathematical Expression Trees**: Evaluating arithmetic expressions with variables and constants, such as `(a + b) * (c - 5)`.
- **Search Query & Filter Parsers**: Parsing user filter expressions in search engines (`author:John AND year > 2020`).

#### ❓ The Problem It Solves
- **Without Interpreter**: Parsing expressions requires hard-coded string manipulation and rigid regexes that break easily when syntax expands.
- **With Interpreter**: Grammar rules are represented as an Abstract Syntax Tree (AST) where terminal and non-terminal expressions are clean, recursive classes.

```csharp
// Implementation.cs
namespace Interpreter
{
    public class ExpressionContext
    {
        private readonly Dictionary<string, int> _vars = [];
        public void Set(string name, int val) => _vars[name] = val;
        public int Get(string name) => _vars[name];
    }

    public interface IExpression
    {
        int Interpret(ExpressionContext context);
    }

    public class VariableExpression : IExpression
    {
        private readonly string _name;
        public VariableExpression(string name) => _name = name;
        public int Interpret(ExpressionContext ctx) => ctx.Get(_name);
    }

    public class AddExpression : IExpression
    {
        private readonly IExpression _left, _right;
        public AddExpression(IExpression left, IExpression right) { _left = left; _right = right; }
        public int Interpret(ExpressionContext ctx) => _left.Interpret(ctx) + _right.Interpret(ctx);
    }
}
```

```csharp
// Program.cs
using Interpreter;

var ctx = new ExpressionContext();
ctx.Set("x", 10);
ctx.Set("y", 25);

IExpression expr = new AddExpression(new VariableExpression("x"), new VariableExpression("y"));
Console.WriteLine($"Result: {expr.Interpret(ctx)}"); // 35
```

---

### Repository

> **Intent**: Mediate between the domain and data mapping layers using a collection-like interface for accessing domain objects.

#### 🌍 Real-Life Analogy: Public Library Front Desk
When you want a book from a large library, you don't search through underground book stacks, decipher shelf cataloging systems, or manually unlock warehouse rooms. You walk up to the front desk librarian and ask for the book by title. The librarian handles fetching, updating, or returning the book. If the library reorganizes its basement shelves or switches to an automated storage robot, your experience at the front desk remains unchanged.

#### 💻 Software Engineering Scenario
- **Data Access Abstraction**: Encapsulating database queries (Entity Framework Core, SQL/Dapper, In-Memory) behind clean methods like `GetById(id)`, `GetAll()`, `Add(entity)`, and `Delete(id)`. This allows services to be unit-tested using fast in-memory mock repositories without spinning up a real database.

#### ❓ The Problem It Solves
- **Without Repository**: Raw SQL queries and database contexts are scattered throughout UI controllers and business services, making code difficult to maintain and impossible to test in isolation.
- **With Repository**: Business logic is completely decoupled from database mechanics.

```csharp
// Implementation.cs
namespace Repository
{
    public record Product(int Id, string Name, decimal Price, int Stock);

    public interface IProductRepository
    {
        Product? GetById(int id);
        IEnumerable<Product> GetAll();
        void Add(Product entity);
        void Update(Product entity);
        bool Delete(int id);
    }

    public class InMemoryProductRepository : IProductRepository
    {
        private readonly List<Product> _products = [];
        public Product? GetById(int id) => _products.FirstOrDefault(p => p.Id == id);
        public IEnumerable<Product> GetAll() => _products.AsReadOnly();
        public void Add(Product p) => _products.Add(p);
        public void Update(Product p)
        {
            var idx = _products.FindIndex(x => x.Id == p.Id);
            if (idx >= 0) _products[idx] = p;
        }
        public bool Delete(int id) => _products.RemoveAll(x => x.Id == id) > 0;
    }
}
```

```csharp
// Program.cs
using Repository;

IProductRepository repo = new InMemoryProductRepository();
repo.Add(new Product(1, "Standing Desk", 499m, 5));
Console.WriteLine($"Found: {repo.GetById(1)?.Name}");
```

---

### Unit of Work

> **Intent**: Maintain a list of objects affected by a business transaction and coordinate the writing out of changes and the resolution of concurrency problems.

#### 🌍 Real-Life Analogy: Bank Wire Transfer
Imagine transferring \$500 from Account A to Account B. If the bank deducts \$500 from Account A, and the power cuts out before Account B is credited, \$500 vanishes! To prevent disaster, banks use an atomic transaction: both operations are bundled together. Either both complete successfully (**Commit**), or if any error occurs, all steps are undone (**Rollback**).

#### 💻 Software Engineering Scenario
- **Multi-Repository Transaction Coordination**: A business action creates a new `Customer` and two initial `Order` records. The `UnitOfWork` coordinates `CustomerRepository` and `OrderRepository`, ensuring all records are saved in a single database transaction. If saving the orders fails, customer creation is rolled back.

#### ❓ The Problem It Solves
- **Without Unit of Work**: Each repository calls its own save method independently. If step 2 fails, step 1 has already been committed, leaving the database in an inconsistent, corrupted state.
- **With Unit of Work**: Multiple repository mutations are tracked and committed as a single atomic unit.

```csharp
// Implementation.cs
namespace UnitOfWork
{
    public record Customer(int Id, string Name, string Email);
    public record Order(int Id, int CustomerId, decimal Amount, DateTime CreatedAt);

    public interface IUnitOfWork : IDisposable
    {
        ICustomerRepository Customers { get; }
        IOrderRepository Orders { get; }
        bool Commit();
        void Rollback();
    }

    public class InMemoryUnitOfWork : IUnitOfWork
    {
        private readonly List<Customer> _dbCustomers = [];
        private readonly List<Order> _dbOrders = [];
        private readonly List<Customer> _pendingCustomers = [];
        private readonly List<Order> _pendingOrders = [];

        public ICustomerRepository Customers { get; }
        public IOrderRepository Orders { get; }

        public InMemoryUnitOfWork()
        {
            Customers = new CustomerRepository(this);
            Orders = new OrderRepository(this);
        }

        public bool Commit()
        {
            _dbCustomers.AddRange(_pendingCustomers);
            _dbOrders.AddRange(_pendingOrders);
            _pendingCustomers.Clear();
            _pendingOrders.Clear();
            Console.WriteLine("Transaction committed successfully.");
            return true;
        }

        public void Rollback()
        {
            _pendingCustomers.Clear();
            _pendingOrders.Clear();
            Console.WriteLine("Transaction rolled back.");
        }

        public void Dispose() => Rollback();
    }
}
```

```csharp
// Program.cs
using UnitOfWork;

using (var uow = new InMemoryUnitOfWork())
{
    uow.Customers.Add(new Customer(1, "David", "david@example.com"));
    uow.Orders.Add(new Order(101, 1, 150m, DateTime.UtcNow));
    uow.Commit(); // Atomically saves customer and order
}
```

---

## 👤 Author

**Md. Abul Kalam**
- GitHub: [@akazad13](https://github.com/akazad13)
- LinkedIn: [Md. Abul Kalam](https://linkedin.com/in/akazad13)

---

## 📄 License

This project is licensed under the [MIT License](LICENSE).

