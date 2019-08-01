using Moq;
using NA.Domain.Bases;
using NA.Domain.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace NA.XUnitTest.Controllers
{
    public class TempControllerTest
    {
        private readonly Mock<IDispatcherFactory> dispatcherFactory;
        public TempControllerTest()
        {
            dispatcherFactory = new Mock<IDispatcherFactory>();
            dispatcherFactory.Setup(x => x.Service<ITempService>().FindOne()).Returns("aaa");
        }

        [Fact]
        public void Test()
        {
            var result = dispatcherFactory.Object.Service<ITempService>().FindOne();
            Assert.Equal("aaa", result);
        }
    }
}
