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
        private readonly Mock<IDispatcherFactory> _dispatcherFactory;
        private readonly Mock<IControllerService> _controllerService;
        private readonly Mock<ITempService> _tempService;
        public TempControllerTest()
        {
            _tempService = new Mock<ITempService>();
            _dispatcherFactory = new Mock<IDispatcherFactory>();
            _controllerService = new Mock<IControllerService>();
            
            //setup
            _tempService.Setup(x => x.FindOne()).Returns("123");
            _dispatcherFactory.Setup(x => x.Service<ITempService,TempService>()).Returns(_tempService.Object);
        }

        [Fact]
        public void TestValidFalse()
        {
            var model = new TempModel
            {
                name = ""
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
                name = "123"
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
            var controller = new TempController(_dispatcherFactory.Object, _controllerService.Object);
            // Act
            var result = controller.Test(new TempModel { name = "123" });

            // Assert
            var viewResult = Assert.IsType<string>(result);
            Assert.Equal("ok", viewResult);
        }

        [Fact]
        public async Task TestGet()
        {
            // Arrange
            var controller = new TempController(_dispatcherFactory.Object, _controllerService.Object);
            // Act
            var result = controller.Test();

            // Assert
            Assert.Equal("123", result);
        }
    }
}
