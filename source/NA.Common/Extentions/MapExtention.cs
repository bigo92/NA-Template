using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace NA.Common.Extentions
{
    public static class MapExtention
    {
        public static T Map<T>(this object source, T toSource)
        {
            if (source.GetType() == typeof(Newtonsoft.Json.Linq.JObject) && typeof(T) == typeof(Newtonsoft.Json.Linq.JObject))
            {
                var result = new Newtonsoft.Json.Linq.JObject();
                foreach (var src in source as Newtonsoft.Json.Linq.JObject)
                {
                    var srcName = src.Key.FirstCharToLoower();

                    if ((toSource as Newtonsoft.Json.Linq.JObject).ContainsKey(srcName))
                    {
                        (source as Newtonsoft.Json.Linq.JObject)[src.Key] = (toSource as Newtonsoft.Json.Linq.JObject).GetValue(srcName);
                    }
                }
                return toSource;
            }
            // other
            var propertyInfos = toSource.GetType().GetProperties();
            foreach (var pr in propertyInfos)
            {
                var name = pr.Name.ToLower();
                var value = pr.GetValue(toSource, null);
                var type = pr.PropertyType;
                var sourceProperty = source.GetType().GetProperties();
                foreach (var src in sourceProperty)
                {
                    var srcName = src.Name.ToLower();
                    var srcValue = src.GetValue(source, null);
                    var srcType = src.PropertyType;

                    if (name == srcName)
                    {
                        if (type == srcType)
                        {
                            pr.SetValue(toSource, srcValue);
                        }
                        else
                        {
                            if (srcType == Type.GetType("System.String"))
                            {
                                pr.SetValue(toSource, srcValue.ToString());
                            }
                        }
                    }
                }
            }
            return toSource;
        }

        public static T Map<T>(this IDictionary<string, object> source, T toSource)
        {
            if (typeof(T) == typeof(Dictionary<string, object>))
            {
                var result = new Dictionary<string, object>();
                foreach (var src in source)
                {
                    var srcName = src.Key.FirstCharToLoower();
                    var srcValue = src.Value;

                    (toSource as Dictionary<string, object>).Add(srcName, srcValue);
                }
                return toSource;
            }

            var propertyInfos = toSource.GetType().GetProperties();
            foreach (var pr in propertyInfos)
            {
                var name = pr.Name.ToLower();
                var value = pr.GetValue(toSource, null);
                var type = pr.PropertyType;
                foreach (var src in source)
                {
                    var srcName = src.Key.ToLower();
                    var srcValue = src.Value;
                    var srcType = src.Value.GetType();

                    if (name == srcName)
                    {
                        if (type == srcType)
                        {
                            pr.SetValue(toSource, srcValue);
                        }
                        else
                        {
                            if (srcType == Type.GetType("System.String"))
                            {
                                pr.SetValue(toSource, srcValue.ToString());
                            }
                        }
                    }
                }
            }
            return toSource;
        }

        public static T MapObject<T>(this Newtonsoft.Json.Linq.JObject source, T toSource) where T : class
        {
            var propertyInfos = toSource.GetType().GetProperties();
            foreach (var pr in propertyInfos)
            {
                var value = pr.GetValue(toSource, null);
                var type = pr.PropertyType;
                if (pr.Name.StartsWith("_"))
                {
                    var name = pr.Name.Substring(1).FirstCharToLoower();
                    if (name == "dataDb")
                    {
                        name = "data_db";
                    }
                    if (source.ContainsKey(name) && HasKey(propertyInfos, $"{pr.Name.Substring(1)}"))
                    {
                        pr.SetValue(toSource, source[name].ToObject(type));
                    }
                }
                else
                {
                    var name = pr.Name.FirstCharToLoower();
                    if (source.ContainsKey(name) && !HasKey(propertyInfos, $"_{pr.Name}"))
                    {
                        pr.SetValue(toSource, source[name].ToObject(type));
                    }
                }
            }
            return toSource;
        }

        public static T Patch<T>(this JObject source, T toSource)
        {
            var result = JObject.FromObject(toSource);
            foreach (var pr in result)
            {
                foreach (var s in source)
                {
                    var sName = s.Key.Split(".");
                    if (sName.Length == 1)
                    {
                        if (pr.Key.ToLower().Equals(sName[0].ToLower()))
                        {
                            result[pr.Key] = s.Value;
                        }
                    }
                    else
                    {
                        if (pr.Key.ToLower().Equals(sName[0].ToLower()) && pr.Value.GetType() == typeof(Newtonsoft.Json.Linq.JObject))
                        {
                            foreach (var c in pr.Value as Newtonsoft.Json.Linq.JObject)
                            {
                                if (c.Key.ToLower().Equals(sName[1].ToLower()))
                                {
                                    (pr.Value as Newtonsoft.Json.Linq.JObject)[c.Key] = s.Value;
                                }
                            }
                            if (result.ContainsKey($"_{pr.Key}"))
                            {
                                result[$"_{pr.Key}"] = pr.Value.JsonToString();
                            }
                        }
                    }
                }
            }
            return result.ToObject<T>();
        }

        public static string Patch(this JObject source, string toSource)
        {
            var result = toSource.JsonToObject<JObject>();

            foreach (var s in source)
            {
                if (result.ContainsKey(s.Key))
                {
                    result[s.Key] = s.Value;
                }
                else
                {
                    result.Add(s.Key, s.Value);
                }                
            }
            return result.JsonToString();
        }

        private static bool HasKey(System.Reflection.PropertyInfo[] pro, string key)
        {
            var result = false;
            foreach (var p in pro)
            {
                if (p.Name == key)
                {
                    result = true;
                }
            }
            return result;
        }
    }
}
