namespace FactoryMethod
{
    /// <summary>
    /// Product
    /// </summary>
    public abstract class DiscountService
    {
        public abstract int DiscountPercentage { get; }

        public override string ToString() => GetType().Name;
    }

    /// <summary>
    /// Concreate Product A
    /// </summary>
    public class CountryDiscountService : DiscountService
    {
        private readonly string _countryIdentifier;

        public CountryDiscountService(string countryIdentifier)
        {
            _countryIdentifier = countryIdentifier;
        }

        public override int DiscountPercentage
        {
            get
            {
                switch (_countryIdentifier)
                {
                    case "USA":
                        return 10;
                    case "UK":
                        return 15;
                    case "GER":
                        return 20;
                    default:
                        return 0; // No discount for other countries
                }
            }
        }
    }

    /// <summary>
    /// Concreate Product B
    /// </summary>
    public class CodeDiscountService : DiscountService
    {
        private readonly Guid _code;

        public CodeDiscountService(Guid code)
        {
            _code = code;
        }

        public override int DiscountPercentage
        {
            get => 10;
        }
    }

    /// <summary>
    /// Creator
    /// </summary>
    public abstract class DiscountFactory
    {
        public abstract DiscountService CreateDiscountService();
    }

    /// <summary>
    /// Concreate Creator A
    /// </summary>
    public class CountryDiscountFactory : DiscountFactory
    {
        public readonly string _countryIdentifier;

        public CountryDiscountFactory(string countryIdentifier)
        {
            _countryIdentifier = countryIdentifier;
        }

        public override DiscountService CreateDiscountService()
        {
            return new CountryDiscountService(_countryIdentifier);
        }
    }

    /// <summary>
    /// Concreate Creator B
    /// </summary>
    public class CodeDiscountFactory : DiscountFactory
    {
        public readonly Guid _code;

        public CodeDiscountFactory(Guid code)
        {
            _code = code;
        }

        public override DiscountService CreateDiscountService()
        {
            return new CodeDiscountService(_code);
        }
    }
}
