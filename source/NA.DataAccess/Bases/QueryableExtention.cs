using NA.Common.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NA.DataAccess.Bases
{
    public static class QueryableExtention
    {
        public static (IList<T> data, PagingModel paging) ToPaging<T>(this IQueryable<T> source, IPagingModel model)
        {
            var skip = model.page * model.size - model.size;
            //DUCNV: tong so trang
            var totalRecord = source.Count();//select count
            var totalPage = totalRecord / model.size;
            if (totalRecord % model.size != 0)
            {
                totalPage++;
            }
            var data = source.Skip(skip).Take(model.size).ToList();
            var paging = new PagingModel()
            {
                page = model.page,
                size = model.size,
                totalPage = totalPage,
                count = totalRecord,
                order = model.order
            };
            return (data, paging);
        }

        public static IQueryable<T> WhereLoopback<T>(this IQueryable<T> source, JObject where)
        {
            var parameter = Expression.Parameter(typeof(T), "x");

            var expression = ExpressionAnd(parameter, where);

            var lambda = Expression.Lambda<Func<T, bool>>(expression, parameter);
            source = source.Where(lambda);
           
            return source;
        }

        public static IQueryable<T> OrderByLoopback<T>(this IQueryable<T> source, JArray order) where T : class
        {
            var table = Expression.Parameter(typeof(T), "x");
            var fisrt = true;
            foreach (JObject param in order)
            {
                var resultKey = param.ReadKeyObject();

                Expression property = default;
                Type propertyType = default;

                if (resultKey.pathJson == "$")
                {
                    property = GetProperty(table, resultKey.property);
                    propertyType = property.Type;
                }
                else
                {
                    var propertyDetail = GetProperty(table, resultKey.property);
                    propertyType = propertyDetail.Type;

                    var propertyJson = Expression.Property(table, resultKey.field);
                    var jsonFC = typeof(DbFunction).GetMethods().First(m => m.Name == "JsonValue" && m.GetParameters().Length == 2);
                    var jsonValue = Expression.Convert(Expression.Convert(propertyJson, typeof(object)), typeof(string));
                    property = Expression.Convert(Expression.Convert(Expression.Call(jsonFC, jsonValue, Expression.Constant(resultKey.pathJson)), typeof(object)), propertyType);
                }

                var queryExpr = source.Expression;
                var selectorExpr = Expression.Lambda(property, table);
                var funcName = fisrt ? resultKey.value.Value<bool>() ? "OrderBy" : "OrderByDescending" : resultKey.value.Value<bool>() ? "ThenBy" : "ThenByDescending";
                queryExpr = Expression.Call(typeof(Queryable), funcName,
                                      new Type[] { source.ElementType, propertyType },
                                     queryExpr,
                                    selectorExpr);
                source = source.Provider.CreateQuery<T>(queryExpr);
                fisrt = false;
            }

            return source;
        }

        //-- private func extension
        private static Expression ExpressionAnd(ParameterExpression table, JObject param)
        {
            Expression expression = default;

            if (param.ContainsKey("and") || param.ContainsKey("or"))
            {
                var key = param.ContainsKey("and") ? "and" : "or";
                var value = param.GetValue(key) as JArray;

                foreach (JObject item in value)
                {
                    var query = ExpressionAnd(table, item);
                    if (key == "and")
                    {
                        if (expression == null)
                        {
                            expression = query;
                        }
                        else
                        {
                            expression = Expression.AndAlso(expression, query);
                        }

                    }
                    else
                    {
                        if (expression == null)
                        {
                            expression = query;
                        }
                        else
                        {
                            expression = Expression.OrElse(expression, query);
                        }
                    }
                }
            }
            else
            {
                var resultKey = param.ReadKeyObject();

                Expression property = default;
                Type propertyType = default;
                if (resultKey.pathJson == "$")
                {
                    property = Expression.Property(table, resultKey.property);
                    propertyType = property.Type;
                }
                else
                {
                    var propertyDetail = GetProperty(table, resultKey.property);
                    propertyType = propertyDetail.Type;

                    var propertyJson = Expression.Property(table, resultKey.field);
                    var jsonFC = typeof(DbFunction).GetMethods().First(m => m.Name == "JsonValue" && m.GetParameters().Length == 2);
                    var jsonValue = Expression.Convert(Expression.Convert(propertyJson, typeof(object)), typeof(string));
                    property = Expression.Convert(Expression.Convert(Expression.Call(jsonFC, jsonValue, Expression.Constant(resultKey.pathJson)), typeof(object)), propertyType);   
                }

                if (resultKey.value.GetType() == typeof(JObject))
                {
                    foreach (var v2value in resultKey.value.Value<JObject>())
                    {
                        var valueEx = Convert.ChangeType(v2value.Value, propertyType);
                        switch (v2value.Key)
                        {
                            case "gt":
                                expression = Expression.GreaterThan(property, Expression.Constant(valueEx));
                                break;
                            case "gte":
                                expression = Expression.GreaterThanOrEqual(property, Expression.Constant(valueEx));
                                break;
                            case "lt":
                                expression = Expression.LessThan(property, Expression.Constant(valueEx));
                                break;
                            case "lte":
                                expression = Expression.LessThanOrEqual(property, Expression.Constant(valueEx));
                                break;
                            case "neq":
                                expression = Expression.NotEqual(property, Expression.Constant(valueEx));
                                break;
                            case "like":
                                var mi = typeof(string).GetMethods().First(m => m.Name == "Contains" && m.GetParameters().Length == 1);
                                expression = Expression.Call(property, mi, Expression.Constant(valueEx));
                                break;
                            case "nlike":
                                mi = typeof(string).GetMethods().First(m => m.Name == "Contains" && m.GetParameters().Length == 1);
                                expression = Expression.Not(Expression.Call(property, mi, Expression.Constant(valueEx)));
                                break;
                            //case "insplit":
                            //    lstWhere += $"{part}{v2value.Value} in (select * from STRING_SPLIT({rk},','))";
                            //    part = " and ";
                            //    break;
                            //case "ninsplit":
                            //    lstWhere += $"{part}{v2value.Value} not in (select * from STRING_SPLIT({rk},','))";
                            //    part = " and ";
                            //    break;
                            //case "inarray":
                            //    lstWhere += $"{part}{v2value.Value} in (select t.* from  {rk.Replace("JSON_VALUE", "OPENJSON")} WITH(id nvarchar(max) '$') as t)";
                            //    part = " and ";
                            //    break;
                            //case "ninarray":
                            //    lstWhere += $"{part}{v2value.Value} not in (select t.* from  {rk.Replace("JSON_VALUE", "OPENJSON")} WITH(id nvarchar(max) '$') as t)";
                            //    part = " and ";
                            //    break;
                            //case "in":
                            //    lstWhere += $"{part}{rk} in ({string.Join(',', v2value.Value)})";
                            //    part = " and ";
                            //    break;
                            //case "nin":
                            //    lstWhere += $"{part}{rk} not in ({string.Join(',', v2value.Value)})";
                            //    part = " and ";
                            //    break;
                            case "startwith":
                                mi = typeof(string).GetMethods().First(m => m.Name == "StartsWith" && m.GetParameters().Length == 1);
                                expression = Expression.Call(property, mi, Expression.Constant(valueEx));
                                break;
                            case "endwith":
                                mi = typeof(string).GetMethods().First(m => m.Name == "EndsWith" && m.GetParameters().Length == 1);
                                expression = Expression.Call(property, mi, Expression.Constant(valueEx));
                                break;
                        }
                    }
                }
                else
                {
                    var valueEx = Convert.ChangeType(resultKey.value.Value<object>(), propertyType);
                    expression = Expression.Equal(property, Expression.Constant(valueEx));
                }
            }

            return expression;
        }

        private static Expression GetProperty(ParameterExpression table, string property)
        {
            string[] properties = property.Split('.');
            Expression lastMember = table;
            for (int i = 0; i < properties.Length; i++)
            {
                MemberExpression member = Expression.Property(lastMember, properties[i]);
                lastMember = member;
            }
            return lastMember;
        }        

        private static (string field, JToken value, string property, string pathJson) ReadKeyObject(this JObject source)
        {
            var field = "";
            var pathJson = "$";
            var property = "";
            JToken vaule = null;

            foreach (var item in source)
            {
                var k = item.Key.ToString().Split('.');
                vaule = item.Value;

                for (int j = 0; j < k.Length; j++)
                {
                    if (j == 0)
                    {
                        field = k[j];
                        property = k[j];
                    }
                    else
                    {
                        pathJson += $".{k[j]}";
                        property += $".{k[j]}";
                    }
                }
            }
            return (field, vaule, property, pathJson);
        }
    }
}
