namespace Composite
{
    /// <summary>
    /// Component
    /// </summary>
    public abstract class FileSystemItem
    {
        public string Name { get; set; }

        protected FileSystemItem(string name)
        {
            Name = name;
        }

        public abstract long GetSize();
        public abstract void Display(int depth = 0);
    }

    /// <summary>
    /// Leaf
    /// </summary>
    public class FileItem : FileSystemItem
    {
        public long Size { get; }

        public FileItem(string name, long size) : base(name)
        {
            Size = size;
        }

        public override long GetSize() => Size;

        public override void Display(int depth = 0)
        {
            Console.WriteLine($"{new string(' ', depth * 2)}- File: {Name} ({Size} KB)");
        }
    }

    /// <summary>
    /// Composite
    /// </summary>
    public class DirectoryItem : FileSystemItem
    {
        private readonly List<FileSystemItem> _children = [];

        public DirectoryItem(string name) : base(name) { }

        public void Add(FileSystemItem item)
        {
            _children.Add(item);
        }

        public void Remove(FileSystemItem item)
        {
            _children.Remove(item);
        }

        public override long GetSize()
        {
            return _children.Sum(child => child.GetSize());
        }

        public override void Display(int depth = 0)
        {
            Console.WriteLine($"{new string(' ', depth * 2)}+ Directory: {Name} (Total: {GetSize()} KB)");
            foreach (var child in _children)
            {
                child.Display(depth + 1);
            }
        }
    }
}
