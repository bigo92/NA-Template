using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace NA.WebApi.Modules.System.Controlles
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            if (Request.Path.StartsWithSegments("/admin"))
            {
                return PhysicalFile(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "admin", "index.html"), "text/HTML");
            }
            return PhysicalFile(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "main", "index.html"), "text/HTML");
        }
    }
}