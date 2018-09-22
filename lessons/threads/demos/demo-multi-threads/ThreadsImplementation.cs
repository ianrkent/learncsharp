using System.Linq;
using System.Threading;

namespace DemoMultiThreads
{
    public static class ThreadsImplementation
    {
        public static void DemoWithThreads()
        {
            var cooksAsThreads = Cooking.TheCooks
                .Select(name => new Thread(Cooking.GoCook) { Name = name })
                .ToList();

            foreach (var cookAsThread in cooksAsThreads)
            {
                cookAsThread.Start();
            }

            Cooking.GoCook();

            foreach (var childThread in cooksAsThreads)
            {
                childThread.Join(500);
            }
        }
    }
}