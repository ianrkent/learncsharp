using System;
using System.Linq;

namespace DotnetFrameworkConfigurationLab
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine($"The database connection string is { ConfigurationRead.GetDatabaseConnectionString()}");
            Console.WriteLine($"The minimum abount for a delivery order is {  ConfigurationRead.GetMinimumDeliveryOrderValue() }");

            foreach (var loyaltyLevel in new[] { "Gold", "Silver", "Bronze" })
            {
                Console.WriteLine($"You become { loyaltyLevel } when you reach { ConfigurationRead.GetMinimumPointsForLoyaltyLevel(loyaltyLevel) } points");
            }

            foreach (var dayOfWeek in Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>())
            {
                var (openFrom, openTo) = ConfigurationRead.GetOpeningTimesForDay(dayOfWeek);
                Console.WriteLine($"Open from { openFrom } to { openTo }");
            }

            Console.ReadKey();
        }
    }
}
