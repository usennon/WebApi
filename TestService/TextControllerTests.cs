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