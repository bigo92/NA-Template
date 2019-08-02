using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NA.Common.Extentions
{
    public static class JsonExtention
    {
        public static string JsonToString<T>(this T source, bool fixMSSQL = true)
        {
            var result = Newtonsoft.Json.JsonConvert.SerializeObject(source);
            if (fixMSSQL)
            {
                result = result.Replace("'", "''");
            }
            return result;
        }

        public static JObject JsonToObject(this string source)
        {
            if (source != null)
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(source);
            }
            else
            {
                return default(JObject);
            }
        }

        public static T JsonToObject<T>(this string source)
        {
            if (source != null)
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(source);
            }
            else
            {
                return default(T);
            }
        }

        public static JObject JsonToObject(this JObject source, string keyName)
        {
            if (source != null)
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(source.Value<string>(keyName));
            }
            else
            {
                return default(JObject);
            }
        }

        public static T JsonToObject<T>(this JObject source, string keyName)
        {
            if (source != null)
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(source.Value<string>(keyName));
            }
            else
            {
                return default(T);
            }
        }

        public static string RenderFilter(this JObject source, string tagAs = "")
        {
            var lstWhere = "";
            var part = "";
            if (source.ContainsKey("and") || source.ContainsKey("or"))
            {
                var key = source.ContainsKey("and") ? "and" : "or";
                lstWhere += "(";
                var value = source.Value<JArray>(key);

                foreach (JObject item in value)
                {
                    lstWhere += $"{part}{RenderFilter(item, tagAs)}";
                    part = $" {key} ";
                }
                lstWhere += ")";
            }
            else
            {
                foreach (var item in source)
                {
                    var k = item.Key.Split('.');
                    var v = item.Value;
                    var t = v.GetType();
                    var convertType = "";

                    var rk = "";
                    if (k.Length == 1)
                    {
                        var rkTemp = k[0].Replace("->", ".");
                        if (rkTemp.IndexOf(":") != -1)
                        {
                            var converData = rkTemp.Split(":");
                            rkTemp = rkTemp.Replace($":{converData[1]}", "");
                            rkTemp = rkTemp.Insert(0, "PARSE(");
                            rkTemp += $" as {converData[1]})";
                            convertType = converData[1];
                        }
                        rk = rkTemp;
                    }
                    else
                    {
                        for (int j = 0; j < k.Length; j++)
                        {
                            if (j == 0)
                            {
                                var columnJson = k[j].IndexOf("->") == -1 ? tagAs : "";
                                rk = $"JSON_VALUE({columnJson}{k[j].Replace("->", ".")},'$";
                            }
                            else if (j == k.Length - 1)
                            {
                                var rkTemp = k[j];
                                if (rkTemp.IndexOf(":") != -1)
                                {
                                    var converData = rkTemp.Split(":");
                                    rkTemp = rkTemp.Replace($":{converData[1]}", "");
                                    rk += $".{rkTemp}')";
                                    rk = rk.Insert(0, "PARSE(");
                                    rk += $" as {converData[1]})";
                                    convertType = converData[1];
                                }
                                else
                                {
                                    rk += $".{k[j]}')";
                                }
                            }
                            else
                            {
                                rk += $".{k[j]}";
                            }
                        }
                    }

                    if (t == typeof(JObject))
                    {
                        foreach (var v2value in v as JObject)
                        {
                            switch (v2value.Key)
                            {
                                case "gt":
                                    lstWhere += $"{part}{rk} > {GetValue(v2value.Value as JValue, convertType)}";
                                    part = " and ";
                                    break;
                                case "gte":
                                    lstWhere += $"{part}{rk} >= {GetValue(v2value.Value as JValue, convertType)}";
                                    part = " and ";
                                    break;
                                case "lt":
                                    lstWhere += $"{part}{rk} < {GetValue(v2value.Value as JValue, convertType)}";
                                    part = " and ";
                                    break;
                                case "lte":
                                    lstWhere += $"{part}{rk} <= {GetValue(v2value.Value as JValue, convertType)}";
                                    part = " and ";
                                    break;
                                case "neq":
                                    if ((v2value.Value as JValue).Type == JTokenType.Null)
                                    {
                                        lstWhere += $"{part}{rk} is not null";
                                    }
                                    else
                                    {
                                        lstWhere += $"{part}{rk} <> {GetValue(v2value.Value as JValue, convertType)}";
                                    }
                                    part = " and ";
                                    break;
                                case "eqn"://Todo remove
                                    lstWhere += $"{part}{rk} is null";
                                    part = " and ";
                                    break;
                                case "neqn"://Todo remove
                                    lstWhere += $"{part}{rk} is not null";
                                    part = " and ";
                                    break;
                                case "like":
                                    lstWhere += $"{part}{rk} like N'%{v2value.Value}%'";
                                    part = " and ";
                                    break;
                                case "nlike":
                                    lstWhere += $"{part}{rk} not like N'%{v2value.Value}%'";
                                    part = " and ";
                                    break;
                                case "insplit":
                                    lstWhere += $"{part}{v2value.Value} in (select * from STRING_SPLIT({rk},','))";
                                    part = " and ";
                                    break;
                                case "ninsplit":
                                    lstWhere += $"{part}{v2value.Value} not in (select * from STRING_SPLIT({rk},','))";
                                    part = " and ";
                                    break;
                                case "inarray":
                                    lstWhere += $"{part}{v2value.Value} in (select t.* from  {rk.Replace("JSON_VALUE", "OPENJSON")} WITH(id nvarchar(max) '$') as t)";
                                    part = " and ";
                                    break;
                                case "ninarray":
                                    lstWhere += $"{part}{v2value.Value} not in (select t.* from  {rk.Replace("JSON_VALUE", "OPENJSON")} WITH(id nvarchar(max) '$') as t)";
                                    part = " and ";
                                    break;
                                case "inarrayid":
                                    lstWhere += $"{part}{v2value.Value} in (select t.* from  {rk.Replace("JSON_VALUE", "OPENJSON")} WITH(id bigint) as t)";
                                    part = " and ";
                                    break;
                                case "ninarrayid":
                                    lstWhere += $"{part}{v2value.Value} not in (select t.* from  {rk.Replace("JSON_VALUE", "OPENJSON")} WITH(id bigint) as t)";
                                    part = " and ";
                                    break;
                                case "in":
                                    lstWhere += $"{part}{rk} in ({string.Join(',', v2value.Value)})";
                                    part = " and ";
                                    break;
                                case "nin":
                                    lstWhere += $"{part}{rk} not in ({string.Join(',', v2value.Value)})";
                                    part = " and ";
                                    break;
                                case "startwith":
                                    lstWhere += $"{part}{rk} like N'{v2value.Value}%'";
                                    part = " and ";
                                    break;
                                case "endwith":
                                    lstWhere += $"{part}{rk} like N'%{v2value.Value}'";
                                    part = " and ";
                                    break;
                                default:
                                    if ((v2value.Value as JValue).Type == JTokenType.Null)
                                    {
                                        lstWhere += $"{part}{rk} is null";
                                    }
                                    else
                                    {
                                        lstWhere += $"{part}{rk} = {GetValue(v2value.Value as JValue, convertType)}";
                                    }
                                    part = " and ";
                                    break;
                            }
                        }

                    }
                    else
                    {
                        if ((v as JValue).Type == JTokenType.Null)
                        {
                            lstWhere += $"{part}{rk} is not null";
                        }
                        else
                        {
                            lstWhere += $"{part}{rk} = {GetValue(v as JValue, convertType)}";
                        }
                        part = " and ";
                    }
                }
            }
            return lstWhere;
        }

        private static string GetValue(JValue v, string convertType)
        {
            switch (v.Type)
            {
                case JTokenType.Null:
                    return "null";
                case JTokenType.String:
                    return $"N'{v}'";
                case JTokenType.Boolean:
                    return $"'{v}'";
                case JTokenType.Date:
                    if (convertType.HasValue())
                    {
                        return $"PARSE('{v.Value<DateTime>().ToISOString()}' as {convertType})";
                    }
                    else
                    {
                        return $"PARSE('{v.Value<DateTime>().ToISOString()}' as datetime2)";
                    }
                case JTokenType.Integer:
                    return $"{v}";
                default:
                    return $"N'{v}'";
            }
        }

        public static bool ValidateJSON(this string s)
        {
            try
            {
                if (s.IndexOf('{') == -1 && s.IndexOf('[') == -1)
                {
                    return false;
                }
                JToken.Parse(s);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static void ConvertJsonData(this JToken source)
        {
            if (source is JObject)
            {
                foreach (var item in source)
                {
                    if (item is JObject || item is JArray)
                    {
                        ConvertJsonData(item);
                    }
                    else
                    {
                        var value = item as JProperty;
                        if (value.Value.Type == JTokenType.String && value.Value.Value<string>().ValidateJSON())
                        {
                            var newValue = value.Value.Value<string>().JsonToObject<JToken>();
                            source[value.Name] = newValue;
                            ConvertJsonData(item);
                        }
                    }
                }
            }
            else if (source is JArray)
            {
                foreach (var item in source)
                {
                    ConvertJsonData(item);
                }
            }
        }

        public static T TolowerKeyName<T>(this T source)
        {
            var listName = new List<object>();
            var jsonString = source.JsonToString();
            var jsonSup = jsonString.Split(',');
            var jsonTarget = jsonSup.Where(x => x.LastIndexOf("\":") != -1);
            foreach (string item in jsonTarget)
            {
                var nameBase = item;
                while (nameBase.IndexOf("\":") != -1)
                {
                    var value = nameBase.Substring(0, nameBase.IndexOf("\":"));
                    var name = value.Substring(value.LastIndexOf('"')).Replace("\"", "").Replace("\\", "");
                    var oldV = $"{value}\""; var newV = oldV.Replace(name, name.FirstCharToLoower());
                    listName.Add(new { oldV, newV });
                    nameBase = nameBase.Substring(nameBase.IndexOf("\":") + 2);
                }
            }
            foreach (dynamic item in listName)
            {
                jsonString = jsonString.Replace(item.oldV, item.newV);
            }
            return jsonString.JsonToObject<T>();
        }

        public static T ToUpKeyName<T>(this T source)
        {
            var listName = new List<object>();
            var jsonString = source.JsonToString();
            var jsonSup = jsonString.Split(',');
            var jsonTarget = jsonSup.Where(x => x.LastIndexOf("\":") != -1);
            foreach (string item in jsonTarget)
            {
                var nameBase = item;
                while (nameBase.IndexOf("\":") != -1)
                {
                    var value = nameBase.Substring(0, nameBase.IndexOf("\":"));
                    var name = value.Substring(value.LastIndexOf('"')).Replace("\"", "").Replace("\\", "");
                    var oldV = $"{value}\""; var newV = oldV.Replace(name, name.FirstCharToUpper());
                    listName.Add(new { oldV, newV });
                    nameBase = nameBase.Substring(nameBase.IndexOf("\":") + 2);
                }
            }
            foreach (dynamic item in listName)
            {
                jsonString = jsonString.Replace(item.oldV, item.newV);
            }
            return jsonString.JsonToObject<T>();
        }        

        public static IDictionary<string, object> ToDictionary(this JObject json)
        {
            var propertyValuePairs = json.ToObject<Dictionary<string, object>>();
            ProcessJObjectProperties(propertyValuePairs);
            ProcessJArrayProperties(propertyValuePairs);
            return propertyValuePairs;
        }

        private static void ProcessJObjectProperties(IDictionary<string, object> propertyValuePairs)
        {
            var objectPropertyNames = (from property in propertyValuePairs
                                       let propertyName = property.Key
                                       let value = property.Value
                                       where value is JObject
                                       select propertyName).ToList();

            objectPropertyNames.ForEach(propertyName => propertyValuePairs[propertyName] = ToDictionary((JObject)propertyValuePairs[propertyName]));
        }

        private static void ProcessJArrayProperties(IDictionary<string, object> propertyValuePairs)
        {
            var arrayPropertyNames = (from property in propertyValuePairs
                                      let propertyName = property.Key
                                      let value = property.Value
                                      where value is JArray
                                      select propertyName).ToList();

            arrayPropertyNames.ForEach(propertyName => propertyValuePairs[propertyName] = ToArray((JArray)propertyValuePairs[propertyName]));
        }

        public static object[] ToArray(this JArray array)
        {
            return array.ToObject<object[]>().Select(ProcessArrayEntry).ToArray();
        }

        private static object ProcessArrayEntry(object value)
        {
            if (value is JObject)
            {
                return ToDictionary((JObject)value);
            }
            if (value is JArray)
            {
                return ToArray((JArray)value);
            }
            return value;
        }
    }
}
