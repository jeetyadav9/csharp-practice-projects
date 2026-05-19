using CSharpPracticeProjects.App;
using CSharpPracticeProjects.Core;
using CSharpPracticeProjects.Services;

namespace CSharpPracticeProjects.Features;

public sealed class CalculatorFeature : IFeature
{
    private readonly ExpressionCalculator _calculator;
    private readonly List<string> _history = new();

    public CalculatorFeature(ExpressionCalculator calculator)
    {
        _calculator = calculator;
    }

    public string Name => "Smart Calculator";
    public string Description => "Evaluate strong math expressions";

    public Task RunAsync()
    {
        while (true)
        {
            ConsoleTheme.Section("Calculator Menu");
            ConsoleTheme.MenuItem(1, "Calculate", "Evaluate an expression");
            ConsoleTheme.MenuItem(2, "Examples", "View supported examples");
            ConsoleTheme.MenuItem(3, "History", "View recent calculations");
            ConsoleTheme.MenuItem(0, "Back", "Return to main menu");

            int option = InputReader.ReadInt("Choose", 0, 3);
            if (option == 0) return Task.CompletedTask;

            if (option == 1) Calculate();
            if (option == 2) ShowExamples();
            if (option == 3) ShowHistory();
        }
    }

    private void Calculate()
    {
        string expression = InputReader.ReadRequiredString("Expression", 200);

        try
        {
            double result = _calculator.Evaluate(expression);
            string entry = $"{expression} = {result}";
            _history.Add(entry);
            ConsoleTheme.Success(entry);
        }
        catch (Exception ex)
        {
            ConsoleTheme.Error(ex.Message);
        }

        InputReader.Pause();
    }

    private static void ShowExamples()
    {
        ConsoleTheme.Info("Try these expressions:");
        Console.WriteLine("2 + 3 * 4");
        Console.WriteLine("(10 + 5) / 3");
        Console.WriteLine("sqrt(144)");
        Console.WriteLine("2 ^ 8");
        Console.WriteLine("round(12.8)");
        InputReader.Pause();
    }

    private void ShowHistory()
    {
        if (_history.Count == 0)
        {
            ConsoleTheme.Warning("No calculations yet.");
        }
        else
        {
            foreach (string item in _history.TakeLast(10))
            {
                Console.WriteLine(item);
            }
        }

        InputReader.Pause();
    }
}
