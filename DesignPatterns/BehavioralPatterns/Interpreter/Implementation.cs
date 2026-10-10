namespace Interpreter
{
    /// <summary>
    /// Context containing variables and shared state
    /// </summary>
    public class ExpressionContext
    {
        private readonly Dictionary<string, int> _variables = [];

        public void SetVariable(string name, int value)
        {
            _variables[name] = value;
        }

        public int GetVariable(string name)
        {
            if (_variables.TryGetValue(name, out var val))
            {
                return val;
            }
            throw new KeyNotFoundException($"Variable '{name}' not found in expression context.");
        }
    }

    /// <summary>
    /// Abstract Expression
    /// </summary>
    public interface IExpression
    {
        int Interpret(ExpressionContext context);
    }

    /// <summary>
    /// Terminal Expression: Constant Number
    /// </summary>
    public class NumberExpression : IExpression
    {
        private readonly int _number;

        public NumberExpression(int number)
        {
            _number = number;
        }

        public int Interpret(ExpressionContext context) => _number;
    }

    /// <summary>
    /// Terminal Expression: Variable lookup
    /// </summary>
    public class VariableExpression : IExpression
    {
        private readonly string _name;

        public VariableExpression(string name)
        {
            _name = name;
        }

        public int Interpret(ExpressionContext context) => context.GetVariable(_name);
    }

    /// <summary>
    /// Non-Terminal Expression: Addition (left + right)
    /// </summary>
    public class AddExpression : IExpression
    {
        private readonly IExpression _left;
        private readonly IExpression _right;

        public AddExpression(IExpression left, IExpression right)
        {
            _left = left;
            _right = right;
        }

        public int Interpret(ExpressionContext context)
        {
            return _left.Interpret(context) + _right.Interpret(context);
        }
    }

    /// <summary>
    /// Non-Terminal Expression: Subtraction (left - right)
    /// </summary>
    public class SubtractExpression : IExpression
    {
        private readonly IExpression _left;
        private readonly IExpression _right;

        public SubtractExpression(IExpression left, IExpression right)
        {
            _left = left;
            _right = right;
        }

        public int Interpret(ExpressionContext context)
        {
            return _left.Interpret(context) - _right.Interpret(context);
        }
    }

    /// <summary>
    /// Non-Terminal Expression: Multiplication (left * right)
    /// </summary>
    public class MultiplyExpression : IExpression
    {
        private readonly IExpression _left;
        private readonly IExpression _right;

        public MultiplyExpression(IExpression left, IExpression right)
        {
            _left = left;
            _right = right;
        }

        public int Interpret(ExpressionContext context)
        {
            return _left.Interpret(context) * _right.Interpret(context);
        }
    }
}
