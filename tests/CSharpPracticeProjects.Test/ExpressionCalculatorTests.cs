using CSharpPracticeProjects.Services;

namespace CSharpPracticeProjects.Tests;

public sealed class ExpressionCalculatorTests
{
    private readonly ExpressionCalculator _calculator = new();

    [Theory]
    [InlineData("2 + 3 * 4", 14)]
    [InlineData("(10 + 5) / 3", 5)]
    [InlineData("2 ^ 8", 256)]
    [InlineData("sqrt(144)", 12)]
    [InlineData("abs(-25)", 25)]
    public void Evaluate_ReturnsExpectedResult(string expression, double expected)
    {
        double result = _calculator.Evaluate(expression);
        Assert.Equal(expected, result, precision: 6);
    }

    [Fact]
    public void Evaluate_ThrowsForDivisionByZero()
    {
        Assert.Throws<DivideByZeroException>(() => _calculator.Evaluate("10 / 0"));
    }
}
