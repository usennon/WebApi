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
            // Arrange
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
            GetSumUtilityMethod(TestData.MathData.ListNumberInputSum, 9);
        }

        [Test]
        public void GetSum_WithEmptyList_ReturnsZero()
        {
            GetSumUtilityMethod(TestData.MathData.EmptyInput, 0);
        }

        [Test]
        public void GetSum_WithLargeNumberList_ReturnsCorrectSum()
        {
            GetSumUtilityMethod(TestData.MathData.LargeInput, TestData.MathData.LargeInput.Numbers.Sum());

        }

        [Test]
        public void GetAverage_ReturnsAverage()
        {
            GetAverageUtilityMethod(TestData.MathData.ListNumbersInputAverage, 3.66);
        }

        [Test]
        public void GetAverage_EmptyInput_ReturnsBadRequest()
        {
            // Arrange
            var controller = CreateController();

            // Act
            var result = controller.GetAverage(TestData.MathData.EmptyInput) as BadRequestObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(400));
        }

        [Test]
        public void GetAverage_OneNumber_ReturnsSameNumber()
        {
            GetAverageUtilityMethod(TestData.MathData.OneNumber, 7);
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
        public void GetIntegral_InvalidInput_ReturnsBadRequest()
        {
            // Specific test, we need to test our ActionFilter here to get error
            // Arrange
            var controller = CreateController();
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

        [Test]
        public void GetIntegral_ZeroInterval_ReturnsZero()
        {
            // Arrange
            var controller = CreateController();
            var expected = 0; // Integral under line(same interval) should be 0

            // Act
            var result = controller.GetIntegral(TestData.MathData.SameintervalIntegral) as OkObjectResult;

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

        [Test]
        public void GetCompoundIntersts_InvalidInputWrongInterestRate_ReturnsBadRequest()
        {
            // Arrange
            //here we are not testing our controller, but instead validating our model(what does apicontroller actually)
            var invalidModel = TestData.MathData.InvalidInterestRate;


            // Act
            var validationResult = ValidateModel(invalidModel);
            // Assert
            Assert.That(validationResult, Is.Not.Empty);
        }

        [Test]
        public void GetCompoundIntersts_InvalidInputWrongNumberOfPeriods_ReturnsBadRequest()
        {
            // Arrange
            //here we are not testing our controller, but instead validating our model(what does apicontroller actually)
            var invalidModel = TestData.MathData.InvalidNumberOfPeriods;


            // Act
            var validationResult = ValidateModel(invalidModel);
            // Assert
            Assert.That(validationResult, Is.Not.Empty);
        }

        [Test]
        public void GetCompoundInterest_ValidInputWithNoReinvestment_ReturnsInterest()
        {
            // Arrange
            var controller = CreateController();
            var expected = 1500.0;

            // Act
            var result = controller.GetCompoundInterest(TestData.MathData.CompoundModelInput) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo(expected).Within(0.01));

        }

        [Test]
        public void GetCompoundInterest_BigNumbersInput_ReturnsInterest()
        {
            // Arrange
            var controller = CreateController();
            var expected = 101000000.0;

            // Act
            var result = controller.GetCompoundInterest(TestData.MathData.CompoundModelBigNumbersInput) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo(expected).Within(0.01));

        }

        private void GetSumUtilityMethod(NumberInputModel input, int expected)
        {
            // Arrange
            var controller = CreateController();

            // Act
            var result = controller.GetSum(input) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo(expected));
        }

        private void GetAverageUtilityMethod(NumberInputModel input, double expected)
        {
            // Arrange
            var controller = CreateController();

            // Act
            var result = controller.GetAverage(input) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo(expected).Within(0.01));
        }
    }
}
