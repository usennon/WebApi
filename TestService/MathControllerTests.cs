using NUnit.Framework;
using Moq;
using WebApi.Presentation.Controllers;
using Service.Contracts;
using Microsoft.AspNetCore.Mvc;
using Entities.TransferObjects;

namespace TestService
{
    [TestFixture]
    public class MathControllerTests
    {
        private Mock<IServiceManager> _mockServiceManager;
        private MathController _controller;

        [SetUp]
        public void Setup()
        {
            _mockServiceManager = new Mock<IServiceManager>();
            _controller = new MathController(_mockServiceManager.Object);
        }

        [Test]
        public void Sum_ReturnsSumOfIntegers()
        {
            // Arrange
            var a = 3;
            var b = 5;
            var expected = 8;
            _mockServiceManager.Setup(s => s.MathService.GetSum(a, b)).Returns(expected);

            // Act
            var result = _controller.Sum(a, b) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo(expected));

        }

        [Test]
        public void Sub_ReturnsDifferenceOfIntegers()
        {
            // Arrange
            var a = 10;
            var b = 3;
            var expected = 7;
            _mockServiceManager.Setup(s => s.MathService.GetSub(a, b)).Returns(expected);

            // Act
            var result = _controller.Sub(a, b) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo(expected));

        }

        [Test]
        public void GetSum_WithBody_ReturnsSum()
        {
            // Arrange
            var input = new NumberInputModel { Numbers = new List<int> { 2, 3, 4 } };
            var expected = 9;
            _mockServiceManager.Setup(s => s.MathService.GetSum(input)).Returns(expected);

            // Act
            var result = _controller.GetSum(input) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo(expected));

        }

        [Test]
        public void GetAverage_ReturnsAverage()
        {
            // Arrange
            var input = new NumberInputModel { Numbers = new List<int> { 2, 4, 6 } };
            var expected = 4;
            _mockServiceManager.Setup(s => s.MathService.GetAverage(input)).Returns(expected);

            // Act
            var result = _controller.GetAverage(input) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo(expected));

        }

        [Test]
        public void GetIntegral_ValidInput_ReturnsIntegral()
        {
            // Arrange
            var input = new IntegralParametersModel { IntervalsAmount = 10 };
            var expected = 42.0;
            _mockServiceManager.Setup(s => s.MathService.GetIntegral(input)).Returns(expected);

            // Act
            var result = _controller.GetIntegral(input) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo(expected));

        }

        [Test]
        public void GetCompoundInterest_ValidInput_ReturnsInterest()
        {
            // Arrange
            var input = new CompoundInterstModel
            {
                StartSum = 1000,
                YearInterestRate = 0.05,
                YearsNumber = 10,
                NumberOfPeriods = 12,
                ifReinvestment = true
            };
            var expected = 1647.01;
            _mockServiceManager.Setup(s => s.MathService.GetCompoundInterest(input)).Returns(expected);

            // Act
            var result = _controller.GetCompoundInterest(input) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo(expected));

        }
    }
}
