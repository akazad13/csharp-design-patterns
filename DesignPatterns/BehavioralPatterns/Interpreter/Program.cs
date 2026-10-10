using Interpreter;

Console.Title = "Interpreter Pattern";

// Expression tree to evaluate: (a + b) * (c - 5)
// where a = 10, b = 20, c = 12
var context = new ExpressionContext();
context.SetVariable("a", 10);
context.SetVariable("b", 20);
context.SetVariable("c", 12);

Console.WriteLine("Evaluating expression: (a + b) * (c - 5)");
Console.WriteLine("With variables: a = 10, b = 20, c = 12\n");

IExpression leftPart = new AddExpression(
    new VariableExpression("a"),
    new VariableExpression("b")
); // 10 + 20 = 30

IExpression rightPart = new SubtractExpression(
    new VariableExpression("c"),
    new NumberExpression(5)
); // 12 - 5 = 7

IExpression syntaxTree = new MultiplyExpression(leftPart, rightPart); // 30 * 7 = 210

int result = syntaxTree.Interpret(context);
Console.WriteLine($"Evaluated Result: {result}");
