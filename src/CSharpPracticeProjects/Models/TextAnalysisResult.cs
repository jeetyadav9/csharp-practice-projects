namespace CSharpPracticeProjects.Models;

public sealed record TextAnalysisResult(
    int Characters,
    int Letters,
    int Digits,
    int Words,
    int Sentences,
    int Lines,
    double AverageWordLength,
    IReadOnlyList<KeyValuePair<string, int>> TopWords);
