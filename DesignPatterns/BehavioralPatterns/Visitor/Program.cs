using Visitor;

Console.Title = "Visitor Pattern";

List<IShoppingItem> cart =
[
    new BookItem("Clean Architecture", 35.00m, "978-0134494166"),
    new ElectronicItem("Sony Noise-Cancelling Headphones", 349.99m),
    new GroceryItem("Organic Apples", 3.50m, 4.0) // 4 kg
];

Console.WriteLine("--- 1. Tax Calculation using TaxVisitor ---");
var taxVisitor = new TaxVisitor();
foreach (var item in cart)
{
    item.Accept(taxVisitor);
}
Console.WriteLine($"Total Calculated Tax: ${taxVisitor.TotalTax:F2}");

Console.WriteLine("\n--- 2. Discount Calculation using DiscountVisitor ---");
var discountVisitor = new DiscountVisitor();
foreach (var item in cart)
{
    item.Accept(discountVisitor);
}
Console.WriteLine($"Total Calculated Discount: ${discountVisitor.TotalDiscount:F2}");
