using System;
using System.IO;
using System.Threading.Tasks;
using NA.Domain.Services;
using NA.WebApi.Bases;
using NA.WebApi.Modules.General.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

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
        public async Task<IActionResult> Get([FromQuery] Search_TempModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _sv.Get(model);
                if (result.errors.Count == 0)
                {
                    //new task  sent mail
                    //push notification
                }
                return await BindData(result.data, result.errors, result.paging);
            }
            return await BindData();
        }

        [HttpPost]
        public async Task<object> Add([FromBody] Add_TempModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _sv.Add(model);
                if (result.errors.Count == 0)
                {
                    //new task  sent mail
                    //push notification
                }
                return await BindData(result.data, result.errors);
            }
            return await BindData();
        }

        [HttpPut("{id}")]
        public async Task<object> Edit(Guid id, [FromBody] Edit_TempModel model)
        {
            if (ModelState.IsValid)
            {
                model.id = id;
                var result = await _sv.Edit(model);
                if (result.errors.Count == 0)
                {
                    //new task  sent mail
                    //push notification
                }
                return await BindData(result.data, result.errors);
            }
            return await BindData();
        }

        [HttpPatch("{id}")]
        public async Task<object> Patch(Guid id, [FromBody] JObject model)
        {
            if (ModelState.IsValid)
            {
                var result = await _sv.Patch(id, model);
                if (result.errors.Count == 0)
                {
                    //new task  sent mail
                    //push notification
                }
                return await BindData(result.data, result.errors);
            }
            return await BindData();
        }

        [HttpDelete("{id}")]
        public async Task<object> Delete(Guid id)
        {
            if (ModelState.IsValid)
            {
                var result = await _sv.Delete(id);
                if (result.errors.Count == 0)
                {
                    //new task  sent mail
                    //push notification
                }
                return await BindData(result.data, result.errors);
            }
            return await BindData();
        }

        [HttpGet("Count")]
        public async Task<object> Count([FromQuery] Count_TempModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _sv.Count(model);
                if (result.errors.Count == 0)
                {
                    //new task  sent mail
                    //push notification
                }
                return await BindData(result.data, result.errors);
            }
            return await BindData();
        }
    }
}