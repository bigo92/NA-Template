using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NA.Domain.Bases;
using NA.Domain.Services;
using NA.WebApi.Bases.Services;

namespace E.NA.WebApi.Modules.General.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TempController : ControllerBase
    {
        private readonly IControllerService controlerService;
        private readonly UnitOfWork unit;
        public TempController(IControllerService controlerService, UnitOfWork unit)
        {
            this.controlerService = controlerService;
            this.unit = unit;
        }

        [HttpGet]
        public IEnumerable<string> List()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet]
        public string Domain()
        {
            return controlerService.GetDomain();
        }

        [HttpGet]
        public string Service()
        {
            return unit.Service<TempService>().FindOne();
        }
    }
}