using WebApi.Presentation.Controllers;
using Service.Contracts;
using Service.Utilities;
using Microsoft.AspNetCore.Mvc;
using Entities.TransferObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApi.Presentation.ActionFilters;
using Microsoft.AspNetCore.Routing;
using System.ComponentModel.DataAnnotations;
using MemoryStorage.Interfaces;
using System;



namespace TestService
{
    [TestFixture]
    public class MathControllerTests
    {
        private Mock<IServiceManager> _mockServiceManager;

        private Mock<IMemoryStorage> _mockStorage;

        private int _currentTotal = 0;

        public static IList<ValidationResult> ValidateModel(object model)
        {
            var results = new List<ValidationResult>();

            var validationContext = new ValidationContext(model, null, null);

            Validator.TryValidateObject(model, validationContext, results, true);

            if (model is IValidatableObject validatableModel)
                results.AddRange(validatableModel.Validate(validationContext));

            return results;
        }

        [SetUp]
        public void Setup()
        {
            _currentTotal = 0;

            _mockServiceManager = new Mock<IServiceManager>();

            _mockStorage = new Mock<IMemoryStorage>();

            _mockStorage.Setup(s => s.GetCurrentTotal()).Returns(() => _currentTotal);

            _mockStorage.Setup(s => s.AddResult(It.IsAny<int>())).Callback<int>(result =>
            {
                _currentTotal = result;
            });

            _mockServiceManager.Setup(s => s.MathService.GetSum(It.IsAny<int>(), It.IsAny<int>()))
                .Returns((int a, int b) =>
                {
                    var sum = _mockStorage.Object.GetCurrentTotal() + a + b;
                    _mockStorage.Object.AddResult(sum);
                    return sum;
                });

            _mockServiceManager.Setup(s => s.MathService.GetSub(It.IsAny<int>(), It.IsAny<int>()))
                .Returns((int a, int b) => {
                    var sub = _mockStorage.Object.GetCurrentTotal() - a - b;
                    _mockStorage.Object.AddResult(sub);
                    return sub;
                });

            _mockServiceManager.Setup(s => s.MathService.GetSum(It.IsAny<NumberInputModel>()))
                .Returns((NumberInputModel input ) => {
                    var sum = input.Numbers.Sum() + _mockStorage.Object.GetCurrentTotal();
                    _mockStorage.Object.AddResult(sum);
                    return sum;
                });

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

            _mockServiceManager.Setup(s => s.MathService.Clear()).Callback(() => _currentTotal = 0);
        }

        [TestCase(5, 3, 8, TestName = "Adding 5 and 3 should return 8")]
        [TestCase(5, -3, 2, TestName = "Adding 5 and -3 should return 2")]
        [TestCase(-5, -3, -8, TestName = "Adding -5 and -3 should return -8")]
        public void Sum_ReturnsSumOfIntegers(int a, int b, int expected)
        {
            // Arrange
            var controller = ControllerManager.CreateController<MathController>(_mockServiceManager);


            // Act
            var result = controller.Sum(a, b) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo(expected));

        }

        [TestCase(10, 3, -13, TestName = "Substracting 10 and 3 should return -13")]
        [TestCase(-10, -3, 13, TestName = "Substracting -10 and -33 should return 13")]
        public void Sub_ReturnsDifferenceOfIntegers(int a, int b, int expected)
        {
            // Arrange
            var controller = ControllerManager.CreateController<MathController>(_mockServiceManager);

            // Act
            var result = controller.Sub(a, b) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo(expected));

        }

        [TestCaseSource(typeof(TestData.MathData), nameof(TestData.MathData.SumListInputCases))]
        public int GetSum_ReturnsSum(NumberInputModel input)
        {
            // Arrange
            var controller = ControllerManager.CreateController<MathController>(_mockServiceManager);

            // Act
            var result = controller.GetSum(input) as OkObjectResult;

            // Assert
            return Int32.Parse(result.Value.ToString());
        }

        [Test]
        public void Sum_AccumulatesCorrectly()
        {
            // Arrange
            var controller = ControllerManager.CreateController<MathController>(_mockServiceManager);

            // Act
            controller.Sum(3, 5);
            var result = controller.Sum(3, 5) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo(16));

            _mockStorage.Verify(s => s.AddResult(8), Times.Once);
            _mockStorage.Verify(s => s.AddResult(16), Times.Once);
        }

        [Test]
        public void Sub_AccumulatesCorrectly()
        {
            // Arrange
            var controller = ControllerManager.CreateController<MathController>(_mockServiceManager);

            // Act
            controller.Sub(3, 4);
            var result = controller.Sub(3, 4) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo(-14));

            _mockStorage.Verify(s => s.AddResult(-7), Times.Once);
            _mockStorage.Verify(s => s.AddResult(-14), Times.Once);
        }

        [Test]
        public void Clear_ResetsStorage()
        {
            // Arrange
            var controller = ControllerManager.CreateController<MathController>(_mockServiceManager);

            // Act
            controller.Sum(3, 5);
            controller.Clear();
            var result = controller.GetSum(TestData.MathData.ListNumberInputSum) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo(9));
        }

        [TestCaseSource(typeof(TestData.MathData), nameof(TestData.MathData.AverageListInputCases))]
        public double GetAverage_ReturnsAverage(NumberInputModel input)
        {
            // Arrange
            var controller = ControllerManager.CreateController<MathController>(_mockServiceManager);

            // Act
            var result = controller.GetAverage(input) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            return Double.Parse(result.Value.ToString());
        }

        [Test]
        public void GetAverage_EmptyInput_ReturnsBadRequest()
        {
            // Arrange
            var controller = ControllerManager.CreateController<MathController>(_mockServiceManager);

            // Act
            var result = controller.GetAverage(TestData.MathData.EmptyInput) as BadRequestObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(400));
        }

        [TestCaseSource(typeof(TestData.MathData), nameof(TestData.MathData.IntegralInputCases))]
        public double GetIntegral_ValidInput_ReturnsIntegral(IntegralParametersModel input)
        {
            // Arrange
            var controller = ControllerManager.CreateController<MathController>(_mockServiceManager);

            // Act
            var result = controller.GetIntegral(input) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            return Math.Round(Double.Parse(result.Value.ToString()), 2);

        }

        [Test]
        public void GetIntegral_InvalidInput_ReturnsBadRequest()
        {
            // Specific test, we need to test our ActionFilter here to get error
            // Arrange
            var controller = ControllerManager.CreateController<MathController>(_mockServiceManager);
            var filter = new ValidateEvenPositiveNumberFilter();
            var context = new ActionExecutingContext(
                new ActionContext
                {
                    HttpContext = new DefaultHttpContext(),
                    RouteData = new RouteData(),
                    ActionDescriptor = new ControllerActionDescriptor()
                },
                new List<IFilterMetadata>(),
                new Dictionary<string, object>
                {
                    { "input", TestData.MathData.InvalidIntervalIntegral }
                },
                controller
            );

            filter.OnActionExecuting(context);

            if (context.Result != null)
            {
                var result = context.Result as ObjectResult;

                // Assert
                Assert.That(result, Is.Not.Null);
                Assert.That(result.StatusCode, Is.EqualTo(400)); 
                return;
            }

            //// Act
            //var res = controller.GetIntegral(TestData.MathData.InvalidIntervalIntegral) as BadRequestObjectResult;

            //// Assert
            //Assert.That(res, Is.Not.Null);
            //Assert.That(res.StatusCode, Is.EqualTo(400));
        }


        [TestCaseSource(typeof(TestData.MathData), nameof(TestData.MathData.InterestInputCases))]
        public double GetCompoundInterest_ValidInput_ReturnsInterest(CompoundInterstModel input)
        {
            // Arrange
            var controller = ControllerManager.CreateController<MathController>(_mockServiceManager);

            // Act
            var result = controller.GetCompoundInterest(input) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            return Math.Round(Double.Parse(result.Value.ToString()), 2);

        }

        [TestCaseSource(typeof(TestData.MathData), nameof(TestData.MathData.InvalidInterestInputCases))]
        public void GetCompoundIntersts_InvalidInputWrongInterestRate_ReturnsBadRequest(CompoundInterstModel input)
        {
            // Arrange
            //here we are not testing our controller, but instead validating our model(what does apicontroller actually)


            // Act
            var validationResult = ValidateModel(input);
            // Assert
            Assert.That(validationResult, Is.Not.Empty);
            Assert.That(validationResult.First().ErrorMessage, Is.Not.Null.Or.Empty);
        }
       
    }
}
