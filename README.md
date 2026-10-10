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

---

## Creational Patterns

These patterns deal with object creation mechanisms, trying to create objects in a manner suitable to the situation:
- Abstract the object instantiation process.
- Make systems independent of how their objects are created, composed, and represented.

---

### Singleton

Singleton lets you access an object from anywhere in your application. It guarantees that only one instance of the class exists at any time.

Holding the class instance in a global variable doesn’t prevent clients from creating other instances. We make the class itself responsible for ensuring only one instance exists.

**Use Cases**
- When a class must have exactly one instance available via a well-known access point.
- When the sole instance should be extensible by subclassing without modifying client code.

**Examples**
- Managing a database connection pool.
- Caching frequently accessed data.
- Managing application configuration settings.

*Note: May violate the Single Responsibility Principle as the object creation, lifetime, and business operations are maintained within the same class.*

```csharp
// Implementation.cs (Lazy Loading & Thread-Safe)
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

Factory Method defines an interface or abstract class for creating an object, but lets subclasses decide which class to instantiate. It defers instantiation to subclasses.

**Use Cases**
- When a class cannot anticipate the type of objects it must create.
- When a class wants its subclasses to specify the objects it creates.
- When delegating responsibility to one of several helper subclasses.

**Examples**
- Restaurant ordering systems creating specific burger types.
- Payment gateway integrations supporting varying country-specific discount rules.

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

var factories = new List<DiscountFactory>
{
    new CodeDiscountFactory(Guid.NewGuid()),
    new CountryDiscountFactory("GER")
};

foreach (var factory in factories)
{
    var discountService = factory.CreateDiscountService();
    Console.WriteLine($"Percentage {discountService.DiscountPercentage} coming from {discountService}");
}
```

---

### Abstract Factory

Abstract Factory provides an interface for creating families of related or dependent objects without specifying their concrete classes.

**Use Cases**
- When a system should be independent of how its products are created, composed, and represented.
- When a system should be configured with one of multiple families of products.
- When a family of related product objects is designed to be used together, and you need to enforce this constraint.

**Examples**
- E-commerce checkout calculating shipping and discounts configured per region/country (e.g., India vs. China).
- Cross-platform UI widget kits (Windows, macOS, Linux).

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

var indiaCart = new ShoppingCart(new IndiaShoppingCartPurchaseFactory());
indiaCart.CalculateCost();

var chinaCart = new ShoppingCart(new ChinaShoppingCartPurchaseFactory());
chinaCart.CalculateCost();
```

---

### Builder

Builder separates the construction of a complex object from its representation, allowing the same construction process to create various representations.

**Use Cases**
- When the algorithm for creating a complex object should be independent of the parts that make up the object and how they're assembled.
- To avoid "telescoping constructors" with numerous optional parameters.

**Examples**
- Constructing complex domain entities (vehicles, computers, SQL query trees, HTTP requests).

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
                       .SetNumDoors(4).SetEngine("Electric").SetGps(true).Build();
Console.WriteLine($"Custom Car: {customCar}");
```

---

### Prototype

Prototype specifies the kinds of objects to create using a prototypical instance, creating new objects by copying this prototype.

**Use Cases**
- When classes to instantiate are specified at run-time.
- To avoid building a class hierarchy of factories that parallels the class hierarchy of products.
- When instances of a class can have one of only a few different combinations of state, and cloning is cheaper than manual instantiation.

**Examples**
- Cloning complex documents, graphic shapes, or game entity configurations.

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
                return new Document(Title, Content, new Author(Author.Name, Author.Email), new List<string>(Tags));
            }
            return (Document)MemberwiseClone();
        }
    }
}
```

```csharp
// Program.cs
using Prototype;

var doc = new Document("Design Patterns", "Content here", new Author("Abul Kalam", "dev@example.com"), ["C#", "OOP"]);
var copy = doc.Clone(deepClone: true);
copy.Title = "Design Patterns 2nd Ed";
copy.Author.Name = "New Author";

Console.WriteLine($"Original Author: {doc.Author.Name}");
Console.WriteLine($"Cloned Author: {copy.Author.Name}");
```

---

## Structural Patterns

Structural patterns deal with how classes and objects are composed to form larger structures while keeping these structures flexible and efficient.

---

### Adapter

Adapter converts the interface of a class into another interface clients expect. It lets classes work together that couldn't otherwise because of incompatible interfaces.

**Use Cases**
- Integrating legacy classes or third-party libraries into modern systems with incompatible signatures.
- Reusing existing subclasses lacking needed functionality via object or class adaptation.

**Examples**
- Adapting third-party legacy city data API to the internal domain model.

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

Bridge decouples an abstraction from its implementation so that the two can vary independently.

**Use Cases**
- When you want to avoid a permanent binding between an abstraction and its implementation.
- When both abstractions and implementations should be extensible through subclassing.

**Examples**
- Remote controls (basic, advanced) interacting with various electronic devices (TV, Radio).
- Cross-platform rendering engines (DirectX, OpenGL, Vulkan).

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

Decorator attaches additional responsibilities to an object dynamically. Decorators provide a flexible alternative to subclassing for extending functionality.

**Use Cases**
- Adding responsibilities to individual objects dynamically and transparently without affecting other objects.
- When extension by subclassing is impractical (exploding class hierarchy).

**Examples**
- Adding logging, caching, statistics collection, or encryption to mail sending services.

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
var statisticsDecorator = new StatisticsDecorator(mailService);
statisticsDecorator.SendMail("Hello Decorator!");
```

---

### Composite

Composite composes objects into tree structures to represent part-whole hierarchies. It lets clients treat individual objects and compositions of objects uniformly.

**Use Cases**
- When you want to represent part-whole hierarchies of objects.
- When you want clients to be able to ignore the difference between compositions of objects and individual objects.

**Examples**
- File system directories containing files and sub-directories.
- GUI layout containers containing buttons, text boxes, and nested panels.

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

Facade provides a unified interface to a set of interfaces in a subsystem. It defines a higher-level interface that makes the subsystem easier to use.

**Use Cases**
- Providing a simple interface to a complex subsystem.
- Decoupling subsystem components from clients and other subsystems.

**Examples**
- Calculating complex customer discount percentages spanning multiple subsystems (orders, base tier, day of week).

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
Console.WriteLine($"Discount for customer 10: {facade.CalculateDiscountPercentage(10)}%");
```

---

### Proxy

Proxy provides a surrogate or placeholder for another object to control access to it.

**Use Cases**
- **Virtual Proxy**: Lazy-loading expensive resources on demand.
- **Caching Proxy**: Caching expensive query or download results.
- **Protection Proxy**: Checking authentication and permissions before accessing operations.

**Examples**
- Caching video stream responses to avoid repeated roundtrips to an external API.

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
                Console.WriteLine($"[Proxy] Returning '{videoId}' from memory cache.");
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
proxy.GetVideo("design-patterns");
proxy.GetVideo("design-patterns"); // Served from cache
```

---

### Flyweight

Flyweight uses sharing to support large numbers of fine-grained objects efficiently by separating intrinsic state (shared) from extrinsic state (context-dependent).

**Use Cases**
- When an application uses a huge number of similar objects that consume high memory.
- When most of each object's state can be made extrinsic and shared.

**Examples**
- Rendering forests of millions of trees sharing graphic textures, colors, and species models.
- Word processor character glyph formatting.

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

var type1 = TreeFactory.GetTreeType("Oak", "Green", "Rough");
var type2 = TreeFactory.GetTreeType("Oak", "Green", "Rough"); // Reused!

var treeA = new Tree(10, 20, type1);
var treeB = new Tree(30, 40, type2);
treeA.Draw();
treeB.Draw();

Console.WriteLine($"Total Flyweight objects: {TreeFactory.TotalFlyweights}"); // 1
```

---

## Behavioral Patterns

Behavioral patterns characterize the ways in which classes or objects interact and distribute responsibility.

---

### Template

Template Method defines the skeleton of an algorithm in an operation, deferring some steps to subclasses. Subclasses can redefine certain steps of an algorithm without changing the algorithm's structure.

**Use Cases**
- Implementing invariant parts of an algorithm once and leaving variant behavior to subclasses.
- Controlling subclass extensions through hook operations.

**Examples**
- Data mining workflows (opening files, parsing records, analyzing, closing handles).
- Build and deployment lifecycle pipelines.

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
miner.Mine("data.csv");
```

---

### Strategy

Strategy defines a family of algorithms, encapsulates each one, and makes them interchangeable. Strategy lets the algorithm vary independently from clients that use it.

**Use Cases**
- When many related classes differ only in their behavior.
- When an algorithm uses data that clients shouldn't know about.
- When a class defines many behaviors that appear as multiple conditional statements.

**Examples**
- E-commerce payment methods (Credit Card, PayPal, Cryptocurrency).
- File compression algorithms (Zip, Rar, Gzip).

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

Command encapsulates a request as an object, thereby letting you parameterize clients with different requests, queue or log requests, and support undoable operations.

**Use Cases**
- Parameterizing objects by an action to perform.
- Supporting undo, redo, and operation histories.
- Structuring a system around high-level operations built on primitive operations.

**Examples**
- Adding/removing employees from a repository with undo support.
- GUI menus and macro recording.

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

manager.Undo();
repo.WriteDataStore();
```

---

### Momento

Memento captures and externalizes an object's internal state without violating encapsulation, allowing the object to be restored to this state later.

**Use Cases**
- A snapshot of an object's state must be saved so it can be restored to that state later.
- A direct interface to obtaining the state would expose implementation details and break encapsulation.

**Examples**
- Text editor undo/redo history.
- Game save state / checkpoint systems.

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
Console.WriteLine(editor.Content); // "Hello"
```

---

### Mediator

Mediator defines an object that encapsulates how a set of objects interact. It promotes loose coupling by keeping objects from referring to each other explicitly.

**Use Cases**
- A set of objects communicate in well-defined but complex ways, resulting in interdependencies that are hard to understand.
- Customizing a behavior distributed among several objects should be done without extensive subclassing.

**Examples**
- Team chat room coordinating communication between developers and QA testers.
- Dialog box controller coordinating interdependent form controls.

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

alice.Send("Build ready for testing!");
```

---

### Chain of Responsibility

Chain of Responsibility avoids coupling the sender of a request to its receiver by giving more than one object a chance to handle the request. It chains the receiving objects and passes the request along the chain until an object handles it.

**Use Cases**
- More than one object may handle a request, and the handler isn't known a priori.
- You want to issue a request to one of several objects without specifying the receiver explicitly.

**Examples**
- Expense / purchase authorization escalation (Team Lead -> Manager -> Vice President).
- HTTP request middleware pipelines.

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

teamLead.Process(new PurchaseRequest(1, "Supplies", 450));
teamLead.Process(new PurchaseRequest(2, "Server Hardware", 3200));
```

---

### Observer

Observer defines a one-to-many dependency between objects so that when one object changes state, all its dependents are notified and updated automatically.

**Use Cases**
- An abstraction has two aspects, one dependent on the other.
- A change to one object requires changing others, and you don't know how many objects need to be changed.

**Examples**
- Event-driven stock update or ticket purchase notification system broadcasting to reseller and inventory services.

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
orderService.PlaceOrder(101, 5);
```

---

### State

State allows an object to alter its behavior when its internal state changes. The object will appear to change its class.

**Use Cases**
- An object's behavior depends on its state, and it must change its behavior at run-time depending on that state.
- Operations have large, multipart conditional statements that depend on the object's state.

**Examples**
- E-commerce order lifecycle (Pending Payment -> Paid -> Shipped -> Delivered / Cancelled).
- Document publishing workflows (Draft -> Moderation -> Published).

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
order.Pay();  // Transition to PaidState
order.Ship(); // Succeeds!
```

---

### Iterator

Iterator provides a way to access the elements of an aggregate object sequentially without exposing its underlying representation.

**Use Cases**
- Accessing an aggregate object's contents without exposing its internal structure.
- Supporting multiple simultaneous traversals of aggregate objects.
- Providing a uniform interface for traversing different aggregate structures.

**Examples**
- Navigating books in a custom collection or catalog.

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

Visitor represents an operation to be performed on elements of an object structure. It lets you define a new operation without changing the classes of the elements on which it operates.

**Use Cases**
- An object structure contains many classes of objects with differing interfaces, and you want to perform operations on these objects that depend on their concrete classes.
- Distinct and unrelated operations need to be performed on objects in an object structure, and you want to avoid "polluting" their classes with these operations.

**Examples**
- Applying varying tax calculations or promotional discounts across heterogeneous shopping items (Books, Electronics, Groceries).

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

Interpreter defines a representation for a language's grammar along with an interpreter that uses the representation to interpret sentences in the language.

**Use Cases**
- When there is a language to interpret, and you can represent statements in the language as abstract syntax trees.
- Mathematical expression evaluation, search query parsing, or rule engines.

**Examples**
- Evaluating arithmetic expressions containing variables (`(a + b) * (c - 5)`).

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

    public class NumberExpression : IExpression
    {
        private readonly int _val;
        public NumberExpression(int val) => _val = val;
        public int Interpret(ExpressionContext ctx) => _val;
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

Repository mediates between the domain and data mapping layers using a collection-like interface for accessing domain objects.

**Use Cases**
- Decoupling business logic from data access technologies (Entity Framework, Dapper, In-Memory, Document DBs).
- Facilitating unit testing and mocking of data access routines.

**Examples**
- In-memory and SQL product catalogs querying active inventory.

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

Unit of Work maintains a list of objects affected by a business transaction and coordinates the writing out of changes and the resolution of concurrency problems.

**Use Cases**
- Bundling multiple database operations across repositories into a single atomic transaction.
- Preventing partial database updates if one operation in a multi-step business process fails.

**Examples**
- Committing customer creation and initial orders atomically or rolling back on error.

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

        // Repository nested implementations omitted for brevity
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
    uow.Commit();
}
```
