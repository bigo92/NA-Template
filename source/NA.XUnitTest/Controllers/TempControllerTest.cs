using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using NA.Domain.Bases;
using NA.Domain.Services;
using NA.WebApi.Bases.Services;
using NA.WebApi.Modules.General.Controllers;
using NA.WebApi.Modules.General.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NA.XUnitTest.Controllers
{
    public class TempControllerTest
    {
        private readonly Mock<ITempService> _tempService;
        public TempControllerTest()
        {
            _tempService = new Mock<ITempService>();
            
            //setup
            _tempService.Setup(x => x.FindOne()).Returns("123");
        }

        [Fact]
        public void TestValidFalse()
        {
            var model = new TempModel
            {
                Name = ""
            };
            // Set some properties here
            var context = new ValidationContext(model, null, null);
            var results = new List<ValidationResult>();
            var isModelStateValid = Validator.TryValidateObject(model, context, results, true);

            // Assert here
            Assert.False(isModelStateValid);
        }

        [Fact]
        public void TestValidTrue()
        {
            var model = new TempModel {
                Name = "123"
            };
            // Set some properties here
            var context = new ValidationContext(model, null, null);
            var results = new List<ValidationResult>();
            var isModelStateValid = Validator.TryValidateObject(model, context, results, true);

            // Assert here
            Assert.True(isModelStateValid);
        }

        [Fact]
        public async Task TestPost()
        {
            // Arrange
            var controller = new TempController(_tempService.Object);
            // Act
            var result = controller.Test(new TempModel { Name = "123" });

            // Assert
            var viewResult = Assert.IsType<string>(result);
            Assert.Equal("ok", viewResult);
        }

        [Fact]
        public async Task TestGet()
        {
            // Arrange
            var controller = new TempController(_tempService.Object);
            // Act
            var result = controller.Test();

            // Assert
            Assert.Equal("123", result);
        }
    }
}
