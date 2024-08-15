using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Entities.TransferObjects;
using WebApi.Presentation.Controllers;

namespace TestService
{
    [TestFixture]
    public class TextControllerTests
    {
        private Mock<IServiceManager> _mockServiceManager;

        private TextController CreateController(Mock<IServiceManager>? mockServiceManager = null)
        {
            var controller = new TextController((mockServiceManager ?? _mockServiceManager).Object);

            return controller;
        }

        [SetUp]
        public void Setup()
        {
            _mockServiceManager = new Mock<IServiceManager>();
            _mockServiceManager.Setup(s => s.TextService.ToUpperCase(It.IsAny<string>())).
                Returns((string s) => s.ToUpper());
            _mockServiceManager.Setup(s => s.TextService.Concatenate(It.IsAny<string>(), It.IsAny<string>())).
                Returns((string str1, string str2) => string.Concat(str1, str2));
        }

        [Test]
        public void GetUpperCase_ReturnsUpper()
        {
            // Arrange
            var inputString = "toupper";
            var expected = "TOUPPER";
            var controller = CreateController();

            // Act
            var result = controller.GetUpperCase(inputString) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo(expected));
        }

        [Test]
        public void GetUpperCase_SendStringNumber_ReturnsOk()
        {
            var inputString = "2";
            var expected = "2";
            var controller = CreateController();

            // Act
            var result = controller.GetUpperCase(inputString) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo(expected));
        }

        [Test]
        public void GetUpperCase_InputIsEmpty_ReturnsError()
        {
            // Arrange
            var input = string.Empty;
            var controller = CreateController();

            // Act
            var result = controller.GetUpperCase(input);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public void GetUpperCase_InputIsNull_ReturnsError()
        {
            // Arrange
            string? input = null;
            var controller = CreateController();

            // Act
            var result = controller.GetUpperCase(input) as BadRequestObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(400));
            Assert.That(result.Value, Is.EqualTo("Input cannot be null or empty."));
        }

        [Test]
        public void GetUpperCase_InputWithNumbersAndSymbols_ReturnsUpperCaseString()
        {
            // Arrange
            var input = "hello123!@#";
            var expected = "HELLO123!@#";
            var controller = CreateController();

            // Act
            var result = controller.GetUpperCase(input) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo(expected));
        }

        [Test]
        public void GetUpperCase_InputIsAlreadyUpperCase_ReturnsSameString()
        {
            // Arrange
            var input = "HELLO";
            var expected = "HELLO";
            var controller = CreateController();

            // Act
            var result = controller.GetUpperCase(input) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo(expected));
        }

        [Test]
        public void GetUpperCase_InputWithSpaces_ReturnsUpperCaseString()
        {
            GetUpperCaseUtilityMethod("HELLO WORLD", "hello world");
        }

        [Test]
        public void GetUpperCase_InputWithMixedCase_ReturnsUpperCaseString()
        {
            GetUpperCaseUtilityMethod("HELLO WORLD", "HeLLo WoRLd");
        }

        [Test]
        public void Concatenate_TwoNonEmptyStrings_ReturnsConcatenatedString()
        {
            ConcatenateUtilityMethod("Hello, World!", TestData.TextData.TwoNonEmptyStrings);
        }

        [Test]
        public void Concatenate_OneEmptyAndOneNonEmptyString_ReturnsNonEmptyString()
        {
            ConcatenateUtilityMethod("World!", TestData.TextData.OneEmptyStringAndNonEmpty);
        }

        [Test]
        public void Concatenate_BothEmptyStrings_ReturnsEmptyString()
        {
            ConcatenateUtilityMethod("", TestData.TextData.Empty);
        }

        [Test]
        public void Concatenate_FirstStringIsNull_ReturnsSecondString()
        {
            ConcatenateUtilityMethod("World", TestData.TextData.StringAndNull);
        }
        [Test]
        public void Concatenate_BothStringNulls_ReturnsSecondString()
        {
            ConcatenateUtilityMethod("", TestData.TextData.Nulls);
        }

        private void ConcatenateUtilityMethod(string expected, StringContainerModel input)
        {
            // Arrange
            var controller = CreateController();

            // Act
            var result = controller.Concatenate(input) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo(expected));
        }
        private void GetUpperCaseUtilityMethod(string expected, string input)
        {
            // Arrange
            var controller = CreateController();

            // Act
            var result = controller.GetUpperCase(input) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo(expected));
        }

    }
}