using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NA.Domain.Bases;
using NA.Domain.Services;
using NA.WebApi.Bases.Services;

namespace NA.WebApi.Modules.General
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TempController : ControllerBase
    {
        private readonly IDispatcherFactory dispatcherFactory;
        private readonly IControllerService controllerService;
        public TempController(IDispatcherFactory dispatcherFactory, IControllerService controllerService)
        {
            this.dispatcherFactory = dispatcherFactory;
            this.controllerService = controllerService;
        }

        [HttpGet]
        public string Test()
        {
           var result = dispatcherFactory.Service<TempService>().FindOne();
           return result;
        }
    }
}