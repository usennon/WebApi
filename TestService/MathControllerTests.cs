using WebApi.Presentation.Controllers;
using Service.Contracts;
using Service.Utilities;
using Microsoft.AspNetCore.Mvc;
using Entities.TransferObjects;
using Microsoft.AspNetCore.Http;
using System;

namespace TestService
{
    [TestFixture]
    public class MathControllerTests
    {
        private Mock<IServiceManager> _mockServiceManager;

        private MathController CreateController( Mock<IServiceManager>? mockServiceManager = null )
        {
            var controller = new MathController((mockServiceManager ?? _mockServiceManager).Object);

            return controller;
        }

        [SetUp]
        public void Setup()
        {
            
            _mockServiceManager = new Mock<IServiceManager>();
            _mockServiceManager.Setup(s => s.MathService.GetSum(It.IsAny<int>(), It.IsAny<int>()))
                .Returns((int a, int b) => a + b);

            _mockServiceManager.Setup(s => s.MathService.GetSub(It.IsAny<int>(), It.IsAny<int>()))
                .Returns((int a, int b) => a - b);

            _mockServiceManager.Setup(s => s.MathService.GetSum(It.IsAny<NumberInputModel>()))
                .Returns((NumberInputModel input ) => input.Numbers.Sum());

            _mockServiceManager.Setup(s => s.MathService.GetAverage(It.IsAny<NumberInputModel>()))
                .Returns((NumberInputModel input) => input.Numbers.Average());

            _mockServiceManager.Setup(s => s.MathService.GetIntegral(It.IsAny<IntegralParametersModel>()))
                .Returns((IntegralParametersModel parameters) 
                => SimpsonMethod.SimpsonIntegral(parameters.StartInterval, parameters.EndInterval, parameters.IntervalsAmount));

            _mockServiceManager.Setup(s => s.MathService.GetCompoundInterest(It.IsAny<CompoundInterstModel>())).
                Returns((CompoundInterstModel values)
            => values.ifReinvestment ? (values.StartSum * Math.Pow((1 + values.YearInterestRate / values.NumberOfPeriods),
                values.NumberOfPeriods * values.YearsNumber))
                : (values.StartSum * (1 + values.YearInterestRate * values.YearsNumber)));
        }

        [Test]
        public void Sum_ReturnsSumOfIntegers()
        {
            // Arrange
            var controller = CreateController();

            // Act
            var result = controller.Sum(TestData.MathData.NumbersToSum.FirstNumber, 
                TestData.MathData.NumbersToSum.SecondNumber) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo(8));

        }

        [Test]
        public void Sub_ReturnsDifferenceOfIntegers()
        {
            //
            var controller = CreateController();

            // Act
            var result = controller.Sub(TestData.MathData.NumbersToSub.FirstNumber,
                TestData.MathData.NumbersToSub.SecondNumber) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo(7));

        }

        [Test]
        public void GetSum_WithBody_ReturnsSum()
        {
            // Arrange 
            var controller = CreateController();

            // Act
            var result = controller.GetSum(TestData.MathData.ListNumberInputSum) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo(9));

        }

        [Test]
        public void GetAverage_ReturnsAverage()
        {
            // Arrange
            var controller = CreateController();

            // Act
            var result = controller.GetAverage(TestData.MathData.ListNumbersInputAverage) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo(3.66).Within(0.01));

        }

        [Test]
        public void GetIntegral_ValidInput_ReturnsIntegral()
        {
            // Arrange
            var controller = CreateController();
            var expected = 0.33;

            // Act
            var result = controller.GetIntegral(TestData.MathData.DefaultIntegralParameters) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo(expected));

        }

        [Test]
        public void GetCompoundInterest_ValidInput_ReturnsInterest()
        {
            // Arrange
            var controller = CreateController();
            var expected = 1647.01;

            // Act
            var result = controller.GetCompoundInterest(TestData.MathData.DefaultCompoundInterest) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo(expected).Within(0.01));

        }
    }
}
