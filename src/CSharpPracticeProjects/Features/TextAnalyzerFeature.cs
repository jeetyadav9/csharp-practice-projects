using CSharpPracticeProjects.App;
using CSharpPracticeProjects.Core;
using CSharpPracticeProjects.Services;

namespace CSharpPracticeProjects.Features;

public sealed class TextAnalyzerFeature : IFeature
{
    private readonly TextAnalyzer _textAnalyzer;

    public TextAnalyzerFeature(TextAnalyzer textAnalyzer)
    {
        _textAnalyzer = textAnalyzer;
    }

    public string Name => "Text Analyzer";
    public string Description => "Analyze words and characters";

    public Task RunAsync()
    {
        string text = InputReader.ReadMultiline("Enter text to analyze");
        var result = _textAnalyzer.Analyze(text);

        ConsoleTheme.Section("Analysis Result");
        ConsoleTheme.KeyValue("Characters", result.Characters);
        ConsoleTheme.KeyValue("Letters", result.Letters);
        ConsoleTheme.KeyValue("Digits", result.Digits);
        ConsoleTheme.KeyValue("Words", result.Words);
        ConsoleTheme.KeyValue("Sentences", result.Sentences);
        ConsoleTheme.KeyValue("Lines", result.Lines);
        ConsoleTheme.KeyValue("Average word length", result.AverageWordLength);

        ConsoleTheme.Section("Top Words");
        foreach (var pair in result.TopWords)
        {
            Console.WriteLine($"{pair.Key}: {pair.Value}");
        }

        InputReader.Pause();
        return Task.CompletedTask;
    }
}
