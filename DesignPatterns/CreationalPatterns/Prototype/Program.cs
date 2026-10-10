using Prototype;

Console.Title = "Prototype Pattern";

var original = new Document(
    "Design Patterns Handbook",
    "Comprehensive guide to Software Architecture Patterns in C#.",
    new Author("Md. Abul Kalam", "akazad@example.com"),
    ["C#", "OOP", "Architecture"]
);

Console.WriteLine("Original Document:");
Console.WriteLine(original);

// Deep Clone
var deepCloned = original.Clone(deepClone: true);
deepCloned.Title = "Design Patterns Handbook - 2nd Edition";
deepCloned.Author.Name = "John Doe"; // Mutating cloned author does not affect original!
deepCloned.Tags.Add("Gang of Four");

Console.WriteLine("\nAfter modifying deep clone:");
Console.WriteLine($"Original Document Author: {original.Author.Name}");
Console.WriteLine($"Cloned Document: {deepCloned}");

// Shallow Clone
var shallowCloned = original.Clone(deepClone: false);
shallowCloned.Author.Name = "Shared Reference Author"; // Mutating affects original because reference is shared!
Console.WriteLine("\nAfter modifying shallow clone's author:");
Console.WriteLine($"Original Document Author: {original.Author.Name} (modified!)");
