using Facade;

Console.Title = "Facade Patten";

var facade = new DiscountFacade();
Console.WriteLine(
    $"Discount percentage for customer with id 4: " + $"{facade.CalculateDiscountPercentage(4)}"
);
Console.WriteLine(
    $"Discount percentage for customer with id 10: " + $"{facade.CalculateDiscountPercentage(10)}"
);
