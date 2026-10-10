namespace Momento
{
    /// <summary>
    /// Memento: stores the internal state of the Originator
    /// </summary>
    public class TextEditorMemento
    {
        public string Content { get; }
        public DateTime SavedAt { get; }

        public TextEditorMemento(string content)
        {
            Content = content;
            SavedAt = DateTime.UtcNow;
        }
    }

    /// <summary>
    /// Originator: creates a memento containing a snapshot of its current state,
    /// and uses the memento to restore its internal state.
    /// </summary>
    public class TextEditor
    {
        public string Content { get; private set; } = string.Empty;

        public void Type(string words)
        {
            Content = string.IsNullOrEmpty(Content) ? words : $"{Content} {words}";
        }

        public TextEditorMemento Save()
        {
            Console.WriteLine($"[Editor] Snapshot created with content: \"{Content}\"");
            return new TextEditorMemento(Content);
        }

        public void Restore(TextEditorMemento memento)
        {
            Content = memento.Content;
            Console.WriteLine($"[Editor] State restored from memento ({memento.SavedAt:T}): \"{Content}\"");
        }
    }

    /// <summary>
    /// Caretaker: keeps track of memento history without modifying memento contents
    /// </summary>
    public class EditorHistory
    {
        private readonly Stack<TextEditorMemento> _history = new();

        public void Push(TextEditorMemento memento)
        {
            _history.Push(memento);
        }

        public TextEditorMemento? Pop()
        {
            if (_history.Count > 0)
            {
                return _history.Pop();
            }

            Console.WriteLine("[History] No previous states found in history.");
            return null;
        }

        public int Count => _history.Count;
    }
}
