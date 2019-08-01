using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using NA.Domain.Bases;
using NA.Domain.Services;
using NA.WebApi.Bases.Services;
using NA.WebApi.Modules.General.Controllers;
using System;
using System.Collections.Generic;
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
            _dispatcherFactory = new Mock<IDispatcherFactory>();
            _controllerService = new Mock<IControllerService>();
            _tempService = new Mock<ITempService>();
            //setup
            _tempService.Setup(x => x.FindOne()).Returns("123");
            _dispatcherFactory.Setup(x => x.Service<ITempService>()).Returns(_tempService.Object);
        }

        [Fact]
        public async Task TestValid()
        {
            // Arrange
            var controller = new TempController(_dispatcherFactory.Object, _controllerService.Object);
            // Act
            var result = controller.TestValid(new WebApi.Modules.General.Models.TempModel { });

            // Assert
            var viewResult = Assert.IsType<string>(result);
            Assert.Equal("ok", viewResult);
        }
    }
}
