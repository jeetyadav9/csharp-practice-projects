using CSharpPracticeProjects.App;
using CSharpPracticeProjects.Core;
using CSharpPracticeProjects.Services;

namespace CSharpPracticeProjects.Features;

public sealed class NumberToolkitFeature : IFeature
{
    private readonly NumberToolkit _numberToolkit;

    public NumberToolkitFeature(NumberToolkit numberToolkit)
    {
        _numberToolkit = numberToolkit;
    }

    public string Name => "Number Toolkit";
    public string Description => "Prime, factorial, Fibonacci, GCD";

    public Task RunAsync()
    {
        while (true)
        {
            ConsoleTheme.Section("Number Toolkit Menu");
            ConsoleTheme.MenuItem(1, "Prime Check", "Check if a number is prime");
            ConsoleTheme.MenuItem(2, "Factorial", "Calculate n!");
            ConsoleTheme.MenuItem(3, "Fibonacci", "Calculate Fibonacci number");
            ConsoleTheme.MenuItem(4, "GCD and LCM", "Compare two numbers");
            ConsoleTheme.MenuItem(0, "Back", "Return to main menu");

            int option = InputReader.ReadInt("Choose", 0, 4);
            if (option == 0) return Task.CompletedTask;

            if (option == 1) PrimeCheck();
            if (option == 2) Factorial();
            if (option == 3) Fibonacci();
            if (option == 4) GcdAndLcm();
        }
    }

    private void PrimeCheck()
    {
        int number = InputReader.ReadInt("Number", 0, 1000000);
        ConsoleTheme.KeyValue("Is prime", _numberToolkit.IsPrime(number));
        InputReader.Pause();
    }

    private void Factorial()
    {
        int number = InputReader.ReadInt("Number", 0, 100);
        ConsoleTheme.KeyValue($"{number}!", _numberToolkit.Factorial(number));
        InputReader.Pause();
    }

    private void Fibonacci()
    {
        int index = InputReader.ReadInt("Index", 0, 500);
        ConsoleTheme.KeyValue($"Fibonacci({index})", _numberToolkit.Fibonacci(index));
        InputReader.Pause();
    }

    private void GcdAndLcm()
    {
        int first = InputReader.ReadInt("First number", 0, 1000000);
        int second = InputReader.ReadInt("Second number", 0, 1000000);
        ConsoleTheme.KeyValue("GCD", _numberToolkit.GreatestCommonDivisor(first, second));
        ConsoleTheme.KeyValue("LCM", _numberToolkit.LeastCommonMultiple(first, second));
        InputReader.Pause();
    }
}
