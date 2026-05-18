namespace CSharpPracticeProjects.Core;

public static class TextTable
{
    public static void Print(string[] headers, IEnumerable<string[]> rows)
    {
        var rowList = rows.ToList();
        int[] widths = headers.Select(h => h.Length).ToArray();

        foreach (string[] row in rowList)
        {
            for (int i = 0; i < headers.Length && i < row.Length; i++)
            {
                widths[i] = Math.Max(widths[i], row[i].Length);
            }
        }

        PrintRow(headers, widths);
        Console.WriteLine(string.Join("-+-", widths.Select(width => new string('-', width))));

        foreach (string[] row in rowList)
        {
            PrintRow(row, widths);
        }

        if (rowList.Count == 0)
        {
            ConsoleTheme.Warning("No records found.");
        }
    }

    private static void PrintRow(string[] values, int[] widths)
    {
        string line = string.Join(" | ", values.Select((value, index) => value.PadRight(widths[index])));
        Console.WriteLine(line);
    }
}
