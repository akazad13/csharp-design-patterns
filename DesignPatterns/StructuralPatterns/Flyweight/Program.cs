using Flyweight;

Console.Title = "Flyweight Pattern";

var forest = new Forest();

// Plant many trees sharing a few types
Console.WriteLine("--- Planting Trees ---");
forest.PlantTree(10, 20, "Oak", "Dark Green", "Rough Bark");
forest.PlantTree(15, 25, "Oak", "Dark Green", "Rough Bark");
forest.PlantTree(100, 200, "Oak", "Dark Green", "Rough Bark");

forest.PlantTree(30, 40, "Pine", "Bright Green", "Smooth Needle");
forest.PlantTree(35, 45, "Pine", "Bright Green", "Smooth Needle");

forest.PlantTree(50, 60, "Birch", "White/Yellow", "Papery Bark");

Console.WriteLine($"\nTotal trees planted in forest: {forest.TotalTrees}");
Console.WriteLine($"Total distinct Flyweight objects in memory: {TreeFactory.TotalFlyweightsCreated}");

Console.WriteLine("\n--- Rendering Forest ---");
forest.Draw();
