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
