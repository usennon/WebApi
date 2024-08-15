using Entities.TransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestService
{
    internal static class TestData
    {
        internal static class MathData
        {
            public static readonly IntegralParametersModel DefaultIntegralParameters
                = new IntegralParametersModel { StartInterval = 0, EndInterval = 1, IntervalsAmount = 10 };

            public static readonly NumberInputModel ListNumberInputSum
                = new NumberInputModel { Numbers = new List<int> { 2, 3, 4 } };

            public static readonly NumberInputModel ListNumbersInputAverage
                = new NumberInputModel { Numbers = new List<int> { 2, 3, 6 } };

            public static readonly NumberInputModel NumbersToSum
                = new NumberInputModel { FirstNumber = 3, SecondNumber = 5 };

            public static readonly NumberInputModel NumbersToSub
                = new NumberInputModel { FirstNumber = 10, SecondNumber = 3 };

            public static readonly CompoundInterstModel DefaultCompoundInterest = new CompoundInterstModel
            {
                StartSum = 1000,
                YearInterestRate = 0.05,
                YearsNumber = 10,
                NumberOfPeriods = 12,
                ifReinvestment = true
            };
        }

        internal static class TextData
        {
            public static readonly StringContainerModel TwoNonEmptyStrings = new StringContainerModel
            {
                FirstString = "Hello, ",
                SecondString = "World!"
            };
            public static readonly StringContainerModel OneEmptyStringAndNonEmpty = new StringContainerModel
            {
                FirstString = "",
                SecondString = "World!"
            };

        }
    }
}
