using FactoryMethod;

Restaurant beefBurgerFactory = new BeefBurgerRestaurant();
Burger beefBurger = beefBurgerFactory.OrderBurger();

Restaurant veggieBurgerFactory = new VeggieBurgerRestaurant();
Burger veggieBurger = veggieBurgerFactory.OrderBurger();

var factories = new List<DiscountFactory>
{
    new CodeDiscountFactory(Guid.NewGuid()),
    new CountryDiscountFactory("GER")
};

foreach (var factory in factories)
{
    var discountService = factory.CreateDiscountService();
    Console.WriteLine(
        $"Percentage {discountService.DiscountPercentage} " + $"coming from {discountService}"
    );
}
