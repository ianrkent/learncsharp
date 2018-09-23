using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadingLab
{
    class Program
    {
        private const uint HighValue = 3_000_000_000;

        static void Main(string[] args)
        {
            SingleThreadCalculation();
            Console.ReadKey();
        }

        private static void SingleThreadCalculation() {
            var stopwatch = new Stopwatch();
            var testNumbers = Algorithm.LongRange(HighValue);
            var countOfNumbersEvaluated = 0L;

            stopwatch.Start();

            foreach (var testNumber in testNumbers)
            {
                if (Algorithm.IsPrime(testNumber) && Algorithm.IsFibonacci(testNumber))
                {
                    Reporting.ReportInHighlight(testNumber.ToString());
                }

                countOfNumbersEvaluated++;
            }

            stopwatch.Stop();

            OutputSummary(countOfNumbersEvaluated, stopwatch.ElapsedMilliseconds);
        }

        private static void OutputSummary(long countEvaluated, long elapsedMilliseconds)
        {
            Reporting.ReportInHighlight("");
            Reporting.ReportInHighlight($"\t Evaluated { countEvaluated } numbers in { elapsedMilliseconds } milliseconds");
            Reporting.ReportInHighlight($"\t at an average rate of { countEvaluated * 1.0 / elapsedMilliseconds } per millisecond");
            Reporting.ReportInHighlight("");
        }
    }
}
