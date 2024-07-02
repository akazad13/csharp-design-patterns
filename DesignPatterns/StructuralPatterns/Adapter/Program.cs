using ObjectAdapter;

Console.Title = "Adapter Pattern";

// Object adapter example
ICityAdapter adapter = new CityAdapter();
var city = adapter.GetCity();

Console.WriteLine($"City: {city.FullName}, Population: {city.Inhabitants}");


// Class adapter example

// using ClassAdapter;

// ICityAdapter adapter = new CityAdapter();
// var city = adapter.GetCity();

// Console.WriteLine($"City: {city.FullName}, Population: {city.Inhabitants}");
