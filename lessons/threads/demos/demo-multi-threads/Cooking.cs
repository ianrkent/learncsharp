using System;
using System.Linq;
using System.Threading;

namespace DemoMultiThreads
{
    public static class Cooking
    {
        public static readonly string[] TheCooks = { "Billy", "Bob", "Mary", "Fiona", "Mad-Max" };

        public static void GoCook()
        {
            foreach (var counter in Enumerable.Range(1, 1000))
            {
                if (counter % 50 == 0)
                {
                    Console.WriteLine($"{ Thread.CurrentThread.ManagedThreadId } - { Thread.CurrentThread.Name } : { counter }");
                }
            }
        }
    }
}