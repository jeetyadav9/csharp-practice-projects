namespace CSharpPracticeProjects.Models;

public sealed record QuizQuestion(
    string Question,
    string[] Options,
    int CorrectOptionIndex,
    string Explanation);
