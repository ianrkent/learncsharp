using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using DemoMultiThreads_extensions;

// using DemoMultiThreads_extensions;

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


        private static IEnumerable<int> ToInfinityAndBeyound()
        { 
            int counter = 0;
            do
            {
                yield return counter++;
            } while (true);
        }
    }
}
