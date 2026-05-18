namespace CSharpPracticeProjects.Core;

public static class ConsoleTheme
{
    public static void Clear()
    {
        Console.Clear();
    }

    public static void Banner(string title, string subtitle)
    {
        var previous = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("============================================================");
        Console.WriteLine($"  {title}");
        Console.WriteLine("============================================================");
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine($"  {subtitle}");
        Console.WriteLine();
        Console.ForegroundColor = previous;
    }

    public static void Section(string title)
    {
        var previous = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"\n=== {title} ===\n");
        Console.ForegroundColor = previous;
    }

    public static void Divider()
    {
        Console.WriteLine(new string('-', 60));
    }

    public static void MenuItem(int number, string title, string description)
    {
        var previous = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write($"{number,2}. {title,-24}");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine(description);
        Console.ForegroundColor = previous;
    }

    public static void Prompt(string text)
    {
        var previous = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write($"{text}: ");
        Console.ForegroundColor = previous;
    }

    public static void Success(string text) => WriteColored(text, ConsoleColor.Green);
    public static void Info(string text) => WriteColored(text, ConsoleColor.Gray);
    public static void Warning(string text) => WriteColored(text, ConsoleColor.Yellow);
    public static void Error(string text) => WriteColored(text, ConsoleColor.Red);

    public static void KeyValue(string key, object? value)
    {
        var previous = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write($"{key}: ");
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine(value?.ToString() ?? "-");
        Console.ForegroundColor = previous;
    }

    private static void WriteColored(string text, ConsoleColor color)
    {
        var previous = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.WriteLine(text);
        Console.ForegroundColor = previous;
    }
}
