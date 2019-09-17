using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using NA.Domain.Bases;
using NA.Domain.Services;
using NA.WebApi.Bases.Services;
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
            
        }

        [Fact]
        public void TestValidFalse()
        {
           
        }

        [Fact]
        public void TestValidTrue()
        {
            
        }

        [Fact]
        public async Task TestPost()
        {
           
        }

        [Fact]
        public async Task TestGet()
        {
            
        }
    }
}
