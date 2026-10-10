using Repository;

Console.Title = "Repository Pattern";

IProductRepository repository = new InMemoryProductRepository();

// Add products
Console.WriteLine("--- 1. Adding Products ---");
repository.Add(new Product(1, "Ergonomic Chair", 299.99m, 15));
repository.Add(new Product(2, "Standing Desk", 499.50m, 8));
repository.Add(new Product(3, "USB-C Docking Station", 149.00m, 0)); // Out of stock

// Query all products
Console.WriteLine("\n--- 2. All Products ---");
foreach (var p in repository.GetAll())
{
    Console.WriteLine($" -> [{p.Id}] {p.Name} - ${p.Price:F2} (Stock: {p.Stock})");
}

// Query in-stock products
Console.WriteLine("\n--- 3. In-Stock Products ---");
foreach (var p in repository.GetProductsInStock())
{
    Console.WriteLine($" -> [{p.Id}] {p.Name} - Stock: {p.Stock}");
}

// Update product
Console.WriteLine("\n--- 4. Updating Product #1 Price ---");
repository.Update(new Product(1, "Ergonomic Chair Pro", 349.99m, 12));

// Delete product #3
Console.WriteLine("\n--- 5. Deleting Product #3 ---");
repository.Delete(3);

Console.WriteLine($"\nRemaining Products Count: {repository.GetAll().Count()}");
