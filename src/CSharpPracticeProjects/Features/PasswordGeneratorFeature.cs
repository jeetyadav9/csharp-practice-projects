using CSharpPracticeProjects.App;
using CSharpPracticeProjects.Core;
using CSharpPracticeProjects.Models;
using CSharpPracticeProjects.Services;

namespace CSharpPracticeProjects.Features;

public sealed class PasswordGeneratorFeature : IFeature
{
    private readonly PasswordGenerator _passwordGenerator;

    public PasswordGeneratorFeature(PasswordGenerator passwordGenerator)
    {
        _passwordGenerator = passwordGenerator;
    }

    public string Name => "Password Generator";
    public string Description => "Create secure local passwords";

    public Task RunAsync()
    {
        int length = InputReader.ReadInt("Password length", 8, 64);
        bool uppercase = InputReader.Confirm("Include uppercase letters");
        bool lowercase = InputReader.Confirm("Include lowercase letters");
        bool numbers = InputReader.Confirm("Include numbers");
        bool symbols = InputReader.Confirm("Include symbols");
        bool avoidAmbiguous = InputReader.Confirm("Avoid confusing characters like O, 0, I, l, 1");

        var options = new PasswordOptions(length, uppercase, lowercase, numbers, symbols, avoidAmbiguous);

        try
        {
            ConsoleTheme.Section("Generated passwords");
            for (int i = 1; i <= 5; i++)
            {
                Console.WriteLine($"{i}. {_passwordGenerator.Generate(options)}");
            }
        }
        catch (Exception ex)
        {
            ConsoleTheme.Error(ex.Message);
        }

        InputReader.Pause();
        return Task.CompletedTask;
    }
}
