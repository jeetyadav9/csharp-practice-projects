using CSharpPracticeProjects.App;
using CSharpPracticeProjects.Core;

namespace CSharpPracticeProjects.Features;

public sealed class DeveloperProfileFeature : IFeature
{
    public string Name => "Developer Profile";
    public string Description => "Introduction and learning goals";

    public Task RunAsync()
    {
        ConsoleTheme.KeyValue("Name", "Jeet Yadav");
        ConsoleTheme.KeyValue("GitHub", "https://github.com/jeetyadav9");
        ConsoleTheme.KeyValue("Repository", "csharp-practice-projects");
        ConsoleTheme.KeyValue("Focus", "C#, .NET, backend development, and clean coding");
        ConsoleTheme.KeyValue("Goal", "Build useful projects and improve step by step");

        ConsoleTheme.Divider();
        ConsoleTheme.Info("This project is designed to show more than basic syntax. It demonstrates structure, persistence, validation, services, and tests.");
        InputReader.Pause();
        return Task.CompletedTask;
    }
}
