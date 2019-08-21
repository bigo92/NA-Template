using NA.Common.Extentions;
using NA.DataAccess.Models;
using NA.Domain.Models;
using NA.WebApi.Bases;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static NA.DataAccess.Models.Template;

namespace NA.WebApi.Modules.General.Models
{
    public class TempModel
    {
        [Required(ErrorMessage = "Not Required")]
        public string name { get; set; }
    }

    public class Add_TempModel : Add_TemplateServiceModel, IValidatableObject
    {
        public override Guid Id { get => base.Id; set => base.Id = value; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return ValidationResultExtension
                .ValidChildren<Info_TempModel>(Info)
                .ValidChildren<Address_TempModel>(Address);
        }

        public class Info_TempModel : InfoJson
        {
            [Required(ErrorMessage = "Required Name")]
            public override string name { get => base.name; set => base.name = value; }
        }
        public class Address_TempModel : AddressJson
        {
            [Required(ErrorMessage = "Required address1")]
            public override string address1 { get => base.address1; set => base.address1 = value; }

            [Required(ErrorMessage = "Required address2")]
            public override string address2 { get => base.address2; set => base.address2 = value; }

            [Required(ErrorMessage = "Required address3")]
            public override string address3 { get => base.address3; set => base.address3 = value; }
        }
    }
}
