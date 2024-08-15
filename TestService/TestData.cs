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

            public static readonly NumberInputModel ListNumberNegativeInput =
                new NumberInputModel { Numbers = new List<int> { -1, -2, -3 } };

            public static readonly NumberInputModel EmptyInput = new NumberInputModel { Numbers = new List<int>() };

            public static readonly NumberInputModel OneNumber = new NumberInputModel { Numbers = new List<int>() { 7 } };

            public static readonly NumberInputModel LargeInput =
                new NumberInputModel { Numbers = Enumerable.Range(1, 1000).ToList() };

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

            public static readonly CompoundInterstModel InvalidInterestRate = new CompoundInterstModel
            {
                StartSum = 1000,
                YearInterestRate = 12,
                YearsNumber = 10,
                NumberOfPeriods = 12,
                ifReinvestment = true
            };

            public static readonly CompoundInterstModel InvalidNumberOfPeriods = new CompoundInterstModel
            {
                StartSum = 1000,
                YearInterestRate = 0.05,
                YearsNumber = 10,
                NumberOfPeriods = -5,
                ifReinvestment = true
            };

            public static readonly CompoundInterstModel CompoundModelInput = new CompoundInterstModel
            {
                StartSum = 1000,
                YearInterestRate = 0.05,
                YearsNumber = 10,
                NumberOfPeriods = 12,
                ifReinvestment = false
            };

            public static readonly CompoundInterstModel CompoundModelBigNumbersInput = new CompoundInterstModel
            {
                StartSum = 1000000,
                YearInterestRate = 1,
                YearsNumber = 100,
                NumberOfPeriods = 12,
                ifReinvestment = false
            };

            public static readonly IntegralParametersModel InvalidIntervalIntegral = new IntegralParametersModel
            {
                StartInterval = 0,
                EndInterval = 10,
                IntervalsAmount = -5
            };
            public static readonly IntegralParametersModel SameintervalIntegral = new IntegralParametersModel
            { 
                StartInterval = 5, 
                EndInterval = 5, 
                IntervalsAmount = 10 
            };

        };

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

            public static readonly StringContainerModel Empty  = new StringContainerModel { FirstString = "", SecondString = "" };

            public static readonly StringContainerModel StringAndNull = new StringContainerModel 
            { FirstString = null, SecondString = "World" };

            public static readonly StringContainerModel Nulls = new StringContainerModel 
            { FirstString = null, SecondString = null };

        }
    }
}
