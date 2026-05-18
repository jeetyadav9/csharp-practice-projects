using System.Security.Cryptography;
using CSharpPracticeProjects.Models;

namespace CSharpPracticeProjects.Services;

public sealed class PasswordGenerator
{
    private const string Uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string Lowercase = "abcdefghijklmnopqrstuvwxyz";
    private const string Numbers = "0123456789";
    private const string Symbols = "!@#$%^&*_-+=?";
    private const string Ambiguous = "O0Il1";

    public string Generate(PasswordOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        if (options.Length < 8 || options.Length > 128)
        {
            throw new ArgumentOutOfRangeException(nameof(options), "Password length must be between 8 and 128.");
        }

        var groups = new List<string>();
        if (options.IncludeUppercase) groups.Add(Filter(Uppercase, options.AvoidAmbiguousCharacters));
        if (options.IncludeLowercase) groups.Add(Filter(Lowercase, options.AvoidAmbiguousCharacters));
        if (options.IncludeNumbers) groups.Add(Filter(Numbers, options.AvoidAmbiguousCharacters));
        if (options.IncludeSymbols) groups.Add(Symbols);

        if (groups.Count == 0)
        {
            throw new InvalidOperationException("At least one character group must be selected.");
        }

        var characters = new List<char>();
        foreach (string group in groups)
        {
            characters.Add(PickRandom(group));
        }

        string allCharacters = string.Concat(groups);
        while (characters.Count < options.Length)
        {
            characters.Add(PickRandom(allCharacters));
        }

        return new string(characters.OrderBy(_ => RandomNumberGenerator.GetInt32(int.MaxValue)).ToArray());
    }

    private static string Filter(string characters, bool avoidAmbiguous)
    {
        return avoidAmbiguous
            ? new string(characters.Where(character => !Ambiguous.Contains(character)).ToArray())
            : characters;
    }

    private static char PickRandom(string characters)
    {
        if (string.IsNullOrEmpty(characters))
        {
            throw new InvalidOperationException("Character group is empty.");
        }

        int index = RandomNumberGenerator.GetInt32(characters.Length);
        return characters[index];
    }
}
