using System;
using System.Linq;

namespace ConsoleApp.LamdaDemo
{
    public class LamdaDemo
    {
        public void DoLamdaDemo()
        {
            const int price = 400;
            Console.WriteLine($"Total additional costs on a price of { price} is { GetTotalAdditionalCosts(price) }");
        }

        public double GetTotalAdditionalCosts(double price)
        {
            // locally defined function
            double CalculateShippingCost(double orderValue) => orderValue > 100 ? 0 : 4.99;

            // local variable holding a fuction
            Func<double, double> CalculateVat = d => d * 0.021;

            // We can call any of these
            CalculateShippingCost(price);
            CalculateVat(price);
            CalculateAdminFee(price);

            // we can pass the functions around as parameters
            var totalAdditionalCosts = GetTotalAdditionalCosts(
                price,
                new[]                               // we are sending in an array of functions
                {
                    CalculateShippingCost,          // reference to the local method
                    CalculateVat,                   // local variable
                    CalculateAdminFee,              // method
                    p =>                            // anonymous function (Lamda)
                    {
                        var markupPercentage = GetMarkupPercentage(p);
                        return p * (markupPercentage / 100);
                    }
                });

            return totalAdditionalCosts;
        }

        private double GetTotalAdditionalCosts(double price, Func<double, double>[] additionalCostCalculators)
        {
            return additionalCostCalculators
                .Select(calculatingFunction => calculatingFunction(price))
                .Sum();
        }

        private double CalculateAdminFee(double price)
        {
            return 20;
        }

        private double GetMarkupPercentage(double price)
        {
            return 5; // 5 percent
        }
    }
}
