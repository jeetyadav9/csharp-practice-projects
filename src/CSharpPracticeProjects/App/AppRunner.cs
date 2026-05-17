using CSharpPracticeProjects.Core;

namespace CSharpPracticeProjects.App;

public sealed class AppRunner
{
    private readonly IReadOnlyList<IFeature> _features;

    public AppRunner(IReadOnlyList<IFeature> features)
    {
        _features = features;
    }

    public async Task RunAsync()
    {
        while (true)
        {
            ConsoleTheme.Clear();
            ConsoleTheme.Banner("C# Practice Projects v2", "Built for Jeet Yadav");
            ConsoleTheme.Info("Choose a module to practice real C# skills.");
            ConsoleTheme.Divider();

            for (int i = 0; i < _features.Count; i++)
            {
                ConsoleTheme.MenuItem(i + 1, _features[i].Name, _features[i].Description);
            }

            ConsoleTheme.MenuItem(0, "Exit", "Close the application");
            ConsoleTheme.Divider();

            int choice = InputReader.ReadInt("Enter option", 0, _features.Count);
            if (choice == 0)
            {
                ConsoleTheme.Success("Thanks for using C# Practice Projects. Keep coding, Jeet!");
                return;
            }

            var selectedFeature = _features[choice - 1];

            try
            {
                ConsoleTheme.Clear();
                ConsoleTheme.Section(selectedFeature.Name);
                await selectedFeature.RunAsync();
            }
            catch (Exception ex)
            {
                ConsoleTheme.Error($"Something went wrong: {ex.Message}");
                InputReader.Pause();
            }
        }
    }
}
