using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Utilities
{
    public class SimpsonMethod
    {
        static double Function(double x)
        {
            return x * x;
        }
        public static double SimpsonIntegral(double a, double b, int n)
        {
            double h = (b - a) / n;
            double sum = Function(a) + Function(b);

            for (int i = 1; i < n; i += 2)
            {
                sum += 4 * Function(a + i * h);
            }

            for (int i = 2; i < n - 1; i += 2)
            {
                sum += 2 * Function(a + i * h);
            }

            return sum * h / 3;
        }
    }
}
