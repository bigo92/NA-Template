using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NA.Common.Extentions;
using NA.Common.Models;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace NA.WebApi.Bases
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ApiController: ControllerBase
    {
        protected string GetDomain()
        {
            return $"{Request.Scheme}://{Request.Host}";
        }

        protected string GetLanguage()
        {
            var key = "vi";
            if (Request.Headers.ContainsKey("Language"))
            {
                key = Request.Headers["Language"];
            }
            return key;
        }

        protected async Task<ResultModel<dynamic>> BindData(dynamic data = null, List<ErrorModel> errors = null, PagingModel paging = null)
        {
            if(errors != null)
            {
                foreach (var item in errors)
                {
                    ModelState.AddModelError(item.key, item.value);
                }
            }
          
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }

            return new ResultModel<dynamic>
            {
                success = ModelState.IsValid,
                error = new SerializableError(ModelState),
                data = data,
                paging = paging
            };
        }       
    }
}
