using System;
using System.IO;
using System.Threading.Tasks;
using NA.Domain.Services;
using NA.WebApi.Bases;
using NA.WebApi.Modules.General.Models;
using Microsoft.AspNetCore.Mvc;

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
        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> Setting()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "settings", "temp.json");
            return PhysicalFile(path, "application/json");
        }

        [HttpGet]
        public async Task<IActionResult> Test([FromQuery] Search_TempModel model)
        {
            if (ModelState.IsValid)
            {
                var result = _sv.Get(model);
                return await BindData(result.data, result.errors, result.paging);
            }
            return await BindData();
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
        public async Task<object> Add(Add_TempModel model)
        {
            if (ModelState.IsValid)
            {
                _sv.Add(model);
            }
            return await BindData();
        }
    }
}