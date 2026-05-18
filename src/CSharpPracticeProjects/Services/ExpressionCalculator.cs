using System.Globalization;

namespace CSharpPracticeProjects.Services;

public sealed class ExpressionCalculator
{
    public double Evaluate(string expression)
    {
        if (string.IsNullOrWhiteSpace(expression))
        {
            throw new ArgumentException("Expression cannot be empty.", nameof(expression));
        }

        return new Parser(expression).Parse();
    }

    private sealed class Parser
    {
        private readonly string _text;
        private int _position;

        public Parser(string text)
        {
            _text = text;
        }

        public double Parse()
        {
            double value = ParseExpression();
            SkipWhiteSpace();

            if (!IsAtEnd)
            {
                throw new FormatException($"Unexpected character '{Current}' at position {_position}.");
            }

            return value;
        }

        private double ParseExpression()
        {
            double value = ParseTerm();

            while (true)
            {
                SkipWhiteSpace();

                if (Match('+'))
                {
                    value += ParseTerm();
                }
                else if (Match('-'))
                {
                    value -= ParseTerm();
                }
                else
                {
                    return value;
                }
            }
        }

        private double ParseTerm()
        {
            double value = ParsePower();

            while (true)
            {
                SkipWhiteSpace();

                if (Match('*'))
                {
                    value *= ParsePower();
                }
                else if (Match('/'))
                {
                    double divisor = ParsePower();
                    if (Math.Abs(divisor) < double.Epsilon)
                    {
                        throw new DivideByZeroException("Cannot divide by zero.");
                    }

                    value /= divisor;
                }
                else
                {
                    return value;
                }
            }
        }

        private double ParsePower()
        {
            double value = ParseUnary();
            SkipWhiteSpace();

            if (Match('^'))
            {
                value = Math.Pow(value, ParsePower());
            }

            return value;
        }

        private double ParseUnary()
        {
            SkipWhiteSpace();

            if (Match('+')) return ParseUnary();
            if (Match('-')) return -ParseUnary();

            return ParsePrimary();
        }

        private double ParsePrimary()
        {
            SkipWhiteSpace();

            if (Match('('))
            {
                double value = ParseExpression();
                SkipWhiteSpace();
                Require(')');
                return value;
            }

            if (char.IsLetter(Current))
            {
                string function = ParseIdentifier();
                SkipWhiteSpace();
                Require('(');
                double argument = ParseExpression();
                SkipWhiteSpace();
                Require(')');
                return ApplyFunction(function, argument);
            }

            return ParseNumber();
        }

        private double ParseNumber()
        {
            SkipWhiteSpace();
            int start = _position;

            while (!IsAtEnd && (char.IsDigit(Current) || Current == '.'))
            {
                _position++;
            }

            if (start == _position)
            {
                throw new FormatException($"Expected number at position {_position}.");
            }

            string text = _text[start.._position];
            if (!double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out double value))
            {
                throw new FormatException($"Invalid number '{text}'.");
            }

            return value;
        }

        private string ParseIdentifier()
        {
            int start = _position;

            while (!IsAtEnd && char.IsLetterOrDigit(Current))
            {
                _position++;
            }

            return _text[start.._position].ToLowerInvariant();
        }

        private static double ApplyFunction(string function, double value)
        {
            return function switch
            {
                "abs" => Math.Abs(value),
                "sqrt" => Math.Sqrt(value),
                "sin" => Math.Sin(value),
                "cos" => Math.Cos(value),
                "tan" => Math.Tan(value),
                "log" => Math.Log(value),
                "log10" => Math.Log10(value),
                "floor" => Math.Floor(value),
                "ceil" or "ceiling" => Math.Ceiling(value),
                "round" => Math.Round(value),
                _ => throw new NotSupportedException($"Function '{function}' is not supported.")
            };
        }

        private void Require(char expected)
        {
            if (!Match(expected))
            {
                throw new FormatException($"Expected '{expected}' at position {_position}.");
            }
        }

        private bool Match(char expected)
        {
            SkipWhiteSpace();
            if (Current != expected)
            {
                return false;
            }

            _position++;
            return true;
        }

        private void SkipWhiteSpace()
        {
            while (!IsAtEnd && char.IsWhiteSpace(Current))
            {
                _position++;
            }
        }

        private bool IsAtEnd => _position >= _text.Length;
        private char Current => IsAtEnd ? '\0' : _text[_position];
    }
}
