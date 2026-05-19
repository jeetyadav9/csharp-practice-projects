using CSharpPracticeProjects.App;
using CSharpPracticeProjects.Core;
using CSharpPracticeProjects.Models;

namespace CSharpPracticeProjects.Features;

public sealed class TodoFeature : IFeature
{
    private readonly JsonRepository<TodoItem> _repository = new("todos.json");

    public string Name => "To-do Manager";
    public string Description => "Tasks with priority and due dates";

    public async Task RunAsync()
    {
        while (true)
        {
            ConsoleTheme.Section("To-do Menu");
            ConsoleTheme.MenuItem(1, "Add Task", "Create a new task");
            ConsoleTheme.MenuItem(2, "List Tasks", "View all tasks");
            ConsoleTheme.MenuItem(3, "Search", "Find tasks by keyword");
            ConsoleTheme.MenuItem(4, "Toggle Complete", "Mark done or not done");
            ConsoleTheme.MenuItem(5, "Delete", "Remove a task");
            ConsoleTheme.MenuItem(0, "Back", "Return to main menu");

            int option = InputReader.ReadInt("Choose", 0, 5);
            if (option == 0) return;

            if (option == 1) await AddAsync();
            if (option == 2) await ListAsync();
            if (option == 3) await SearchAsync();
            if (option == 4) await ToggleAsync();
            if (option == 5) await DeleteAsync();
        }
    }

    private async Task AddAsync()
    {
        var items = await _repository.LoadAsync();
        var item = new TodoItem
        {
            Title = InputReader.ReadRequiredString("Title"),
            Description = InputReader.ReadOptionalString("Description"),
            Priority = (TodoPriority)InputReader.ReadInt("Priority: 1 Low, 2 Medium, 3 High", 1, 3),
            DueDate = InputReader.ReadOptionalDate("Due date")
        };

        items.Add(item);
        await _repository.SaveAsync(items);
        ConsoleTheme.Success("Task added.");
        InputReader.Pause();
    }

    private async Task ListAsync()
    {
        var items = await _repository.LoadAsync();
        Print(items.OrderBy(item => item.IsCompleted).ThenByDescending(item => item.Priority).ThenBy(item => item.DueDate ?? DateTime.MaxValue));
        InputReader.Pause();
    }

    private async Task SearchAsync()
    {
        string keyword = InputReader.ReadRequiredString("Keyword").ToLowerInvariant();
        var items = await _repository.LoadAsync();
        var results = items.Where(item => item.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase) || item.Description.Contains(keyword, StringComparison.OrdinalIgnoreCase));
        Print(results);
        InputReader.Pause();
    }

    private async Task ToggleAsync()
    {
        var items = await _repository.LoadAsync();
        Print(items);
        string id = InputReader.ReadRequiredString("Enter task id prefix", 32);
        TodoItem? item = FindByPrefix(items, id);

        if (item is null)
        {
            ConsoleTheme.Error("Task not found.");
        }
        else
        {
            item.IsCompleted = !item.IsCompleted;
            item.CompletedAt = item.IsCompleted ? DateTime.UtcNow : null;
            await _repository.SaveAsync(items);
            ConsoleTheme.Success("Task updated.");
        }

        InputReader.Pause();
    }

    private async Task DeleteAsync()
    {
        var items = await _repository.LoadAsync();
        Print(items);
        string id = InputReader.ReadRequiredString("Enter task id prefix", 32);
        TodoItem? item = FindByPrefix(items, id);

        if (item is null)
        {
            ConsoleTheme.Error("Task not found.");
        }
        else if (InputReader.Confirm($"Delete '{item.Title}'"))
        {
            items.Remove(item);
            await _repository.SaveAsync(items);
            ConsoleTheme.Success("Task deleted.");
        }

        InputReader.Pause();
    }

    private static TodoItem? FindByPrefix(IEnumerable<TodoItem> items, string idPrefix)
    {
        return items.FirstOrDefault(item => item.Id.ToString("N").StartsWith(idPrefix, StringComparison.OrdinalIgnoreCase));
    }

    private static void Print(IEnumerable<TodoItem> items)
    {
        TextTable.Print(
            new[] { "Id", "Status", "Priority", "Due", "Title" },
            items.Select(item => new[]
            {
                item.Id.ToString("N")[..8],
                item.IsCompleted ? "Done" : "Open",
                item.Priority.ToString(),
                item.DueDate?.ToString("yyyy-MM-dd") ?? "-",
                item.Title
            }));
    }
}
