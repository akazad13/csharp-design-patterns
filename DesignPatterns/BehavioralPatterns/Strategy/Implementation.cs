namespace Strategy
{
    /// <summary>
    /// Strategy Interface
    /// </summary>
    public interface IPaymentStrategy
    {
        void Pay(decimal amount);
    }

    /// <summary>
    /// Concrete Strategy A
    /// </summary>
    public class CreditCardPaymentStrategy : IPaymentStrategy
    {
        private readonly string _cardNumber;
        private readonly string _cardHolder;

        public CreditCardPaymentStrategy(string cardNumber, string cardHolder)
        {
            _cardNumber = cardNumber;
            _cardHolder = cardHolder;
        }

        public void Pay(decimal amount)
        {
            Console.WriteLine($"Paid ${amount:F2} using Credit Card ending in {_cardNumber[^4..]} (Cardholder: {_cardHolder}).");
        }
    }

    /// <summary>
    /// Concrete Strategy B
    /// </summary>
    public class PayPalPaymentStrategy : IPaymentStrategy
    {
        private readonly string _email;

        public PayPalPaymentStrategy(string email)
        {
            _email = email;
        }

        public void Pay(decimal amount)
        {
            Console.WriteLine($"Paid ${amount:F2} using PayPal account ({_email}).");
        }
    }

    /// <summary>
    /// Concrete Strategy C
    /// </summary>
    public class CryptoPaymentStrategy : IPaymentStrategy
    {
        private readonly string _walletAddress;

        public CryptoPaymentStrategy(string walletAddress)
        {
            _walletAddress = walletAddress;
        }

        public void Pay(decimal amount)
        {
            Console.WriteLine($"Paid ${amount:F2} via Cryptocurrency wallet ({_walletAddress[..6]}...{_walletAddress[^4..]}).");
        }
    }

    /// <summary>
    /// Context
    /// </summary>
    public class ShoppingCartContext
    {
        private IPaymentStrategy? _paymentStrategy;
        private readonly List<(string Item, decimal Price)> _items = [];

        public void AddItem(string item, decimal price)
        {
            _items.Add((item, price));
        }

        public void SetPaymentStrategy(IPaymentStrategy strategy)
        {
            _paymentStrategy = strategy;
        }

        public void Checkout()
        {
            if (_paymentStrategy == null)
            {
                throw new InvalidOperationException("Payment strategy is not set.");
            }

            decimal total = _items.Sum(i => i.Price);
            Console.WriteLine($"Total Order Amount: ${total:F2} across {_items.Count} items.");
            _paymentStrategy.Pay(total);
            _items.Clear();
        }
    }
}
