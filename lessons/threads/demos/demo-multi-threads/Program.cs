using System;
using System.Linq;
using System.Threading;

namespace DemoMultiThreads
{
    public class Program
    {
        static void Main(string[] args)
        {
            Thread.CurrentThread.Name = "Head Chef";

            ThreadsImplementation.DemoWithThreads();

            Console.ReadLine();
        }
    }
}
