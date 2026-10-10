namespace Builder
{
    /// <summary>
    /// Product
    /// </summary>
    public class Car
    {
        public string Make { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int NumDoors { get; set; }
        public string Engine { get; set; } = string.Empty;
        public bool HasGps { get; set; }
        public bool HasSunroof { get; set; }

        public override string ToString()
        {
            return $"{Make} {Model} [Doors: {NumDoors}, Engine: {Engine}, GPS: {HasGps}, Sunroof: {HasSunroof}]";
        }
    }

    /// <summary>
    /// Builder interface
    /// </summary>
    public interface ICarBuilder
    {
        ICarBuilder SetMake(string make);
        ICarBuilder SetModel(string model);
        ICarBuilder SetNumDoors(int numDoors);
        ICarBuilder SetEngine(string engine);
        ICarBuilder SetGps(bool hasGps);
        ICarBuilder SetSunroof(bool hasSunroof);
        Car Build();
        void Reset();
    }

    /// <summary>
    /// Concrete Builder
    /// </summary>
    public class CarBuilder : ICarBuilder
    {
        private Car _car = new();

        public void Reset()
        {
            _car = new Car();
        }

        public ICarBuilder SetMake(string make)
        {
            _car.Make = make;
            return this;
        }

        public ICarBuilder SetModel(string model)
        {
            _car.Model = model;
            return this;
        }

        public ICarBuilder SetNumDoors(int numDoors)
        {
            _car.NumDoors = numDoors;
            return this;
        }

        public ICarBuilder SetEngine(string engine)
        {
            _car.Engine = engine;
            return this;
        }

        public ICarBuilder SetGps(bool hasGps)
        {
            _car.HasGps = hasGps;
            return this;
        }

        public ICarBuilder SetSunroof(bool hasSunroof)
        {
            _car.HasSunroof = hasSunroof;
            return this;
        }

        public Car Build()
        {
            var result = _car;
            Reset();
            return result;
        }
    }

    /// <summary>
    /// Director
    /// </summary>
    public class CarDirector
    {
        private readonly ICarBuilder _builder;

        public CarDirector(ICarBuilder builder)
        {
            _builder = builder;
        }

        public Car ConstructSportsCar()
        {
            return _builder
                .SetMake("Ferrari")
                .SetModel("488 GTB")
                .SetNumDoors(2)
                .SetEngine("V8 Twin Turbo")
                .SetGps(true)
                .SetSunroof(false)
                .Build();
        }

        public Car ConstructFamilySuv()
        {
            return _builder
                .SetMake("Toyota")
                .SetModel("RAV4")
                .SetNumDoors(4)
                .SetEngine("2.5L Hybrid")
                .SetGps(true)
                .SetSunroof(true)
                .Build();
        }
    }
}
