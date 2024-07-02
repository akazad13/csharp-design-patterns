namespace AbstractFactory
{
    /// <summary>
    /// Creator / Abstract Factory
    /// </summary>
    public interface IShoppingCartPurchaseFactory
    {
        public IShippingCostsService CreateShippingCostsService();
        public IDiscountService CreateDiscountService();
    }

    /// <summary>
    /// Abstract Product A
    /// </summary>

    public interface IDiscountService
    {
        public int DiscountPercentage { get; set; }
    }

    /// <summary>
    /// Abstract Product B
    /// </summary>

    public interface IShippingCostsService
    {
        public decimal ShippingCost { get; set; }
    }

    /// <summary>
    /// Concrete Product A1
    /// </summary>
    public class IndiaDiscountService : IDiscountService
    {
        public int DiscountPercentage { get; set; } = 10;
    }

    /// <summary>
    /// Concrete Product B1
    /// </summary>
    public class IndiaShippingCostsService : IShippingCostsService
    {
        public decimal ShippingCost { get; set; } = 20;
    }

    /// <summary>
    /// Concrete Product A2
    /// </summary>
    public class ChinaDiscountService : IDiscountService
    {
        public int DiscountPercentage { get; set; } = 20;
    }

    /// <summary>
    /// Concrete Product B2
    /// </summary>
    public class ChinaShippingCostsService : IShippingCostsService
    {
        public decimal ShippingCost { get; set; } = 50;
    }

    /// <summary>
    /// Concrete factory 1
    /// </summary>
    public class IndiaShoppingCartPurchaseFactory : IShoppingCartPurchaseFactory
    {
        public IShippingCostsService CreateShippingCostsService()
        {
            return new IndiaShippingCostsService();
        }

        public IDiscountService CreateDiscountService()
        {
            return new IndiaDiscountService();
        }
    }

    /// <summary>
    /// Concrete factory 2
    /// </summary>
    public class ChinaShoppingCartPurchaseFactory : IShoppingCartPurchaseFactory
    {
        public IShippingCostsService CreateShippingCostsService()
        {
            return new ChinaShippingCostsService();
        }

        public IDiscountService CreateDiscountService()
        {
            return new ChinaDiscountService();
        }
    }

    /// <summary>
    /// Client Class
    /// </summary>

    public class ShoppingCart
    {
        private readonly IShippingCostsService _shippingCostsService;
        private readonly IDiscountService _discountService;
        private decimal _costOfOrders;

        public ShoppingCart(IShoppingCartPurchaseFactory factory)
        {
            _discountService = factory.CreateDiscountService();
            _shippingCostsService = factory.CreateShippingCostsService();

            _costOfOrders = 300;
        }

        public void CalculateCost()
        {
            Console.WriteLine($"Shipping Cost: {_shippingCostsService.ShippingCost}");
            Console.WriteLine($"Discount: {_discountService.DiscountPercentage}%");
            var discountAmount = (decimal)_discountService.DiscountPercentage / 100 * _costOfOrders;
            var totalCost = _costOfOrders - discountAmount + _shippingCostsService.ShippingCost;
            Console.WriteLine($"Total Cost: {totalCost}");
        }
    }
}
