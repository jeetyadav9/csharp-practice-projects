using CSharpPracticeProjects.Models;
using CSharpPracticeProjects.Services;

namespace CSharpPracticeProjects.Tests;

public sealed class PasswordGeneratorTests
{
    [Fact]
    public void Generate_ReturnsPasswordWithRequestedLength()
    {
        var generator = new PasswordGenerator();
        string password = generator.Generate(new PasswordOptions(Length: 20));

        Assert.Equal(20, password.Length);
    }

    [Fact]
    public void Generate_ThrowsWhenNoCharacterGroupsAreSelected()
    {
        var generator = new PasswordGenerator();
        var options = new PasswordOptions(12, false, false, false, false);

        Assert.Throws<InvalidOperationException>(() => generator.Generate(options));
    }
}
