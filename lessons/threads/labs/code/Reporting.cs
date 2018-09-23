using System;
using System.Threading;

namespace ThreadingLab
{
    public static class Reporting
    {
        public static void Report(string message)
        {
            Console.WriteLine($"Thread Id: {Thread.CurrentThread.ManagedThreadId } -  { message }");
        }

        public static void ReportInHighlight(string message)
        {
            Console.WriteLine($"**** THREAD ID : {Thread.CurrentThread.ManagedThreadId } -  { message }  ****");
        }
    }
}