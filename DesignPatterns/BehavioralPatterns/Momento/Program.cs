using Momento;

Console.Title = "Memento Pattern";

var editor = new TextEditor();
var history = new EditorHistory();

// 1. Initial typing
editor.Type("Hello,");
editor.Type("world!");
Console.WriteLine($"Current Content: \"{editor.Content}\"");
history.Push(editor.Save());

// 2. Further modifications
editor.Type("Design Patterns are powerful.");
Console.WriteLine($"Current Content: \"{editor.Content}\"");
history.Push(editor.Save());

// 3. Mistake typing
editor.Type("This is a typo error here.");
Console.WriteLine($"Current Content: \"{editor.Content}\"");

// 4. Undo 1
Console.WriteLine("\n--- Undoing last change ---");
var memento1 = history.Pop();
if (memento1 != null) editor.Restore(memento1);

// 5. Undo 2
Console.WriteLine("\n--- Undoing another change ---");
var memento2 = history.Pop();
if (memento2 != null) editor.Restore(memento2);
