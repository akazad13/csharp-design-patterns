namespace UnitOfWork
{
    public record Customer(int Id, string Name, string Email);
    public record Order(int Id, int CustomerId, decimal Amount, DateTime CreatedAt);

    /// <summary>
    /// Customer Repository Interface
    /// </summary>
    public interface ICustomerRepository
    {
        Customer? GetById(int id);
        IEnumerable<Customer> GetAll();
        void Add(Customer customer);
    }

    /// <summary>
    /// Order Repository Interface
    /// </summary>
    public interface IOrderRepository
    {
        Order? GetById(int id);
        IEnumerable<Order> GetByCustomerId(int customerId);
        void Add(Order order);
    }

    /// <summary>
    /// Unit of Work Interface
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        ICustomerRepository Customers { get; }
        IOrderRepository Orders { get; }
        bool Commit();
        void Rollback();
    }

    /// <summary>
    /// Concrete Unit of Work managing transaction boundary
    /// </summary>
    public class InMemoryUnitOfWork : IUnitOfWork
    {
        // Underlying persistent data store
        private readonly List<Customer> _databaseCustomers = [];
        private readonly List<Order> _databaseOrders = [];

        // Pending transaction changes
        private readonly List<Customer> _pendingCustomers = [];
        private readonly List<Order> _pendingOrders = [];

        public ICustomerRepository Customers { get; }
        public IOrderRepository Orders { get; }

        public InMemoryUnitOfWork()
        {
            Customers = new CustomerRepository(this);
            Orders = new OrderRepository(this);
        }

        public void RegisterNewCustomer(Customer customer)
        {
            _pendingCustomers.Add(customer);
            Console.WriteLine($"[UnitOfWork] Tracked new Customer: #{customer.Id} ({customer.Name})");
        }

        public void RegisterNewOrder(Order order)
        {
            _pendingOrders.Add(order);
            Console.WriteLine($"[UnitOfWork] Tracked new Order: #{order.Id} (${order.Amount:F2})");
        }

        public bool Commit()
        {
            Console.WriteLine($"[UnitOfWork] Committing transaction ({_pendingCustomers.Count} customers, {_pendingOrders.Count} orders)...");

            // Atomic commit: all or nothing
            try
            {
                _databaseCustomers.AddRange(_pendingCustomers);
                _databaseOrders.AddRange(_pendingOrders);

                _pendingCustomers.Clear();
                _pendingOrders.Clear();

                Console.WriteLine("[UnitOfWork] Transaction committed successfully to database.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[UnitOfWork] Commit failed: {ex.Message}");
                Rollback();
                return false;
            }
        }

        public void Rollback()
        {
            Console.WriteLine($"[UnitOfWork] Rolling back transaction. Discarding {_pendingCustomers.Count} pending customers and {_pendingOrders.Count} pending orders.");
            _pendingCustomers.Clear();
            _pendingOrders.Clear();
        }

        public IEnumerable<Customer> GetCommittedCustomers() => _databaseCustomers.AsReadOnly();
        public IEnumerable<Order> GetCommittedOrders() => _databaseOrders.AsReadOnly();

        public void Dispose()
        {
            if (_pendingCustomers.Count > 0 || _pendingOrders.Count > 0)
            {
                Rollback();
            }
            GC.SuppressFinalize(this);
        }

        private class CustomerRepository : ICustomerRepository
        {
            private readonly InMemoryUnitOfWork _uow;

            public CustomerRepository(InMemoryUnitOfWork uow) => _uow = uow;

            public Customer? GetById(int id) =>
                _uow._databaseCustomers.Concat(_uow._pendingCustomers).FirstOrDefault(c => c.Id == id);

            public IEnumerable<Customer> GetAll() =>
                _uow._databaseCustomers.Concat(_uow._pendingCustomers);

            public void Add(Customer customer) => _uow.RegisterNewCustomer(customer);
        }

        private class OrderRepository : IOrderRepository
        {
            private readonly InMemoryUnitOfWork _uow;

            public OrderRepository(InMemoryUnitOfWork uow) => _uow = uow;

            public Order? GetById(int id) =>
                _uow._databaseOrders.Concat(_uow._pendingOrders).FirstOrDefault(o => o.Id == id);

            public IEnumerable<Order> GetByCustomerId(int customerId) =>
                _uow._databaseOrders.Concat(_uow._pendingOrders).Where(o => o.CustomerId == customerId);

            public void Add(Order order) => _uow.RegisterNewOrder(order);
        }
    }
}
