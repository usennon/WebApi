using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Entities.TransferObjects;
using WebApi.Presentation.Controllers;
using MemoryStorage.Interfaces;
using System.Text;

namespace TestService
{
    [TestFixture]
    public class TextControllerTests
    {
        private Mock<IServiceManager> _mockServiceManager;

        private Mock<IMemoryStorage> _mockStorage;

        private string _text;

        [SetUp]
        public void Setup()
        {
            _text = string.Empty;

            _mockServiceManager = new Mock<IServiceManager>();

            _mockStorage = new Mock<IMemoryStorage>();

            _mockServiceManager.Setup(s => s.TextService.ToUpperCase(It.IsAny<string>())).
                Returns((string s) => s.ToUpper());
            _mockServiceManager.Setup(s => s.TextService.Concatenate(It.IsAny<string>(), It.IsAny<string>())).
                Returns((string str1, string str2) =>
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(str1);
                    sb.Append(str2);
                    sb.Append(_text);
                    _text = sb.ToString();
                    return _text;
                });

            _mockServiceManager.Setup(s => s.TextService.Clear()).Callback(() => _text = string.Empty);
        }

        [Test]
        public void GetUpperCase_ReturnsUpper()
        {
            GetUpperCaseUtilityMethod("TOUPPER", "toupper");
        }

        [Test]
        public void GetUpperCase_SendStringNumber_ReturnsOk()
        {
            GetUpperCaseUtilityMethod("2", "2");

        }

        [Test]
        public void GetUpperCase_InputWithNumbersAndSymbols_ReturnsUpperCaseString()
        {
            GetUpperCaseUtilityMethod("HELLO123!@#", "hello123!@#");
        }

        [Test]
        public void GetUpperCase_InputIsAlreadyUpperCase_ReturnsSameString()
        {
            GetUpperCaseUtilityMethod("HELLO", "HELLO");
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
        public void Concatenate_StringIsNull_ReturnsSecondString()
        {
            // Arrange
            var controller = ControllerManager.CreateController<TextController>(_mockServiceManager);

            // Act
            var result = controller.Concatenate(TestData.TextData.StringAndNull) as BadRequestObjectResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(400));
        }
        

        [Test]
        public void Concatenate_WithMemory_ReturnsString()
        {
            // Arrange
            var controller = ControllerManager.CreateController<TextController>(_mockServiceManager);

            // Act
            controller.Concatenate(TestData.TextData.TwoNonEmptyStrings);
            var result = controller.Concatenate(TestData.TextData.OneEmptyStringAndNonEmpty) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo("World!Hello, World!"));
        }

        [Test]
        public void Concatenate_WithMemeryClear_ReturnsString()
        {
            // Arrange
            var controller = ControllerManager.CreateController<TextController>(_mockServiceManager);

            // Act
            controller.Concatenate(TestData.TextData.TwoNonEmptyStrings);
            controller.Clear();
            var result = controller.Concatenate(TestData.TextData.OneEmptyStringAndNonEmpty) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo("World!"));
        }

        private void ConcatenateUtilityMethod(string expected, StringContainerModel input)
        {
            // Arrange
            var controller = ControllerManager.CreateController<TextController>(_mockServiceManager);

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
            var controller = ControllerManager.CreateController<TextController>(_mockServiceManager);

            // Act
            var result = controller.GetUpperCase(input) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo(expected));
        }

    }
}