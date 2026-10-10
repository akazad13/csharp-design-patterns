using Composite;

Console.Title = "Composite Pattern";

var root = new DirectoryItem("Root");

var documents = new DirectoryItem("Documents");
documents.Add(new FileItem("Resume.pdf", 250));
documents.Add(new FileItem("Budget.xlsx", 540));

var pictures = new DirectoryItem("Pictures");
pictures.Add(new FileItem("Vacation.jpg", 3200));

var screenshots = new DirectoryItem("Screenshots");
screenshots.Add(new FileItem("BugReport.png", 410));
pictures.Add(screenshots);

root.Add(documents);
root.Add(pictures);
root.Add(new FileItem("readme.txt", 5));

Console.WriteLine("File System Hierarchy:");
root.Display();

Console.WriteLine($"\nTotal Size of Root directory: {root.GetSize()} KB");
