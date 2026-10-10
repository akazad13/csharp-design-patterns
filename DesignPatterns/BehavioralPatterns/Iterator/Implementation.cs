namespace Iterator
{
    public record Book(string Title, string Author, int Year);

    /// <summary>
    /// Iterator Interface
    /// </summary>
    public interface IIterator<T>
    {
        bool HasNext();
        T Next();
        void Reset();
    }

    /// <summary>
    /// Aggregate Interface
    /// </summary>
    public interface IBookAggregate
    {
        IIterator<Book> CreateIterator();
    }

    /// <summary>
    /// Concrete Aggregate
    /// </summary>
    public class BookCollection : IBookAggregate
    {
        private readonly List<Book> _books = [];

        public void Add(Book book)
        {
            _books.Add(book);
        }

        public int Count => _books.Count;

        public Book this[int index] => _books[index];

        public IIterator<Book> CreateIterator()
        {
            return new BookIterator(this);
        }
    }

    /// <summary>
    /// Concrete Iterator
    /// </summary>
    public class BookIterator : IIterator<Book>
    {
        private readonly BookCollection _collection;
        private int _currentPosition = 0;

        public BookIterator(BookCollection collection)
        {
            _collection = collection;
        }

        public bool HasNext()
        {
            return _currentPosition < _collection.Count;
        }

        public Book Next()
        {
            if (!HasNext())
            {
                throw new InvalidOperationException("End of collection reached.");
            }

            var book = _collection[_currentPosition];
            _currentPosition++;
            return book;
        }

        public void Reset()
        {
            _currentPosition = 0;
        }
    }
}
