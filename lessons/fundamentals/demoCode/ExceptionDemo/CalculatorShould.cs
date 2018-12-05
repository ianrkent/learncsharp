using System;
using NUnit.Framework;

namespace apprentice_learncsharp_2018_12
{
    [TestFixture()]
    public class CalculatorShould
    {

        [Test]
        public void SumAnArrayOfNumbers()
        {
            var numbersToSum = new[] {1, 1, 1, 1, 1};
            const int expectedSum = 5;

            var calcuator = new Calcuator();
            var calculatedSum = calcuator.SumNumbers(numbersToSum);

            Assert.AreEqual(expectedSum, calculatedSum);
        }
    }

    public class Calcuator
    {
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
