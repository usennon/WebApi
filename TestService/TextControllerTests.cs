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

        [TestCase("toupper", "TOUPPER")]
        [TestCase("2", "2")]
        [TestCase("hello123!@#", "HELLO123!@#")]
        [TestCase("HELLO", "HELLO")]
        [TestCase("HeLLo WoRLd", "HELLO WORLD")]
        public void GetUpperCase_ReturnsUpper(string input, string expected)
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

        [TestCaseSource(typeof(TestData.TextData), nameof(TestData.TextData.StringTestData))]
        public string Concatenate_ReturnsConcatenatedString(StringContainerModel input)
        {
            // Arrange
            var controller = ControllerManager.CreateController<TextController>(_mockServiceManager);

            // Act
            var result = controller.Concatenate(input) as OkObjectResult;

            // Assert
            Assert.That(result.StatusCode, Is.EqualTo(200));
            return result.Value.ToString();
        }

        [Test]
        public void Concatenate_StringIsNull_ReturnsError()
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

    }
}