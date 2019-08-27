using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace NA.Common.Extentions
{
    public static class ValidationResultExtension
    {
        public static List<ValidationResult> ValidModel<T>(object value)
        {
            var trace = new StackTrace();
            Type calledFromType = trace.GetFrame(1).GetMethod().ReflectedType;
            var field = calledFromType.GetProperties().FirstOrDefault(i => i.PropertyType == value.GetType());

            var result = new List<ValidationResult>();
            var model = JToken.FromObject(value).ToObject<T>();
            var context = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, context, result, true);

            for (int i = 0; i < result.Count; i++)
            {
                result[i] = new ValidationResult(result[i].ErrorMessage, new[] { $"{field.Name}.{result[i].MemberNames.ElementAt(0)}" });
            }

            return result;
        }

        public static List<ValidationResult> ValidModel<T>(this List<ValidationResult> source, object value)
        {
            var trace = new StackTrace();
            Type calledFromType = trace.GetFrame(1).GetMethod().ReflectedType;
            var field = calledFromType.GetProperties().FirstOrDefault(i => i.PropertyType == value.GetType());

            var result = new List<ValidationResult>();
            var model = JToken.FromObject(value).ToObject<T>();
            var context = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, context, result, true);

            for (int i = 0; i < result.Count; i++)
            {
                result[i] = new ValidationResult(result[i].ErrorMessage, new[] { $"{field.Name}.{result[i].MemberNames.ElementAt(0)}" });
            }

            source.AddRange(result);
            return source;
        }

        public class TestT<T>
        {
            public T ddd {get;set;}
        }
    }
}
