using System.Globalization;

namespace CSharpPracticeProjects.Core;

public static class InputReader
{
    public static string ReadRequiredString(string prompt, int maxLength = 120)
    {
        while (true)
        {
            ConsoleTheme.Prompt(prompt);
            string value = Console.ReadLine()?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(value))
            {
                ConsoleTheme.Error("Value is required.");
                continue;
            }

            if (value.Length > maxLength)
            {
                ConsoleTheme.Error($"Value must be {maxLength} characters or less.");
                continue;
            }

            return value;
        }
    }

    public static string ReadOptionalString(string prompt, int maxLength = 200)
    {
        while (true)
        {
            ConsoleTheme.Prompt(prompt);
            string value = Console.ReadLine()?.Trim() ?? string.Empty;

            if (value.Length <= maxLength)
            {
                return value;
            }

            ConsoleTheme.Error($"Value must be {maxLength} characters or less.");
        }
    }

    public static int ReadInt(string prompt, int min, int max)
    {
        while (true)
        {
            ConsoleTheme.Prompt($"{prompt} ({min}-{max})");
            string input = Console.ReadLine()?.Trim() ?? string.Empty;

            if (int.TryParse(input, NumberStyles.Integer, CultureInfo.InvariantCulture, out int value) && value >= min && value <= max)
            {
                return value;
            }

            ConsoleTheme.Error($"Enter a number between {min} and {max}.");
        }
    }

    public static decimal ReadDecimal(string prompt, decimal min, decimal max)
    {
        while (true)
        {
            ConsoleTheme.Prompt($"{prompt} ({min}-{max})");
            string input = Console.ReadLine()?.Trim() ?? string.Empty;

            if (decimal.TryParse(input, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal value) && value >= min && value <= max)
            {
                return value;
            }

            ConsoleTheme.Error($"Enter a valid amount between {min} and {max}.");
        }
    }

    public static DateTime? ReadOptionalDate(string prompt)
    {
        while (true)
        {
            ConsoleTheme.Prompt($"{prompt} (yyyy-mm-dd or empty)");
            string input = Console.ReadLine()?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(input))
            {
                return null;
            }

            if (DateTime.TryParseExact(input, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime value))
            {
                return value.Date;
            }

            ConsoleTheme.Error("Enter a date in yyyy-mm-dd format.");
        }
    }

    public static bool Confirm(string prompt)
    {
        while (true)
        {
            ConsoleTheme.Prompt($"{prompt} (y/n)");
            string input = Console.ReadLine()?.Trim().ToLowerInvariant() ?? string.Empty;

            if (input is "y" or "yes") return true;
            if (input is "n" or "no") return false;

            ConsoleTheme.Error("Please enter y or n.");
        }
    }

    public static string ReadMultiline(string prompt)
    {
        ConsoleTheme.Info(prompt);
        ConsoleTheme.Warning("Write your text. Enter a single dot on a new line to finish.");

        var lines = new List<string>();
        while (true)
        {
            string line = Console.ReadLine() ?? string.Empty;
            if (line.Trim() == ".")
            {
                break;
            }

            lines.Add(line);
        }

        return string.Join(Environment.NewLine, lines).Trim();
    }

    public static void Pause()
    {
        Console.WriteLine();
        ConsoleTheme.Warning("Press Enter to continue...");
        Console.ReadLine();
    }
}
