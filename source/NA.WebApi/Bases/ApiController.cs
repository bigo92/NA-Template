using Microsoft.AspNetCore.Mvc;
using NA.Common.Models;
using System.Collections.Generic;
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

        protected async Task<IActionResult> BindData(dynamic data = null, List<ErrorModel> errors = null, PagingModel paging = null)
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
                return BadRequest(new ResultModel<dynamic>
                {
                    error = new SerializableError(ModelState),
                    data = data,
                    paging = paging
                });
            }

            if (paging != null)
            {
                return Ok(new ResultModel<dynamic>
                {
                    data = data,
                    paging = paging
                });
            }
            return Ok(data);
        }       
    }
}
