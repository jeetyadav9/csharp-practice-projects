using CSharpPracticeProjects.App;
using CSharpPracticeProjects.Core;
using CSharpPracticeProjects.Models;

namespace CSharpPracticeProjects.Features;

public sealed class NotesFeature : IFeature
{
    private readonly JsonRepository<Note> _repository = new("notes.json");

    public string Name => "Notes Manager";
    public string Description => "Write and search notes";

    public async Task RunAsync()
    {
        while (true)
        {
            ConsoleTheme.Section("Notes Menu");
            ConsoleTheme.MenuItem(1, "Add Note", "Write a new note");
            ConsoleTheme.MenuItem(2, "List Notes", "View note titles");
            ConsoleTheme.MenuItem(3, "View Note", "Read full note");
            ConsoleTheme.MenuItem(4, "Search", "Search notes");
            ConsoleTheme.MenuItem(5, "Delete", "Remove note");
            ConsoleTheme.MenuItem(0, "Back", "Return to main menu");

            int option = InputReader.ReadInt("Choose", 0, 5);
            if (option == 0) return;

            if (option == 1) await AddAsync();
            if (option == 2) await ListAsync();
            if (option == 3) await ViewAsync();
            if (option == 4) await SearchAsync();
            if (option == 5) await DeleteAsync();
        }
    }

    private async Task AddAsync()
    {
        var notes = await _repository.LoadAsync();
        var note = new Note
        {
            Title = InputReader.ReadRequiredString("Title"),
            Content = InputReader.ReadMultiline("Note content"),
            Tags = ParseTags(InputReader.ReadOptionalString("Tags comma separated"))
        };

        notes.Add(note);
        await _repository.SaveAsync(notes);
        ConsoleTheme.Success("Note saved.");
        InputReader.Pause();
    }

    private async Task ListAsync()
    {
        var notes = await _repository.LoadAsync();
        Print(notes.OrderByDescending(note => note.UpdatedAt));
        InputReader.Pause();
    }

    private async Task ViewAsync()
    {
        var notes = await _repository.LoadAsync();
        Print(notes);
        string id = InputReader.ReadRequiredString("Enter note id prefix", 32);
        Note? note = notes.FirstOrDefault(item => item.Id.ToString("N").StartsWith(id, StringComparison.OrdinalIgnoreCase));

        if (note is null)
        {
            ConsoleTheme.Error("Note not found.");
        }
        else
        {
            ConsoleTheme.Section(note.Title);
            Console.WriteLine(note.Content);
            ConsoleTheme.Divider();
            ConsoleTheme.KeyValue("Tags", string.Join(", ", note.Tags));
            ConsoleTheme.KeyValue("Updated", note.UpdatedAt.ToLocalTime());
        }

        InputReader.Pause();
    }

    private async Task SearchAsync()
    {
        string keyword = InputReader.ReadRequiredString("Keyword");
        var notes = await _repository.LoadAsync();
        var results = notes.Where(note =>
            note.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
            note.Content.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
            note.Tags.Any(tag => tag.Contains(keyword, StringComparison.OrdinalIgnoreCase)));

        Print(results);
        InputReader.Pause();
    }

    private async Task DeleteAsync()
    {
        var notes = await _repository.LoadAsync();
        Print(notes);
        string id = InputReader.ReadRequiredString("Enter note id prefix", 32);
        Note? note = notes.FirstOrDefault(item => item.Id.ToString("N").StartsWith(id, StringComparison.OrdinalIgnoreCase));

        if (note is null)
        {
            ConsoleTheme.Error("Note not found.");
        }
        else if (InputReader.Confirm($"Delete '{note.Title}'"))
        {
            notes.Remove(note);
            await _repository.SaveAsync(notes);
            ConsoleTheme.Success("Note deleted.");
        }

        InputReader.Pause();
    }

    private static List<string> ParseTags(string input)
    {
        return input.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    private static void Print(IEnumerable<Note> notes)
    {
        TextTable.Print(
            new[] { "Id", "Updated", "Title", "Tags" },
            notes.Select(note => new[]
            {
                note.Id.ToString("N")[..8],
                note.UpdatedAt.ToLocalTime().ToString("yyyy-MM-dd"),
                note.Title,
                string.Join(", ", note.Tags)
            }));
    }
}
