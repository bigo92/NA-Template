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


        //[ApiExplorerSettings(IgnoreApi = true)]
        //public async Task<ResultModel<dynamic>> BindForm(dynamic data)
        //{
        //    var result = new List<Dictionary<string, object>>();
        //    await Task.Run(() =>
        //    {
        //        result = GetAttrForm(data);
        //    });
        //    return new ResultModel<dynamic>
        //    {
        //        success = true,
        //        error = null,
        //        data = result,
        //        paging = null
        //    };
        //}

        //[ApiExplorerSettings(IgnoreApi = true)]
        //public List<Dictionary<string, object>> GetAttrForm(dynamic data)
        //{
        //    var propertyInfos = data.GetType().GetProperties();
        //    var result = new List<Dictionary<string, object>>();
        //    foreach (var pr in propertyInfos)
        //    {
        //        var property = new Dictionary<string, object>();
        //        var name = NA.Common.Extentions.StringExtention.FirstCharToLoower(pr.Name);
        //        var attrs = pr.CustomAttributes;
        //        var value = pr.GetValue(data, null);
        //        property.Add("name", name);
        //        var isArray = false;
        //        var isObject = true;

        //        var isControl = false;
        //        var jsonObject = false;
        //        var checkRequaid = false;
        //        foreach (var attr in attrs)
        //        {
        //            if (attr.AttributeType.Name == "JsonSringAttribute" && value != null)
        //            {
        //                jsonObject = true;
        //            }
        //            if (attr.AttributeType.Name == "FormControlAttribute" && value != null)
        //            {
        //                isControl = true;
        //            }
        //            if (attr.AttributeType.Name == "DefaultNullAttribute" && !(value is string) && ((value is DateTime && value == new DateTime()) || (!(value is DateTime) && 0 == (long)value)))
        //            {
        //                checkRequaid = true;
        //            }
        //        }

        //        if (isControl)
        //        {
        //            isObject = false;
        //        }
        //        else if (value is Array || value is IList)
        //        {
        //            var itemTypeBase = ArrayExtentiton.IsTypeBase(value);
        //            isArray = !itemTypeBase;
        //            isObject = isArray;
        //        }
        //        else
        //        {
        //            isObject = !ObjectExtention.IsTypeBase(value);
        //        }
        //        property.Add("isArray", isArray);
        //        property.Add("isObject", isObject);
        //        if (!isObject)
        //        {
        //            if (value is null)
        //            {
        //                property.Add("value", null);
        //            }
        //            else
        //            {
        //                if (checkRequaid)
        //                {
        //                    property.Add("value", null);
        //                }
        //                else
        //                {
        //                    if (jsonObject)
        //                    {
        //                        property.Add("value", Newtonsoft.Json.JsonConvert.SerializeObject(value));
        //                    }
        //                    else
        //                    {
        //                        property.Add("value", value);
        //                    }
        //                }
        //            }
        //        }
        //        else
        //        {
        //            if (isArray)
        //            {
        //                var lstArray = new List<dynamic>();
        //                foreach (var item in value)
        //                {
        //                    var chil = GetAttrForm(item);
        //                    lstArray.Add(chil);
        //                }
        //                property.Add("value", lstArray);
        //            }
        //            else
        //            {
        //                var chil = GetAttrForm(value);
        //                property.Add("value", chil);
        //            }
        //        }

        //        var lstValidate = new List<Dictionary<string, object>>();
        //        foreach (var attr in attrs)
        //        {
        //            var customAttr = new Dictionary<string, object>();
        //            var attrName = attr.AttributeType.Name.Replace("Attribute", "").ToLower();
        //            if (attrName == "defaultnull")
        //            {
        //                continue;
        //            }
        //            customAttr.Add("name", attrName);
        //            var attrValue = new Dictionary<string, object>();
        //            switch (attrName)
        //            {
        //                case "required":
        //                    foreach (var at in attr.NamedArguments)
        //                    {
        //                        if (at.MemberName == "ErrorMessage")
        //                        {
        //                            attrValue.Add("errorMessage", at.TypedValue.Value);
        //                        }
        //                    }
        //                    customAttr.Add("value", attrValue);
        //                    break;
        //                case "compare":
        //                    foreach (var at in attr.NamedArguments)
        //                    {
        //                        if (at.MemberName == "ErrorMessage")
        //                        {

        //                            attrValue.Add("errorMessage", at.TypedValue.Value);
        //                        }
        //                    }
        //                    if (attr.ConstructorArguments.Count > 0)
        //                    {
        //                        attrValue.Add("controllName", attr.ConstructorArguments[0].Value);
        //                    }
        //                    customAttr.Add("value", attrValue);
        //                    break;
        //                case "stringlength":
        //                    foreach (var at in attr.NamedArguments)
        //                    {
        //                        if (at.MemberName == "ErrorMessage")
        //                        {
        //                            attrValue.Add("errorMessage", at.TypedValue.Value);
        //                        }
        //                        if (at.MemberName == "MinimumLength")
        //                        {
        //                            attrValue.Add("minLength", at.TypedValue.Value);
        //                        }
        //                    }
        //                    if (attr.ConstructorArguments.Count > 0)
        //                    {
        //                        attrValue.Add("maxLength", attr.ConstructorArguments[0].Value);
        //                    }
        //                    customAttr.Add("value", attrValue);
        //                    break;
        //                case "minlength":
        //                    foreach (var at in attr.NamedArguments)
        //                    {
        //                        if (at.MemberName == "ErrorMessage")
        //                        {

        //                            attrValue.Add("errorMessage", at.TypedValue.Value);
        //                        }
        //                    }
        //                    if (attr.ConstructorArguments.Count > 0)
        //                    {
        //                        attrValue.Add("minLength", attr.ConstructorArguments[0].Value);
        //                    }
        //                    customAttr.Add("value", attrValue);
        //                    break;
        //                case "maxlength":
        //                    foreach (var at in attr.NamedArguments)
        //                    {
        //                        if (at.MemberName == "ErrorMessage")
        //                        {

        //                            attrValue.Add("errorMessage", at.TypedValue.Value);
        //                        }
        //                    }
        //                    if (attr.ConstructorArguments.Count > 0)
        //                    {
        //                        attrValue.Add("maxLength", attr.ConstructorArguments[0].Value);
        //                    }
        //                    customAttr.Add("value", attrValue);
        //                    break;
        //                case "emailaddress":
        //                    foreach (var at in attr.NamedArguments)
        //                    {
        //                        if (at.MemberName == "ErrorMessage")
        //                        {
        //                            attrValue.Add("errorMessage", at.TypedValue.Value);
        //                        }
        //                    }
        //                    customAttr.Add("value", attrValue);
        //                    break;
        //                case "phone":
        //                    foreach (var at in attr.NamedArguments)
        //                    {
        //                        if (at.MemberName == "ErrorMessage")
        //                        {
        //                            attrValue.Add("errorMessage", at.TypedValue.Value);
        //                        }
        //                    }
        //                    customAttr.Add("value", attrValue);
        //                    break;
        //                case "regularexpression":
        //                    foreach (var at in attr.NamedArguments)
        //                    {
        //                        if (at.MemberName == "ErrorMessage")
        //                        {
        //                            attrValue.Add("errorMessage", at.TypedValue.Value);
        //                        }
        //                    }
        //                    if (attr.ConstructorArguments.Count > 0)
        //                    {
        //                        attrValue.Add("regularExpression", attr.ConstructorArguments[0].Value);
        //                    }
        //                    customAttr.Add("value", attrValue);
        //                    break;
        //                case "datetime":
        //                    foreach (var at in attr.NamedArguments)
        //                    {
        //                        if (at.MemberName == "ErrorMessage")
        //                        {
        //                            attrValue.Add("errorMessage", at.TypedValue.Value);
        //                        }
        //                        if (at.MemberName == "Fomater")
        //                        {
        //                            attrValue.Add("fomater", at.TypedValue.Value);
        //                        }
        //                    }
        //                    customAttr.Add("value", attrValue);
        //                    break;
        //            }
        //            lstValidate.Add(customAttr);
        //        }
        //        property.Add("validate", lstValidate);
        //        result.Add(property);
        //    }
        //    return result;
        //}

        //[ApiExplorerSettings(IgnoreApi = true)]
        //public void AddErrors(IdentityResult result, Dictionary<string, string> replaceError = null)
        //{
        //    foreach (var error in result.Errors)
        //    {
        //        var errorMessage = error.Description;
        //        if (replaceError != null)
        //        {
        //            foreach (var item in replaceError)
        //            {
        //                if (errorMessage == item.Key)
        //                {
        //                    errorMessage = errorMessage.Replace(item.Key, item.Value);
        //                }
        //            }
        //        }

        //        ModelState.AddModelError(string.Empty, errorMessage);
        //    }
        //}

        //[ApiExplorerSettings(IgnoreApi = true)]
        //public async Task<JObject> PatchModel<T>(T model)
        //{
        //    var stream = this.HttpContext.Request.Body;
        //    stream.Position = 0;
        //    using (var reader = new System.IO.StreamReader(stream))
        //    {
        //        string body = await reader.ReadToEndAsync();
        //        var result = body.JsonToObject<JObject>();

        //        var propertyInfos = model.GetType().GetProperties();
        //        foreach (var pr in propertyInfos)
        //        {
        //            var name = pr.Name;
        //            if (!result.ContainsKey(name))
        //            {
        //                ModelState.Remove(name);
        //            }
        //        }
        //        return result;
        //    }
        //}

        //[ApiExplorerSettings(IgnoreApi = true)]
        //public async Task<Newtonsoft.Json.Linq.JObject> PatchModel2<T>(T model)
        //{
        //    var stream = this.HttpContext.Request.Body;
        //    stream.Position = 0;
        //    using (var reader = new System.IO.StreamReader(stream))
        //    {
        //        string body = await reader.ReadToEndAsync();
        //        var result = body.JsonToObject<Newtonsoft.Json.Linq.JObject>();

        //        var propertyInfos = model.GetType().GetProperties();
        //        foreach (var pr in propertyInfos)
        //        {
        //            var name = pr.Name;
        //            if (!result.ContainsKey(name.FirstCharToLoower()))
        //            {
        //                ModelState.Remove(name.FirstCharToUpper());
        //            }
        //        }
        //        return result;
        //    }
        //}

        //[ApiExplorerSettings(IgnoreApi = true)]
        //public bool ValidationParams<T>(T data, dynamic paramValid)
        //{
        //    var propertyInfos = JObject.FromObject(data);
        //    foreach (var pr in paramValid)
        //    {
        //        var name = pr.keyName;
        //        object value = null;
        //        switch (pr.type)
        //        {
        //            case "1"://Number                        
        //                if (propertyInfos.GetValue(name) != null && !string.IsNullOrEmpty(propertyInfos.GetValue(name).ToObject<string>()))
        //                {
        //                    try
        //                    {
        //                        value = propertyInfos.GetValue(name).ToObject<long>();
        //                    }
        //                    catch (Exception)
        //                    {
        //                        ModelState.AddModelError($"data.{name}", $"shared.{name}.format");
        //                    }
        //                }
        //                break;
        //            case "3"://Datetime                        
        //                if (propertyInfos.GetValue(name) != null && !string.IsNullOrEmpty(propertyInfos.GetValue(name).ToObject<string>()))
        //                {
        //                    try
        //                    {
        //                        value = propertyInfos.GetValue(name).ToObject<DateTime>().ToLocalTime();
        //                    }
        //                    catch (Exception)
        //                    {
        //                        ModelState.AddModelError($"data.{name}", $"shared.{name}.format");
        //                    }
        //                }
        //                break;
        //            case "4"://Float
        //                if (propertyInfos.GetValue(name) != null && !string.IsNullOrEmpty(propertyInfos.GetValue(name).ToObject<string>()))
        //                {
        //                    try
        //                    {
        //                        value = propertyInfos.GetValue(name).ToObject<decimal>();
        //                    }
        //                    catch (Exception)
        //                    {
        //                        ModelState.AddModelError($"data.{name}", $"shared.{name}.format");
        //                    }
        //                }
        //                break;
        //            case "5"://Dropdown Sigle                        
        //                if (propertyInfos.GetValue(name) != null && !string.IsNullOrEmpty(propertyInfos.GetValue(name).ToObject<string>()))
        //                {
        //                    try
        //                    {
        //                        value = propertyInfos.GetValue(name).ToObject<long?>();
        //                    }
        //                    catch (Exception)
        //                    {
        //                        value = propertyInfos.GetValue(name).ToObject<string>();
        //                    }
        //                }
        //                break;
        //            case "11"://Dropdown Multi                        
        //                if (propertyInfos.GetValue(name) != null && !string.IsNullOrEmpty(propertyInfos.GetValue(name).ToObject<string>()))
        //                {
        //                    try
        //                    {
        //                        value = propertyInfos.GetValue(name).ToObject<long[]>();
        //                    }
        //                    catch (Exception)
        //                    {
        //                        value = propertyInfos.GetValue(name).ToObject<string[]>();
        //                    }
        //                }
        //                break;
        //            case "6"://Boolean
        //                if (propertyInfos.GetValue(name) != null && !string.IsNullOrEmpty(propertyInfos.GetValue(name).ToObject<string>()))
        //                {
        //                    try
        //                    {
        //                        value = propertyInfos.GetValue(name).ToObject<bool>();
        //                    }
        //                    catch (Exception)
        //                    {
        //                        ModelState.AddModelError($"data.{name}", $"shared.{name}.format");
        //                    }
        //                }
        //                break;
        //            case "9"://Image
        //                if (propertyInfos.GetValue(name) != null)
        //                {
        //                    try
        //                    {
        //                        value = propertyInfos.GetValue(name).ToObject<List<tci.entites.Bases.FilesJson>>();
        //                    }
        //                    catch (Exception)
        //                    {
        //                        ModelState.AddModelError($"data.{name}", $"shared.{name}.format");
        //                    }
        //                }
        //                break;
        //            case "14"://File
        //                if (propertyInfos.GetValue(name) != null)
        //                {
        //                    try
        //                    {
        //                        value = propertyInfos.GetValue(name).ToObject<List<tci.entites.Bases.FilesJson>>();
        //                    }
        //                    catch (Exception)
        //                    {
        //                        ModelState.AddModelError($"data.{name}", $"shared.{name}.format");
        //                    }
        //                }
        //                break;
        //            case "12"://Email
        //                if (propertyInfos.GetValue(name) != null && !string.IsNullOrEmpty(propertyInfos.GetValue(name).ToObject<string>()))
        //                {
        //                    try
        //                    {
        //                        value = propertyInfos.GetValue(name).ToObject<string>();
        //                        if (!(value as string).ValidEmail())
        //                        {
        //                            ModelState.AddModelError($"data.{name}", $"shared.{name}.format");
        //                        }
        //                    }
        //                    catch (Exception)
        //                    {
        //                        ModelState.AddModelError($"data.{name}", $"shared.{name}.format");
        //                    }
        //                }
        //                break;
        //            case "13"://Phone
        //                if (propertyInfos.GetValue(name) != null && !string.IsNullOrEmpty(propertyInfos.GetValue(name).ToObject<string>()))
        //                {
        //                    try
        //                    {
        //                        value = propertyInfos.GetValue(name).ToObject<string>();
        //                    }
        //                    catch (Exception)
        //                    {
        //                        ModelState.AddModelError($"data.{name}", $"shared.{name}.format");
        //                    }
        //                }
        //                break;
        //            case "15"://drop table sigler                 
        //                if (propertyInfos.GetValue(name) != null && !string.IsNullOrEmpty(propertyInfos.GetValue(name).ToObject<string>()))
        //                {
        //                    try
        //                    {
        //                        value = propertyInfos.GetValue(name).ToObject<long?>();
        //                    }
        //                    catch (Exception)
        //                    {
        //                        value = propertyInfos.GetValue(name).ToObject<string>();
        //                    }
        //                }
        //                break;
        //            case "16"://Object
        //                if (propertyInfos.GetValue(name) != null)
        //                {
        //                    try
        //                    {
        //                        value = propertyInfos.GetValue(name).ToObject<JObject>();
        //                    }
        //                    catch (Exception)
        //                    {
        //                        ModelState.AddModelError($"data.{name}", $"shared.{name}.format");
        //                    }
        //                }
        //                break;
        //            case "17"://Array
        //                if (propertyInfos.GetValue(name) != null)
        //                {
        //                    if (propertyInfos.GetValue(name).Type == JTokenType.Array)
        //                    {
        //                        value = propertyInfos.GetValue(name);
        //                    }
        //                    else
        //                    {
        //                        ModelState.AddModelError($"data.{name}", $"shared.{name}.format");
        //                    }
        //                }
        //                break;
        //            case "18"://drop table multi                 
        //                if (propertyInfos.GetValue(name) != null)
        //                {
        //                    try
        //                    {
        //                        value = propertyInfos.GetValue(name).ToObject<long[]>();
        //                    }
        //                    catch (Exception)
        //                    {
        //                        value = propertyInfos.GetValue(name).ToObject<string[]>();
        //                    }
        //                }
        //                break;
        //            case "19"://Date time   
        //                if (propertyInfos.GetValue(name) != null && !string.IsNullOrEmpty(propertyInfos.GetValue(name).ToObject<string>()))
        //                {
        //                    try
        //                    {
        //                        value = propertyInfos.GetValue(name).ToObject<DateTime>().ToLocalTime();
        //                    }
        //                    catch (Exception)
        //                    {
        //                        ModelState.AddModelError($"data.{name}", $"shared.{name}.format");
        //                    }
        //                }
        //                break;
        //            default://String,Textarea,Editor,tag
        //                // value = propertyInfos.GetValue(name).ToObject<string>();
        //                value = propertyInfos.Value<string>(name);
        //                break;
        //        }
        //        foreach (var item in pr.validation)
        //        {
        //            switch (item.key)
        //            {
        //                case "1"://Required
        //                    if (value == null || (value is string && string.IsNullOrEmpty(value as string)))
        //                    {
        //                        ModelState.AddModelError($"data.{name}", item.message);
        //                    }
        //                    if (pr.type == "9" || pr.type == "14")// Image or File
        //                    {
        //                        if ((value as List<tci.entites.Bases.FilesJson>).Count(x => x.url != "") == 0)
        //                        {
        //                            ModelState.AddModelError($"data.{name}", item.message);
        //                        }
        //                    }
        //                    if (pr.type == "11" || pr.type == "18")
        //                    {
        //                        if ((value as Array) == null || (value as Array).Length == 0)
        //                        {
        //                            ModelState.AddModelError($"data.{name}", item.message);
        //                        }
        //                    }
        //                    if (pr.type == "17")
        //                    {
        //                        if ((value as JArray) == null || (value as JArray).Count == 0)
        //                        {
        //                            ModelState.AddModelError($"data.{name}", item.message);
        //                        }
        //                    }
        //                    break;
        //            }
        //        }
        //    }
        //    return ModelState.ErrorCount > 0 ? false : true;
        //}
    }
}
