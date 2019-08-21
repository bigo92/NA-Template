using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NA.Common.Extentions
{
    public static class ValidationResultExtension
    {
        public static List<ValidationResult> ValidChildren<T>(object value)
        {
            var result = new List<ValidationResult>();
            var model = JToken.FromObject(value).ToObject<T>();
            var context = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, context, result, true);
            return result;
        }

        public static List<ValidationResult> ValidChildren<T>(this List<ValidationResult> source, object value)
        {
            var result = new List<ValidationResult>();
            var model = JToken.FromObject(value).ToObject<T>();
            var context = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, context, result, true);
            source.AddRange(result);
            return source;
        }
    }
}
