using Builder;

Console.Title = "Builder Pattern";

var builder = new CarBuilder();
var director = new CarDirector(builder);

// 1. Build using Director
var sportsCar = director.ConstructSportsCar();
Console.WriteLine($"Sports Car built by Director: {sportsCar}");

var familySuv = director.ConstructFamilySuv();
Console.WriteLine($"Family SUV built by Director: {familySuv}");

// 2. Custom Build using Builder directly
var customCar = builder
    .SetMake("Tesla")
    .SetModel("Model S Plaid")
    .SetNumDoors(4)
    .SetEngine("Tri-Motor Electric")
    .SetGps(true)
    .SetSunroof(true)
    .Build();

Console.WriteLine($"Custom Car built directly: {customCar}");
