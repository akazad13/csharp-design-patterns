namespace Repository
{
    public record Product(int Id, string Name, decimal Price, int Stock);

    /// <summary>
    /// Generic Repository Interface
    /// </summary>
    public interface IRepository<T> where T : class
    {
        T? GetById(int id);
        IEnumerable<T> GetAll();
        void Add(T entity);
        void Update(T entity);
        bool Delete(int id);
    }

    /// <summary>
    /// Domain-specific Repository Interface
    /// </summary>
    public interface IProductRepository : IRepository<Product>
    {
        IEnumerable<Product> GetProductsInStock();
        IEnumerable<Product> SearchByName(string term);
    }

    /// <summary>
    /// Concrete In-Memory Implementation
    /// </summary>
    public class InMemoryProductRepository : IProductRepository
    {
        private readonly List<Product> _products = [];

        public Product? GetById(int id)
        {
            return _products.FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<Product> GetAll()
        {
            return _products.AsReadOnly();
        }

        public void Add(Product entity)
        {
            if (_products.Any(p => p.Id == entity.Id))
            {
                throw new InvalidOperationException($"Product with ID {entity.Id} already exists.");
            }
            _products.Add(entity);
            Console.WriteLine($"[Repository] Added product: #{entity.Id} - {entity.Name}");
        }

        public void Update(Product entity)
        {
            var index = _products.FindIndex(p => p.Id == entity.Id);
            if (index >= 0)
            {
                _products[index] = entity;
                Console.WriteLine($"[Repository] Updated product: #{entity.Id} - {entity.Name}");
            }
            else
            {
                throw new KeyNotFoundException($"Product with ID {entity.Id} not found.");
            }
        }

        public bool Delete(int id)
        {
            var product = GetById(id);
            if (product != null)
            {
                _products.Remove(product);
                Console.WriteLine($"[Repository] Deleted product: #{id}");
                return true;
            }
            return false;
        }

        public IEnumerable<Product> GetProductsInStock()
        {
            return _products.Where(p => p.Stock > 0);
        }

        public IEnumerable<Product> SearchByName(string term)
        {
            return _products.Where(p => p.Name.Contains(term, StringComparison.OrdinalIgnoreCase));
        }
    }
}
