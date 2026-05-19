using CSharpPracticeProjects.App;
using CSharpPracticeProjects.Core;
using CSharpPracticeProjects.Models;

namespace CSharpPracticeProjects.Features;

public sealed class ExpenseTrackerFeature : IFeature
{
    private readonly JsonRepository<Expense> _repository = new("expenses.json");

    public string Name => "Expense Tracker";
    public string Description => "Track spending and summaries";

    public async Task RunAsync()
    {
        while (true)
        {
            ConsoleTheme.Section("Expense Menu");
            ConsoleTheme.MenuItem(1, "Add Expense", "Save a new expense");
            ConsoleTheme.MenuItem(2, "List Expenses", "View expenses");
            ConsoleTheme.MenuItem(3, "Summary", "Total by category");
            ConsoleTheme.MenuItem(4, "Delete", "Remove an expense");
            ConsoleTheme.MenuItem(0, "Back", "Return to main menu");

            int option = InputReader.ReadInt("Choose", 0, 4);
            if (option == 0) return;

            if (option == 1) await AddAsync();
            if (option == 2) await ListAsync();
            if (option == 3) await SummaryAsync();
            if (option == 4) await DeleteAsync();
        }
    }

    private async Task AddAsync()
    {
        var expenses = await _repository.LoadAsync();
        var expense = new Expense
        {
            Category = InputReader.ReadRequiredString("Category"),
            Amount = InputReader.ReadDecimal("Amount", 1, 10000000),
            Date = InputReader.ReadOptionalDate("Date") ?? DateTime.Today,
            Note = InputReader.ReadOptionalString("Note")
        };

        expenses.Add(expense);
        await _repository.SaveAsync(expenses);
        ConsoleTheme.Success("Expense saved.");
        InputReader.Pause();
    }

    private async Task ListAsync()
    {
        var expenses = await _repository.LoadAsync();
        Print(expenses.OrderByDescending(expense => expense.Date));
        InputReader.Pause();
    }

    private async Task SummaryAsync()
    {
        var expenses = await _repository.LoadAsync();
        decimal total = expenses.Sum(expense => expense.Amount);
        ConsoleTheme.KeyValue("Total", total.ToString("0.00"));

        TextTable.Print(
            new[] { "Category", "Count", "Amount" },
            expenses.GroupBy(expense => expense.Category, StringComparer.OrdinalIgnoreCase)
                .OrderByDescending(group => group.Sum(expense => expense.Amount))
                .Select(group => new[] { group.Key, group.Count().ToString(), group.Sum(expense => expense.Amount).ToString("0.00") }));

        InputReader.Pause();
    }

    private async Task DeleteAsync()
    {
        var expenses = await _repository.LoadAsync();
        Print(expenses);
        string id = InputReader.ReadRequiredString("Enter expense id prefix", 32);
        Expense? expense = expenses.FirstOrDefault(item => item.Id.ToString("N").StartsWith(id, StringComparison.OrdinalIgnoreCase));

        if (expense is null)
        {
            ConsoleTheme.Error("Expense not found.");
        }
        else if (InputReader.Confirm($"Delete {expense.Category} expense"))
        {
            expenses.Remove(expense);
            await _repository.SaveAsync(expenses);
            ConsoleTheme.Success("Expense deleted.");
        }

        InputReader.Pause();
    }

    private static void Print(IEnumerable<Expense> expenses)
    {
        TextTable.Print(
            new[] { "Id", "Date", "Category", "Amount", "Note" },
            expenses.Select(expense => new[]
            {
                expense.Id.ToString("N")[..8],
                expense.Date.ToString("yyyy-MM-dd"),
                expense.Category,
                expense.Amount.ToString("0.00"),
                expense.Note
            }));
    }
}
