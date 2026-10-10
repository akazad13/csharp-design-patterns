using Iterator;

Console.Title = "Iterator Pattern";

var library = new BookCollection();
library.Add(new Book("Design Patterns: Elements of Reusable Object-Oriented Software", "Gang of Four", 1994));
library.Add(new Book("Clean Code", "Robert C. Martin", 2008));
library.Add(new Book("Refactoring", "Martin Fowler", 1999));
library.Add(new Book("Domain-Driven Design", "Eric Evans", 2003));

Console.WriteLine($"Iterating through library of {library.Count} books:");
var iterator = library.CreateIterator();

while (iterator.HasNext())
{
    var book = iterator.Next();
    Console.WriteLine($" -> \"{book.Title}\" by {book.Author} ({book.Year})");
}
