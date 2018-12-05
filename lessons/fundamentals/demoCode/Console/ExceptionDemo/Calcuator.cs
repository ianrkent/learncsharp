using System;

namespace ConsoleApp.ExceptionDemo
{
    public class Calcuator
    {
        public static void DemoExceptions()
        {
            var numbersToSum = new[] { 1, 1, 1, 1, 1 };

            var calcuator = new Calcuator();
            var calculatedSum = calcuator.SumNumbers(numbersToSum);
            System.Console.WriteLine($"The sum is {calculatedSum}");
        }

        public double SumNumbers(int[] data)
        {
            var sum = 0;
            for (var i = 1; i <= data.Length; i++)
            {
                sum += data[i];
            }

            return sum;
        }
    }
}