using System.Linq;
using System.Threading.Tasks;

namespace DemoMultiThreads
{
    public static class TasksImplementation
    {
        public static void DemoWithTasks()
        {
            var cooksAsTasks = Cooking.TheCooks
                .Select(name => Task.Factory.StartNew(Cooking.GoCook))
                .ToArray();

            Cooking.GoCook();

            Task.WaitAll(cooksAsTasks);
        }
    }
}