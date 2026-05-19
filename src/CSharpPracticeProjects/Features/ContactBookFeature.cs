using CSharpPracticeProjects.App;
using CSharpPracticeProjects.Core;
using CSharpPracticeProjects.Models;

namespace CSharpPracticeProjects.Features;

public sealed class ContactBookFeature : IFeature
{
    private readonly JsonRepository<Contact> _repository = new("contacts.json");

    public string Name => "Contact Book";
    public string Description => "Store searchable contacts";

    public async Task RunAsync()
    {
        while (true)
        {
            ConsoleTheme.Section("Contact Menu");
            ConsoleTheme.MenuItem(1, "Add Contact", "Create contact");
            ConsoleTheme.MenuItem(2, "List Contacts", "View all contacts");
            ConsoleTheme.MenuItem(3, "Search", "Find contacts");
            ConsoleTheme.MenuItem(4, "Delete", "Remove contact");
            ConsoleTheme.MenuItem(0, "Back", "Return to main menu");

            int option = InputReader.ReadInt("Choose", 0, 4);
            if (option == 0) return;

            if (option == 1) await AddAsync();
            if (option == 2) await ListAsync();
            if (option == 3) await SearchAsync();
            if (option == 4) await DeleteAsync();
        }
    }

    private async Task AddAsync()
    {
        var contacts = await _repository.LoadAsync();
        var contact = new Contact
        {
            Name = InputReader.ReadRequiredString("Name"),
            Email = InputReader.ReadOptionalString("Email"),
            Phone = InputReader.ReadOptionalString("Phone"),
            City = InputReader.ReadOptionalString("City"),
            Tags = ParseTags(InputReader.ReadOptionalString("Tags comma separated"))
        };

        contacts.Add(contact);
        await _repository.SaveAsync(contacts);
        ConsoleTheme.Success("Contact saved.");
        InputReader.Pause();
    }

    private async Task ListAsync()
    {
        var contacts = await _repository.LoadAsync();
        Print(contacts.OrderBy(contact => contact.Name));
        InputReader.Pause();
    }

    private async Task SearchAsync()
    {
        string keyword = InputReader.ReadRequiredString("Keyword");
        var contacts = await _repository.LoadAsync();
        var results = contacts.Where(contact =>
            contact.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
            contact.Email.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
            contact.Phone.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
            contact.City.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
            contact.Tags.Any(tag => tag.Contains(keyword, StringComparison.OrdinalIgnoreCase)));

        Print(results);
        InputReader.Pause();
    }

    private async Task DeleteAsync()
    {
        var contacts = await _repository.LoadAsync();
        Print(contacts);
        string id = InputReader.ReadRequiredString("Enter contact id prefix", 32);
        Contact? contact = contacts.FirstOrDefault(item => item.Id.ToString("N").StartsWith(id, StringComparison.OrdinalIgnoreCase));

        if (contact is null)
        {
            ConsoleTheme.Error("Contact not found.");
        }
        else if (InputReader.Confirm($"Delete {contact.Name}"))
        {
            contacts.Remove(contact);
            await _repository.SaveAsync(contacts);
            ConsoleTheme.Success("Contact deleted.");
        }

        InputReader.Pause();
    }

    private static List<string> ParseTags(string input)
    {
        return input.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    private static void Print(IEnumerable<Contact> contacts)
    {
        TextTable.Print(
            new[] { "Id", "Name", "Email", "Phone", "City", "Tags" },
            contacts.Select(contact => new[]
            {
                contact.Id.ToString("N")[..8],
                contact.Name,
                contact.Email,
                contact.Phone,
                contact.City,
                string.Join(", ", contact.Tags)
            }));
    }
}
