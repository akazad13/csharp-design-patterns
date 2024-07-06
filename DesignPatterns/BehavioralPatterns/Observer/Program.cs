using Observer;

Console.Title = "Observer Pattern";

TicketStockService ticketStockService = new();
TicketResellerService ticketResellerService = new();

OrderService orderService = new();

// add two observers

orderService.AddObserver(ticketStockService);
orderService.AddObserver(ticketResellerService);

// notify observers
orderService.PlaceOrder(1, 20);

Console.WriteLine("");

// remove one observer
orderService.RemoveObserver(ticketStockService);

//notify
orderService.PlaceOrder(2, 30);
