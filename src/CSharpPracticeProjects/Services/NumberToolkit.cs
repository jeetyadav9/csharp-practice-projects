using System.Numerics;

namespace CSharpPracticeProjects.Services;

public sealed class NumberToolkit
{
    public bool IsPrime(long number)
    {
        if (number < 2) return false;
        if (number == 2) return true;
        if (number % 2 == 0) return false;

        long limit = (long)Math.Sqrt(number);
        for (long divisor = 3; divisor <= limit; divisor += 2)
        {
            if (number % divisor == 0)
            {
                return false;
            }
        }

        return true;
    }

    public BigInteger Factorial(int number)
    {
        if (number < 0 || number > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(number), "Factorial supports numbers from 0 to 100.");
        }

        BigInteger result = BigInteger.One;
        for (int i = 2; i <= number; i++)
        {
            result *= i;
        }

        return result;
    }

    public BigInteger Fibonacci(int index)
    {
        if (index < 0 || index > 500)
        {
            throw new ArgumentOutOfRangeException(nameof(index), "Fibonacci supports indexes from 0 to 500.");
        }

        BigInteger previous = BigInteger.Zero;
        BigInteger current = BigInteger.One;

        for (int i = 0; i < index; i++)
        {
            (previous, current) = (current, previous + current);
        }

        return previous;
    }

    public long GreatestCommonDivisor(long a, long b)
    {
        a = Math.Abs(a);
        b = Math.Abs(b);

        while (b != 0)
        {
            long remainder = a % b;
            a = b;
            b = remainder;
        }

        return a;
    }

    public long LeastCommonMultiple(long a, long b)
    {
        if (a == 0 || b == 0) return 0;
        return Math.Abs(a / GreatestCommonDivisor(a, b) * b);
    }
}
