namespace Visitor
{
    /// <summary>
    /// Visitor Interface
    /// </summary>
    public interface IVisitor
    {
        void VisitBook(BookItem book);
        void VisitElectronic(ElectronicItem electronic);
        void VisitGrocery(GroceryItem grocery);
    }

    /// <summary>
    /// Element Interface
    /// </summary>
    public interface IShoppingItem
    {
        void Accept(IVisitor visitor);
    }

    /// <summary>
    /// Concrete Element A
    /// </summary>
    public class BookItem : IShoppingItem
    {
        public string Title { get; }
        public decimal Price { get; }
        public string Isbn { get; }

        public BookItem(string title, decimal price, string isbn)
        {
            Title = title;
            Price = price;
            Isbn = isbn;
        }

        public void Accept(IVisitor visitor)
        {
            visitor.VisitBook(this);
        }
    }

    /// <summary>
    /// Concrete Element B
    /// </summary>
    public class ElectronicItem : IShoppingItem
    {
        public string Brand { get; }
        public decimal Price { get; }

        public ElectronicItem(string brand, decimal price)
        {
            Brand = brand;
            Price = price;
        }

        public void Accept(IVisitor visitor)
        {
            visitor.VisitElectronic(this);
        }
    }

    /// <summary>
    /// Concrete Element C
    /// </summary>
    public class GroceryItem : IShoppingItem
    {
        public string Name { get; }
        public decimal PricePerKg { get; }
        public double WeightKg { get; }

        public GroceryItem(string name, decimal pricePerKg, double weightKg)
        {
            Name = name;
            PricePerKg = pricePerKg;
            WeightKg = weightKg;
        }

        public decimal TotalPrice => PricePerKg * (decimal)WeightKg;

        public void Accept(IVisitor visitor)
        {
            visitor.VisitGrocery(this);
        }
    }

    /// <summary>
    /// Concrete Visitor 1: Calculates sales tax
    /// </summary>
    public class TaxVisitor : IVisitor
    {
        public decimal TotalTax { get; private set; }

        public void VisitBook(BookItem book)
        {
            // Books are zero-rated / exempt from standard sales tax
            decimal tax = 0m;
            TotalTax += tax;
            Console.WriteLine($"[Tax] Book \"{book.Title}\": Tax exempt ($0.00)");
        }

        public void VisitElectronic(ElectronicItem electronic)
        {
            // Electronics have 18% luxury tax
            decimal tax = electronic.Price * 0.18m;
            TotalTax += tax;
            Console.WriteLine($"[Tax] Electronic \"{electronic.Brand}\": 18% Tax = ${tax:F2}");
        }

        public void VisitGrocery(GroceryItem grocery)
        {
            // Groceries have 5% food tax
            decimal tax = grocery.TotalPrice * 0.05m;
            TotalTax += tax;
            Console.WriteLine($"[Tax] Grocery \"{grocery.Name}\": 5% Tax = ${tax:F2}");
        }
    }

    /// <summary>
    /// Concrete Visitor 2: Calculates promotional discount
    /// </summary>
    public class DiscountVisitor : IVisitor
    {
        public decimal TotalDiscount { get; private set; }

        public void VisitBook(BookItem book)
        {
            decimal discount = book.Price * 0.10m; // 10% off books
            TotalDiscount += discount;
            Console.WriteLine($"[Discount] Book \"{book.Title}\": 10% off = -${discount:F2}");
        }

        public void VisitElectronic(ElectronicItem electronic)
        {
            decimal discount = electronic.Price > 200 ? 25m : 0m; // $25 flat rebate over $200
            TotalDiscount += discount;
            Console.WriteLine($"[Discount] Electronic \"{electronic.Brand}\": Flat rebate = -${discount:F2}");
        }

        public void VisitGrocery(GroceryItem grocery)
        {
            decimal discount = grocery.WeightKg > 3 ? grocery.TotalPrice * 0.15m : 0m;
            TotalDiscount += discount;
            Console.WriteLine($"[Discount] Grocery \"{grocery.Name}\": Bulk discount = -${discount:F2}");
        }
    }
}
