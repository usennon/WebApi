using Microsoft.AspNetCore.Mvc;
using Moq;
using Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Presentation.Controllers;

namespace TestService
{
    [TestFixture]
    public class TextControllerTests
    {
        private Mock<IServiceManager> _mockServiceManager;
        private TextController _controller;

        [SetUp]
        public void Setup()
        {
            _mockServiceManager = new Mock<IServiceManager>();
            _controller = new TextController(_mockServiceManager.Object);
        }

        [Test]
        public void GetUpperCase_ReturnsUpper()
        {
            var inputString = "toupper";
            var expected = "TOUPPER";
            _mockServiceManager.Setup(s => s.TextService.ToUpperCase(inputString)).Returns(expected);

            // Act
            var result = _controller.GetUpperCase(inputString) as OkObjectResult;

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
            _mockServiceManager.Setup(s => s.TextService.ToUpperCase(inputString)).Returns(expected);

            // Act
            var result = _controller.GetUpperCase(inputString) as OkObjectResult;

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

            // Act
            var result = _controller.GetUpperCase(input);

            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        }

        [Test]
        public void GetUpperCase_InputIsNull_ReturnsError()
        {
            // Arrange
            string? input = null;

            // Act
            var result = _controller.GetUpperCase(input) as BadRequestObjectResult;

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
            _mockServiceManager.Setup(s => s.TextService.ToUpperCase(input)).Returns(expected);

            // Act
            var result = _controller.GetUpperCase(input) as OkObjectResult;

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
            _mockServiceManager.Setup(s => s.TextService.ToUpperCase(input)).Returns(expected);

            // Act
            var result = _controller.GetUpperCase(input) as OkObjectResult;

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
            _mockServiceManager.Setup(s => s.TextService.ToUpperCase(input)).Returns(expected);

            // Act
            var result = _controller.GetUpperCase(input) as OkObjectResult;

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
            _mockServiceManager.Setup(s => s.TextService.ToUpperCase(input)).Returns(expected);

            // Act
            var result = _controller.GetUpperCase(input) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo(expected));
        }

        [Test]
        public void Concatenate_TwoNonEmptyStrings_ReturnsConcatenatedString()
        {
            // Arrange
            var str1 = "Hello, ";
            var str2 = "World!";
            var expected = "Hello, World!";
            _mockServiceManager.Setup(s => s.TextService.Concatenate(str1, str2)).Returns(expected);

            // Act
            var result = _controller.Concatenate(str1, str2) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo(expected));
        }

        [Test]
        public void Concatenate_OneEmptyAndOneNonEmptyString_ReturnsNonEmptyString()
        {
            // Arrange
            var str1 = "";
            var str2 = "World!";
            var expected = "World!";
            _mockServiceManager.Setup(s => s.TextService.Concatenate(str1, str2)).Returns(expected);

            // Act
            var result = _controller.Concatenate(str1, str2) as OkObjectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo(expected));
        }

    }
}