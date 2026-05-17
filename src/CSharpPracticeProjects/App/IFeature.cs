namespace CSharpPracticeProjects.App;

public interface IFeature
{
    string Name { get; }
    string Description { get; }
    Task RunAsync();
}
