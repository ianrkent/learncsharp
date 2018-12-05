using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp.Lamda
{
    public class LinqDemo
    {
        public void Go()
        {
            IEnumerable<int> myInts = new List<int> {1, 2, 3, 4, 5, 4, 3, 2};

            Price[] euroPrices = myInts
                .Cast<double>()
                .Where(num => num % 2 == 0)
                .Select(num =>
                {
                    var p = new Price(num);
                    p.ConvertToEuro();
                    return p;
                })
                .ToArray();
        }

        public class Price
        {
            private readonly double _value;

            public Price(double value)
            {
                _value = value;
            }

            public void ConvertToEuro()
            {
                throw new NotImplementedException();
            }
        }
    }
}