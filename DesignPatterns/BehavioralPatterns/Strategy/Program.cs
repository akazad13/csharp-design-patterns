using Strategy;

Console.Title = "Strategy Pattern";

var cart = new ShoppingCartContext();
cart.AddItem("Mechanical Keyboard", 129.99m);
cart.AddItem("Wireless Mouse", 49.99m);

// 1. Pay with Credit Card
Console.WriteLine("--- Customer 1 Checkout (Credit Card) ---");
cart.SetPaymentStrategy(new CreditCardPaymentStrategy("4111111111119876", "John Doe"));
cart.Checkout();

// 2. Pay with PayPal
Console.WriteLine("\n--- Customer 2 Checkout (PayPal) ---");
cart.AddItem("UltraWide Monitor", 499.00m);
cart.SetPaymentStrategy(new PayPalPaymentStrategy("john.doe@example.com"));
cart.Checkout();

// 3. Pay with Crypto
Console.WriteLine("\n--- Customer 3 Checkout (Crypto) ---");
cart.AddItem("Noise-Cancelling Headphones", 249.50m);
cart.SetPaymentStrategy(new CryptoPaymentStrategy("0x71C84050D33E2B81308351B423fA4f479C938162"));
cart.Checkout();
