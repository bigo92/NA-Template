using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using tci.common.Extentions;
using tci.entites.Bases;
using tci.repository;
using tci.repository.Bases;
using tci.repository.Models;
using tci.server.Bases;
using static tci.server.Modules.Aut.Models.RolesModel;

namespace tci.server.Modules.Aut.Controllers
{
    [Route("/api/[controller]/[action]")]
    [Authorize]
    public class RolesController : BaseController
    {
        ILogger log;
        public RolesController(UnitOfWork unit, ILoggerFactory loggerFactory, IConfiguration config, IHttpContextAccessor httpContextAccessor) : base(unit, loggerFactory, config, httpContextAccessor)
        {
            this.log = loggerFactory.CreateLogger<RolesController>();
        }
               
        [HttpGet("/api/[controller]")]
        public async Task<object> GetData(Search_RolesModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await unit.Repository<RolesRepository>().List(model);
                return await BindData(result.data, result.errors, result.paging);
            }
            return await BindData();
        }
        
        [HttpPost("/api/[controller]")]
        public async Task<object> PostData([FromBody]Add_RolesModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await unit.Repository<RolesRepository>().Add(model);
                return await BindData(result.data, result.errors);
            }
            return await BindData();
        }        

        [HttpPut("/api/[controller]")]
        public async Task<object> PutData(long id,[FromBody] Edit_RolesModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await unit.Repository<RolesRepository>().Edit(id, model);
                return await BindData(result.data, result.errors);
            }
            return await BindData();
        }

        [HttpPatch("/api/[controller]")]
        public async Task<object> PatchData(long id, [FromBody]Patch_RolesModel model)
        {
            var patch = await PatchModel(model);
            if (ModelState.IsValid)
            {
                var result = await unit.Repository<RolesRepository>().Patch(id, patch);
                return await BindData(result.data, result.errors);
            }
            return await BindData();
        }

        [HttpDelete("/api/[controller]")]
        public async Task<object> DeleteData(Delete_RolesModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await unit.Repository<RolesRepository>().Delete(model);
                return await BindData(result.data, result.errors);
            }
            return await BindData();
        }

        [HttpGet]
        public async Task<object> GetForm(Search_RolesModel model)
        {
            return await BindForm(model);
        }

        [HttpGet]
        public async Task<object> PostForm(Add_RolesModel model)
        {            
            return await BindForm(model);
        }

        [HttpGet]
        public async Task<object> PutForm(long id, Edit_RolesModel model)
        {
            var result = await unit.db.Select("*").From("aut.Roles").WhereRaw($"id = {id}").First();
            if (result != null)
            {
                var dataJson = result.Value<string>("data").JsonToObject<JObject>();
                model.name = result.Value<string>("name");
                model.description = dataJson.Value<string>("description");

                model.claims = unit.Repository<RoleClaimsRepository>().List();
                var lstFocus = unit.db.RoleClaims.Where(x => x.RoleId == id).ToList();               
                model.claims.ForEach(x => {
                    x.groupClaims.ForEach(g =>
                    {
                        if (lstFocus.Any(c => c.ClaimValue == g.values))
                        {
                            g.selected = true;
                        }
                        else
                        {
                            g.selected = false;
                        }
                    });
                    x.groupClaims = x.groupClaims.Where(c => c.selected).ToList();
                });
                model.claims = model.claims.Where(x => x.groupClaims.Count > 0).ToList();                
            }
            return await BindForm(model);
        }

        [HttpGet]
        public async Task<object> FindOne(Detail_RolesModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await unit.Repository<RolesRepository>().Detail(model);
                return await BindData(result.data, result.errors);
            }
            return await BindData();
        }        

        [HttpGet]
        public async Task<object> GetClaims()
        {
            var result = unit.Repository<RoleClaimsRepository>().List();
            return await BindData(result);
        }
    }
}