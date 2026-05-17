using System.Text;
using CSharpPracticeProjects.App;
using CSharpPracticeProjects.Features;
using CSharpPracticeProjects.Services;

Console.OutputEncoding = Encoding.UTF8;
Console.Title = "C# Practice Projects - Jeet Yadav";

var expressionCalculator = new ExpressionCalculator();
var textAnalyzer = new TextAnalyzer();
var passwordGenerator = new PasswordGenerator();
var numberToolkit = new NumberToolkit();

var features = new List<IFeature>
{
    new DeveloperProfileFeature(),
    new CalculatorFeature(expressionCalculator),
    new TodoFeature(),
    new ExpenseTrackerFeature(),
    new ContactBookFeature(),
    new NotesFeature(),
    new PasswordGeneratorFeature(passwordGenerator),
    new QuizGameFeature(),
    new TextAnalyzerFeature(textAnalyzer),
    new NumberToolkitFeature(numberToolkit)
};

var app = new AppRunner(features);
await app.RunAsync();
