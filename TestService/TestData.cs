using Entities.TransferObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestService
{
    internal static class TestData
    {
        internal static class MathData
        {
            public static IEnumerable SumListInputCases
            {
                get 
                {
                    yield return new TestCaseData(ListNumberInputSum).Returns(9);
                    yield return new TestCaseData(new NumberInputModel { Numbers = new List<int> { -1, -2, -3 } }).Returns(-6);
                    yield return new TestCaseData(EmptyInput).Returns(0);
                    yield return new TestCaseData(LargeInput).Returns(LargeInput.Numbers.Sum());
                    yield return new TestCaseData(OneNumber).Returns(7);
                }
            }

            public static IEnumerable AverageListInputCases
            {
                get
                {
                    yield return new TestCaseData(ListNumberInputSum).Returns(3);
                    yield return new TestCaseData(OneNumber).Returns(7);
                    yield return new TestCaseData(LargeInput).Returns(LargeInput.Numbers.Average());
                }
            }

            public static readonly NumberInputModel ListNumberInputSum = new NumberInputModel { Numbers = new List<int> { 2, 3, 4 } };

            public static readonly NumberInputModel EmptyInput = new NumberInputModel { Numbers = new List<int>() };

            public static readonly NumberInputModel OneNumber = new NumberInputModel { Numbers = new List<int>() { 7 } };

            public static readonly NumberInputModel LargeInput =
                new NumberInputModel { Numbers = Enumerable.Range(1, 1000).ToList() };

            public static readonly NumberInputModel ListNumbersInputAverage
                = new NumberInputModel { Numbers = new List<int> { 2, 3, 6 } };

            public static IEnumerable InterestInputCases
            {
                get
                {
                    yield return new TestCaseData(DefaultCompoundInterest).Returns(1647.01);
                    yield return new TestCaseData(CompoundModelInput).Returns(1500.0);
                    yield return new TestCaseData(CompoundModelBigNumbersInput).Returns(101000000.0);
                }
            }

            public static IEnumerable InvalidInterestInputCases
            {
                get
                {
                    yield return new TestCaseData(InvalidInterestRate).SetName("InvalidInterestRate_ThrowsValidationException");
                    yield return new TestCaseData(InvalidNumberOfPeriods).SetName("InvalidNumberOfPeriods_ReturnsValidationError");
                }
            }


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

            public static IEnumerable IntegralInputCases
            {
                get
                {
                    yield return new TestCaseData(DefaultIntegralParameters).Returns(0.33);
                    yield return new TestCaseData(SameintervalIntegral).Returns(0);
                }
            }

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

            public static readonly IntegralParametersModel DefaultIntegralParameters
                = new IntegralParametersModel { StartInterval = 0, EndInterval = 1, IntervalsAmount = 10 };

        };

        internal static class TextData
        {

            public static IEnumerable StringTestData
            {
                get
                {
                    yield return new TestCaseData(TwoNonEmptyStrings).Returns("Hello, World!");

                    yield return new TestCaseData(OneEmptyStringAndNonEmpty).Returns("World!");

                    yield return new TestCaseData(new StringContainerModel
                    {
                        FirstString = "",
                        SecondString = ""
                    }).Returns("");
                }
            }

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


            public static readonly StringContainerModel StringAndNull = new StringContainerModel 
            { FirstString = null, SecondString = "World" };


        }
    }
}
