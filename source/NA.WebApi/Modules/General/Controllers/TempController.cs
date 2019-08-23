using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NA.Domain.Bases;
using NA.Domain.Models;
using NA.Domain.Services;
using NA.WebApi.Bases;
using NA.WebApi.Bases.Services;
using NA.WebApi.Modules.General.Models;

namespace NA.WebApi.Modules.General.Controllers
{
    public class TempController : ApiController
    {
        private readonly ITempService _sv;
        public TempController(ITempService sv)
        {
            this._sv = sv;
        }

        [HttpGet]
        public string Test()
        {
           var result = _sv.FindOne();
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

        [HttpPost]
        public async Task<object> Add([FromBody] Add_TempModel model)
        {
            ModelState.AddModelError("", "hehee");
            if (ModelState.IsValid)
            {
                _sv.Add(model);
            }
            return await BindData();
        }
    }
}