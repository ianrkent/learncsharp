using System;
using System.Collections.Generic;

namespace ThreadingLab
{
    public static class Algorithm
    {
        public static bool IsPrime(long number)
        {
            if (number <= 1) return false;
            if (number == 2) return true;
            if (number % 2 == 0) return false;

            var boundary = (int)Math.Floor(Math.Sqrt(number));

            for (var i = 3; i <= boundary; i += 2)
            {
                if (number % i == 0) return false;
            }

            return true;
        }

        public static bool IsFibonacci(long number)
        {
            if (number <= 0) return false;
            if (number == 1) return true;
            if (number == 2) return true;

            long a = 1, b = 2, c = a + b;

            while (c <= number)
            {
                if (c == number) return true;
                a = b;
                b = c;
                c = a + b;
            }

            return false;
        }


        public static IEnumerable<long> LongRange(long highValue)
        {
            for (long i = 0; i <= highValue; i++)
            {
                yield return i;
            }
        }
    }
}