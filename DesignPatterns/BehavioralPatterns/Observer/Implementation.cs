namespace Observer
{
    public class Ticket
    {
        public int MovieId { get; private set; }
        public int Amount { get; private set; }

        public Ticket(int movieId, int amount)
        {
            MovieId = movieId;
            Amount = amount;
        }
    }

    /// <summary>
    /// Subject
    /// </summary>

    public abstract class TicketChangeNotifier {
        private List<ITicketChangeListener> _observers = [];

        public void AddObserver(ITicketChangeListener observer) {
            _observers.Add(observer);
        }

        public void RemoveObserver(ITicketChangeListener observer) {
            _observers.Remove(observer);
        }

        public void NotifyObservers(Ticket ticket) {
            foreach (var observer in _observers) {
                observer.OnTicketChange(ticket);
            }
        }
    }

    /// <summary>
    /// Observer
    /// </summary>
    public interface ITicketChangeListener {
        void OnTicketChange(Ticket ticket);
    }

    /// <summary>
    /// Concrete Subject
    /// </summary>

    public class OrderService : TicketChangeNotifier {
        public void PlaceOrder(int movieId, int amount) {
            Console.WriteLine($"{nameof(OrderService)} is changing its state.");
            var ticket = new Ticket(movieId, amount);
            Console.WriteLine($"{nameof(OrderService)} is notifying observers...");
            NotifyObservers(ticket);
        }
    }

    /// <summary>
    /// Concrete Observer 1
    /// </summary>

    public class TicketResellerService : ITicketChangeListener {
        public void OnTicketChange(Ticket ticket) {
            Console.WriteLine($"{nameof(TicketResellerService)} received a new ticket: {ticket.MovieId} - {ticket.Amount}");
        }
    }

    /// <summary>
    /// Concrete Observer 2
    /// </summary>

    public class TicketStockService : ITicketChangeListener {
        public void OnTicketChange(Ticket ticket) {
            Console.WriteLine($"{nameof(TicketStockService)} received a new ticket: {ticket.MovieId} - {ticket.Amount}");
        }
    }
}
