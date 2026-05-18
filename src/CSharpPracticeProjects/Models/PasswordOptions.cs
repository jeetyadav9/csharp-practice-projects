namespace CSharpPracticeProjects.Models;

public sealed record PasswordOptions(
    int Length = 16,
    bool IncludeUppercase = true,
    bool IncludeLowercase = true,
    bool IncludeNumbers = true,
    bool IncludeSymbols = true,
    bool AvoidAmbiguousCharacters = true);
