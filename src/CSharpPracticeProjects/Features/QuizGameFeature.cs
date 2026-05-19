using CSharpPracticeProjects.App;
using CSharpPracticeProjects.Core;
using CSharpPracticeProjects.Models;

namespace CSharpPracticeProjects.Features;

public sealed class QuizGameFeature : IFeature
{
    private static readonly List<QuizQuestion> Questions = new()
    {
        new QuizQuestion("Which keyword is used to create a class in C#?", new[] { "class", "object", "structs", "define" }, 0, "The class keyword defines a class."),
        new QuizQuestion("Which type stores true or false values?", new[] { "int", "bool", "string", "double" }, 1, "bool stores Boolean values."),
        new QuizQuestion("What does OOP stand for?", new[] { "Only One Program", "Object-Oriented Programming", "Open Online Platform", "Object Output Process" }, 1, "OOP means Object-Oriented Programming."),
        new QuizQuestion("Which collection can grow dynamically?", new[] { "int", "array only", "List<T>", "char" }, 2, "List<T> can grow dynamically."),
        new QuizQuestion("What does async help with?", new[] { "Non-blocking operations", "Deleting code", "Changing colors", "Ignoring errors" }, 0, "async helps with asynchronous non-blocking work."),
        new QuizQuestion("Which file usually defines a .NET project?", new[] { ".csproj", ".txt", ".png", ".lock" }, 0, ".csproj contains project configuration."),
        new QuizQuestion("What does JSON commonly store?", new[] { "Images only", "Structured data", "Only passwords", "Compiled code" }, 1, "JSON is a structured data format."),
        new QuizQuestion("Which keyword prevents changing a local variable after assignment?", new[] { "const", "public", "namespace", "using" }, 0, "const declares a compile-time constant."),
    };

    public string Name => "C# Quiz Game";
    public string Description => "Practice beginner concepts";

    public Task RunAsync()
    {
        int score = 0;
        var selectedQuestions = Questions.OrderBy(_ => Guid.NewGuid()).Take(5).ToList();

        for (int i = 0; i < selectedQuestions.Count; i++)
        {
            QuizQuestion question = selectedQuestions[i];
            ConsoleTheme.Section($"Question {i + 1} of {selectedQuestions.Count}");
            Console.WriteLine(question.Question);

            for (int option = 0; option < question.Options.Length; option++)
            {
                Console.WriteLine($"{option + 1}. {question.Options[option]}");
            }

            int answer = InputReader.ReadInt("Answer", 1, question.Options.Length) - 1;
            if (answer == question.CorrectOptionIndex)
            {
                score++;
                ConsoleTheme.Success("Correct!");
            }
            else
            {
                ConsoleTheme.Error("Wrong answer.");
            }

            ConsoleTheme.Info(question.Explanation);
        }

        ConsoleTheme.Divider();
        ConsoleTheme.KeyValue("Final score", $"{score}/{selectedQuestions.Count}");
        InputReader.Pause();
        return Task.CompletedTask;
    }
}
