using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NA.Common.Extentions
{
    public static class EnumExtention
    {
        /// <summary>
        /// Ham lay gia tri string Enums
        /// </summary>
        /// <param name="enumValue">Enums</param>
        /// <returns>string</returns>
        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<DisplayAttribute>()
                            .GetName();
        }

        /// <summary>
        /// Ham lay gia trị string description
        /// </summary>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum enumValue)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])enumValue.GetType().GetField(enumValue.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }

        /// <summary>
        /// Ham chuyen sang list object
        /// </summary>
        /// <typeparam name="T">Enums</typeparam>
        /// <returns>List id-name </returns>
        public static List<object> ToList<T>()
        {
            var result = new List<object>();
            var data = Enum.GetValues(typeof(T)).Cast<T>();
            foreach (var item in data)
            {
                dynamic x = item;
                result.Add(new
                {
                    id = (int)x,
                    name = GetDisplayName(x)
                });
            }
            return result;
        }

        /// <summary>
        /// Ham chuyen sang Dictionary
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>Key-Value</returns>
        public static Dictionary<int, string> ToDictionary<T>()
        {
            var result = new Dictionary<int, string>();
            var data = Enum.GetValues(typeof(T)).Cast<T>();
            foreach (var item in data)
            {
                dynamic x = item;
                result.Add((int)x, GetDisplayName(x));
            }
            return result;
        }

        public static List<dynamic> ToKeyValue<T>()
        {
            var result = new List<dynamic>();
            var data = Enum.GetValues(typeof(T)).Cast<T>();
            foreach (var item in data)
            {
                dynamic x = item;
                result.Add(new
                {
                    id = (int)x,
                    name = x.ToString()
                });
            }
            return result;
        }
    }
}
