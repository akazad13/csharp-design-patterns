Console.Title = "Adapter Pattern";

// Object adapter example
Console.WriteLine("--- Object Adapter ---");
ObjectAdapter.ICityAdapter objectAdapter = new ObjectAdapter.CityAdapter();
var cityFromObjectAdapter = objectAdapter.GetCity();
Console.WriteLine($"City: {cityFromObjectAdapter.FullName}, Population: {cityFromObjectAdapter.Inhabitants}");

// Class adapter example
Console.WriteLine("\n--- Class Adapter ---");
ClassAdapter.ICityAdapter classAdapter = new ClassAdapter.CityAdapter();
var cityFromClassAdapter = classAdapter.GetCity();
Console.WriteLine($"City: {cityFromClassAdapter.FullName}, Population: {cityFromClassAdapter.Inhabitants}");
