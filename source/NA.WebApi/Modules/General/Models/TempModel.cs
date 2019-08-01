using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NA.WebApi.Modules.General.Models
{
    public class TempModel
    {
        [Required(ErrorMessage = "Not Required")]
        public string name { get; set; }
    }
}
