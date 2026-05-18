using System.Text.RegularExpressions;
using CSharpPracticeProjects.Models;

namespace CSharpPracticeProjects.Services;

public sealed class TextAnalyzer
{
    private static readonly Regex WordRegex = new("[A-Za-z0-9']+", RegexOptions.Compiled);

    public TextAnalysisResult Analyze(string text)
    {
        text ??= string.Empty;

        var words = WordRegex.Matches(text)
            .Select(match => match.Value.Trim('\''))
            .Where(word => !string.IsNullOrWhiteSpace(word))
            .ToList();

        var topWords = words
            .Select(word => word.ToLowerInvariant())
            .GroupBy(word => word)
            .OrderByDescending(group => group.Count())
            .ThenBy(group => group.Key)
            .Take(5)
            .Select(group => new KeyValuePair<string, int>(group.Key, group.Count()))
            .ToList();

        int sentences = text.Count(character => character is '.' or '!' or '?');
        int lines = string.IsNullOrEmpty(text) ? 0 : text.Split(Environment.NewLine).Length;
        int letters = text.Count(char.IsLetter);
        int digits = text.Count(char.IsDigit);
        double averageWordLength = words.Count == 0 ? 0 : words.Average(word => word.Length);

        return new TextAnalysisResult(
            Characters: text.Length,
            Letters: letters,
            Digits: digits,
            Words: words.Count,
            Sentences: sentences,
            Lines: lines,
            AverageWordLength: Math.Round(averageWordLength, 2),
            TopWords: topWords);
    }
}
