using System;
using Microsoft.Extensions.Configuration;

namespace DotnetCoreConfigurationLab
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ReadLine();
        }

        private static void DisplayConfig(IConfiguration config)
        {
            Console.WriteLine(
                $"Minimum order amount : { config["minOrderAmountForDelivery"] } \n" +
                $"The DB connection string : { config["theDatabaseConnectionString"] }");
        }
    }
}
