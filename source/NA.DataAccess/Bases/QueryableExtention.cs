using NA.Common.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NA.Common.Extentions
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

        public static IQueryable<T> WhereLoopback<T>(this IQueryable<T> source, JObject where, string type = "and")
        {
            var parameter = Expression.Parameter(typeof(T), "x");

            var expression = ExpressionAnd(parameter, where);

            var lambda = Expression.Lambda<Func<T, bool>>(expression, parameter);
            source = source.Where(lambda);

            return source;
        }

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
                foreach (var item in param)
                {
                    var k = item.Key.ToString().Split('.');
                    var v = item.Value;
                    var t = v.GetType();

                    var field = "";
                    var pathJson = "$";
                    if (k.Length == 1)
                    {
                        field = k[0];
                    }
                    else
                    {
                        for (int j = 0; j < k.Length; j++)
                        {
                            if (j == 0)
                            {
                                field = k[j];
                            }
                            else
                            {
                                pathJson += $".{k[j]}";
                            }
                        }
                    }

                    Expression property = default;
                    if (pathJson == "$")
                    {
                        property = Expression.Property(table, field);
                    }
                    else
                    {
                        var p = Expression.Property(table, field);
                        var jsonFC = typeof(NA.DataAccess.Bases.DbFunction).GetMethods().First(m => m.Name == "JsonValue" && m.GetParameters().Length == 2);
                        property = Expression.Call(jsonFC, p, Expression.Constant(pathJson));
                    }

                    if (t == typeof(JObject))
                    {
                        foreach (var v2value in v as JObject)
                        {
                            var valueEx = Convert.ChangeType(v2value.Value, property.Type);
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
                        var valueEx = Convert.ChangeType(v, property.Type);
                        expression = Expression.Equal(property, Expression.Constant(valueEx));
                    }
                }
            }

            return expression;
        }

        public static IQueryable<T> WhereDynamic<T>(this IQueryable<T> source,
                                       string property,
                                       string value) where T : class
        {
            //STEP 1: Validate MORE!
            var searchProperty = typeof(T).GetProperty(property);
            if (searchProperty == null) throw new ArgumentException("property");

            //STEP 2: Create property selector
            var parameter = Expression.Parameter(typeof(T), "x");
            var equal = Expression.Equal(Expression.Property(parameter, property), Expression.Constant(value));

            //STEP 3: Update the IQueryable expression to include OrderBy
            var lambda = Expression.Lambda<Func<T, bool>>(equal, parameter);

            return source.Where(lambda);
        }

        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source,
                                       string property,
                                       bool asc = true) where T : class
        {
            //STEP 1: Validate MORE!
            var searchProperty = typeof(T).GetProperty(property);
            if (searchProperty == null) throw new ArgumentException("property");

            //STEP 2: Create the OrderBy property selector
            var parameter = Expression.Parameter(typeof(T), "o");
            var selectorExpr = Expression.Lambda(Expression.Property(parameter, property), parameter);

            //STEP 3: Update the IQueryable expression to include OrderBy
            Expression queryExpr = source.Expression;
            queryExpr = Expression.Call(typeof(Queryable), asc ? "OrderBy" : "OrderByDescending",
                                          new Type[] { source.ElementType, searchProperty.PropertyType },
                                         queryExpr,
                                        selectorExpr);

            return source.Provider.CreateQuery<T>(queryExpr);
        }
    }
}
