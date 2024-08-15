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
            // Arrange
            var input = "hello world";
            var expected = "HELLO WORLD";
            var controller = CreateController();

            // Act
            var result = controller.GetUpperCase(input) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo(expected));
        }

        [Test]
        public void GetUpperCase_InputWithMixedCase_ReturnsUpperCaseString()
        {
            // Arrange
            var input = "HeLLo WoRLd";
            var expected = "HELLO WORLD";
            var controller = CreateController();

            // Act
            var result = controller.GetUpperCase(input) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo(expected));
        }

        [Test]
        public void Concatenate_TwoNonEmptyStrings_ReturnsConcatenatedString()
        {
            // Arrange
            var expected = "Hello, World!";
            var controller = CreateController();

            // Act
            var result = controller.Concatenate(TestData.TextData.TwoNonEmptyStrings) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo(expected));
        }

        [Test]
        public void Concatenate_OneEmptyAndOneNonEmptyString_ReturnsNonEmptyString()
        {
            // Arrange
            var expected = "World!";
            var controller = CreateController();

            // Act
            var result = controller.Concatenate(TestData.TextData.OneEmptyStringAndNonEmpty) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo(expected));
        }

    }
}