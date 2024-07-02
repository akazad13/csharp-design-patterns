namespace ClassAdapter
{
    /// <summary>
    /// External System
    /// </summary>
    public class CityFromExternalSystem
    {
        public string Name { get; set; }
        public string KnownAs { get; set; }
        public int Inhabitants { get; set; }

        public CityFromExternalSystem(string name, string knownAs, int inhabitants)
        {
            Name = name;
            KnownAs = knownAs;
            Inhabitants = inhabitants;
        }
    }

    /// <summary>
    /// Adaptee
    /// </summary>
    public class ExternalSystem
    {
        public CityFromExternalSystem GetCity()
        {
            return new CityFromExternalSystem("Dhaka", "Capital of Bangladesh", 100);
        }
    }

    public class City
    {
        public string FullName { get; set; }
        public long Inhabitants { get; set; }

        public City(string fullName, long inhabitants)
        {
            FullName = fullName;
            Inhabitants = inhabitants;
        }
    }

    /// <summary>
    /// Target
    /// </summary>
    public interface ICityAdapter
    {
        City GetCity();
    }

    /// <summary>
    /// Adapter
    /// </summary>
    public class CityAdapter : ExternalSystem, ICityAdapter
    {
        public new City GetCity()
        {
            var externalCity = base.GetCity();
            return new City(
                $"{externalCity.Name} , {externalCity.KnownAs}",
                externalCity.Inhabitants
            );
        }
    }
}
