using CSharpPracticeProjects.Services;

namespace CSharpPracticeProjects.Tests;

public sealed class NumberToolkitTests
{
    private readonly NumberToolkit _toolkit = new();

    [Theory]
    [InlineData(2, true)]
    [InlineData(17, true)]
    [InlineData(21, false)]
    [InlineData(1, false)]
    public void IsPrime_ReturnsExpectedResult(long number, bool expected)
    {
        Assert.Equal(expected, _toolkit.IsPrime(number));
    }

    [Fact]
    public void GreatestCommonDivisor_ReturnsExpectedResult()
    {
        Assert.Equal(6, _toolkit.GreatestCommonDivisor(48, 18));
    }
}
