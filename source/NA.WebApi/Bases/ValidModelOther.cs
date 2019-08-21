using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static NA.DataAccess.Models.Template;

namespace NA.WebApi.Bases
{
    public class ValidModelOther : ValidationAttribute
    {
        public Type typeModel;
        
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            
            var results = new List<ValidationResult>();
            var model = JToken.FromObject(value).ToObject(typeModel);
            var context = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, context, results, true);
            if (results.Count > 0)
            {
                return results[0];
            }
            return ValidationResult.Success;
        }
    }
}
