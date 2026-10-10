namespace Prototype
{
    /// <summary>
    /// Prototype Interface
    /// </summary>
    public interface IPrototype<T>
    {
        T Clone(bool deepClone = true);
    }

    /// <summary>
    /// Complex reference type property
    /// </summary>
    public class Author
    {
        public string Name { get; set; }
        public string Email { get; set; }

        public Author(string name, string email)
        {
            Name = name;
            Email = email;
        }

        public override string ToString() => $"{Name} ({Email})";
    }

    /// <summary>
    /// Concrete Prototype
    /// </summary>
    public class Document : IPrototype<Document>
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public Author Author { get; set; }
        public List<string> Tags { get; set; }

        public Document(string title, string content, Author author, List<string>? tags = null)
        {
            Title = title;
            Content = content;
            Author = author;
            Tags = tags ?? [];
        }

        public Document Clone(bool deepClone = true)
        {
            if (deepClone)
            {
                // Deep copy: duplicate reference objects
                return new Document(
                    Title,
                    Content,
                    new Author(Author.Name, Author.Email),
                    new List<string>(Tags)
                );
            }

            // Shallow copy
            return (Document)MemberwiseClone();
        }

        public override string ToString()
        {
            return $"Document: '{Title}' by {Author} | Tags: [{string.Join(", ", Tags)}] | Content: {Content}";
        }
    }
}
