using System;
using System.Collections.Specialized;
using System.Configuration;

namespace DotnetFrameworkConfigurationLab
{
    public static class ConfigurationRead
    {
        public static string GetDatabaseConnectionString()
        {
            // fix this code
            return string.Empty;
        }

        public static decimal GetMinimumDeliveryOrderValue()
        {
            // fix this code
            return -1;
        }

        public static int? GetMinimumPointsForLoyaltyLevel(string level)
        {
            // fix this code
            return -1;
        }

        public static (string from, string to) GetOpeningTimesForDay(DayOfWeek day)
        {
            // fix this code
            return (string.Empty, string.Empty);
        }
    }
}
