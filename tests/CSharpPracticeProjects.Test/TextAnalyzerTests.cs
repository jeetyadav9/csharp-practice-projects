using CSharpPracticeProjects.Services;

namespace CSharpPracticeProjects.Tests;

public sealed class TextAnalyzerTests
{
    [Fact]
    public void Analyze_CountsWordsAndTopWords()
    {
        var analyzer = new TextAnalyzer();
        var result = analyzer.Analyze("Hello world. Hello CSharp 2026!");

        Assert.Equal(5, result.Words);
        Assert.Equal(2, result.Sentences);
        Assert.Contains(result.TopWords, pair => pair.Key == "hello" && pair.Value == 2);
    }
}
