using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NA.WebApi.Bases;

namespace NA.WebApi.Modules.System.Controllers
{
    public class UseExceptionHandlerController : ApiController
    {
        private readonly ILogger _logger;
        public UseExceptionHandlerController(ILogger<UseExceptionHandlerController> logger)
        {
            _logger = logger;
        }

        [Route("/api/error"), ApiExplorerSettings(IgnoreApi = true)]
        public async Task<object> Error()
        {
            // Get the details of the exception that occurred
            var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            if (exceptionFeature != null)
            {
                // Get which route the exception occurred at
                string routeWhereExceptionOccurred = exceptionFeature.Path;

                // Get the exception that occurred
                Exception exceptionThatOccurred = exceptionFeature.Error;

                ModelState.AddModelError("", exceptionThatOccurred.Message);

                this._logger.LogError($"[{routeWhereExceptionOccurred}]: {exceptionThatOccurred.Message}");
                this._logger.LogError(exceptionThatOccurred.StackTrace);
            }
            return await BindData();
        }
    }
}