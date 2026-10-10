using State;

Console.Title = "State Pattern";

var order = new OrderContext();

// 1. Invalid action in PendingPayment state
Console.WriteLine("--- Action 1: Try shipping unpaid order ---");
order.Ship();

// 2. Pay for the order
Console.WriteLine("\n--- Action 2: Pay for order ---");
order.Pay();

// 3. Ship order
Console.WriteLine("\n--- Action 3: Ship order ---");
order.Ship();

// 4. Try cancelling order while shipped
Console.WriteLine("\n--- Action 4: Try cancelling while in transit ---");
order.Cancel();

// 5. Deliver order
Console.WriteLine("\n--- Action 5: Deliver order ---");
order.Deliver();

// 6. Action on delivered order
Console.WriteLine("\n--- Action 6: Try paying again ---");
order.Pay();
