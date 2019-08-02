using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NA.Domain.Bases;
using NA.Domain.Services;
using NA.WebApi.Bases.Services;
using NA.WebApi.Modules.General.Models;

namespace NA.WebApi.Modules.General.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TempController : ControllerBase
    {
        private readonly IDispatcherFactory dispatcherFactory;
        public TempController(IDispatcherFactory dispatcherFactory)
        {
            this.dispatcherFactory = dispatcherFactory;
        }

        [HttpGet]
        public string Test()
        {
           var result = dispatcherFactory.Service<ITempService,TempService>().FindOne();
           return result;
        }

        [HttpPost]
        public string Test([FromBody] TempModel model)
        {
            if (ModelState.IsValid)
            {
                return "ok";
            }
            return "false";
        }
    }
}