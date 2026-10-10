namespace Flyweight
{
    /// <summary>
    /// Flyweight (holds intrinsic shared state)
    /// </summary>
    public class TreeType
    {
        public string Name { get; }
        public string Color { get; }
        public string Texture { get; }

        public TreeType(string name, string color, string texture)
        {
            Name = name;
            Color = color;
            Texture = texture;
        }

        public void Draw(int x, int y)
        {
            Console.WriteLine($"Drawing tree '{Name}' [Color: {Color}, Texture: {Texture}] at coordinates ({x}, {y})");
        }
    }

    /// <summary>
    /// Flyweight Factory (caches and reuses intrinsic state objects)
    /// </summary>
    public class TreeFactory
    {
        private static readonly Dictionary<string, TreeType> _treeTypes = [];

        public static TreeType GetTreeType(string name, string color, string texture)
        {
            string key = $"{name}_{color}_{texture}";

            if (!_treeTypes.TryGetValue(key, out var treeType))
            {
                treeType = new TreeType(name, color, texture);
                _treeTypes[key] = treeType;
                Console.WriteLine($"[TreeFactory] Created NEW TreeType: {key}");
            }
            else
            {
                Console.WriteLine($"[TreeFactory] Reusing EXISTING TreeType: {key}");
            }

            return treeType;
        }

        public static int TotalFlyweightsCreated => _treeTypes.Count;
    }

    /// <summary>
    /// Context object (contains extrinsic state)
    /// </summary>
    public class Tree
    {
        public int X { get; }
        public int Y { get; }
        public TreeType Type { get; }

        public Tree(int x, int y, TreeType type)
        {
            X = x;
            Y = y;
            Type = type;
        }

        public void Draw()
        {
            Type.Draw(X, Y);
        }
    }

    /// <summary>
    /// Client
    /// </summary>
    public class Forest
    {
        private readonly List<Tree> _trees = [];

        public void PlantTree(int x, int y, string name, string color, string texture)
        {
            var type = TreeFactory.GetTreeType(name, color, texture);
            var tree = new Tree(x, y, type);
            _trees.Add(tree);
        }

        public void Draw()
        {
            foreach (var tree in _trees)
            {
                tree.Draw();
            }
        }

        public int TotalTrees => _trees.Count;
    }
}
