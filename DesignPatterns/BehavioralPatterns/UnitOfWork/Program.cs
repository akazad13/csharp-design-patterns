using UnitOfWork;

Console.Title = "Unit of Work Pattern";

using (var unitOfWork = new InMemoryUnitOfWork())
{
    // Transaction 1: Create customer and an initial order, then commit
    Console.WriteLine("--- Transaction 1: Add Customer & Order and Commit ---");
    unitOfWork.Customers.Add(new Customer(1, "David Miller", "david@example.com"));
    unitOfWork.Orders.Add(new Order(101, 1, 150.00m, DateTime.UtcNow));
    unitOfWork.Orders.Add(new Order(102, 1, 320.50m, DateTime.UtcNow));

    unitOfWork.Commit();

    Console.WriteLine($"Committed Customers in DB: {unitOfWork.GetCommittedCustomers().Count()}");
    Console.WriteLine($"Committed Orders in DB: {unitOfWork.GetCommittedOrders().Count()}");

    // Transaction 2: Staging changes then rolling back
    Console.WriteLine("\n--- Transaction 2: Stage Changes and Rollback ---");
    unitOfWork.Customers.Add(new Customer(2, "Invalid User", "invalid@example.com"));
    unitOfWork.Orders.Add(new Order(103, 2, 9999.00m, DateTime.UtcNow));

    Console.WriteLine("Encountered validation issue or exception during processing!");
    unitOfWork.Rollback();

    Console.WriteLine($"Committed Customers in DB: {unitOfWork.GetCommittedCustomers().Count()}");
    Console.WriteLine($"Committed Orders in DB: {unitOfWork.GetCommittedOrders().Count()}");
}
