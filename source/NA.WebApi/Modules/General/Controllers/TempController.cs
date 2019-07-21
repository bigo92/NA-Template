using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NA.Domain.Bases;
using NA.Domain.Services;
using NA.WebApi.Bases.Services;

namespace E.NA.WebApi.Modules.General.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TempController : ControllerBase
    {
        private readonly IControllerService _cs;
        private readonly IDispatcherFactory _dp;
        private readonly ILogger _log;
        public TempController(IControllerService cs, IDispatcherFactory dp, ILogger<TempController> log)
        {
            _cs = cs;_dp = dp;_log = log;
        }

        [HttpGet]
        public IEnumerable<string> List()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet]
        public string Domain()
        {
            return _cs.GetDomain();
        }

        [HttpGet]
        public async Task<string> Service()
        {
            _log.LogError("ahihi");
            var a = _dp.Service<TempService>().FindOne();
            return a;
        }
    }
}