using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Na.Common.Enums;
using NA.Common.Extentions;
using NA.Common.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Na.DataAcess.Bases
{
    public static class QueryExtention
    {
        
        public static (DbContext, string) Select(this (DbContext db, string query) source, string select)
        {
            if (select.HasValue())
            {
                if (source.query.IndexOf("[SELECT-BEGIN][SELECT-END]") != -1)
                {
                    source.query = source.query.Insert(source.query.IndexOf("[SELECT-END]"), select);
                }
                else
                {
                    source.query = source.query.Insert(source.query.IndexOf("[SELECT-END]"), $",{select}");
                }
            }
            return source;
        }

        public static (DbContext, string) Select(this DbContext db, string select)
        {
            var query = @"SELECT [SELECT-BEGIN][SELECT-END] 
                [FROM-BEGIN][FROM-END] 
                [WHERE-BEGIN][WHERE-END] 
                [GROUP-BY-BEGIN][GROUP-BY-END] 
                [HAVING-BEGIN][HAVING-END] 
                [ORDER-BY-BEGIN][ORDER-BY-END] 
                [PAGING-BEGIN][PAGING-END]";
            if (select.HasValue())
            {
                query = query.Insert(query.IndexOf("[SELECT-END]"), select);
            }
            return (db, query);
        }

        public static (DbContext, string) Set(this DbContext db, string set)
        {
            var query = @"UPDATE [FROM-BEGIN][FROM-END] [SET-BEGIN][SET-END] [WHERE-BEGIN][WHERE-END]";
            if (set.HasValue())
            {
                query = query.Insert(query.IndexOf("[SET-END]"), set);
            }
            return (db, query);
        }

        public static (DbContext, string) Delete(this DbContext db)
        {
            var query = @"DELETE 
                [FROM-BEGIN][FROM-END] 
                [WHERE-BEGIN][WHERE-END]";
            return (db, query);
        }

        public static (DbContext, string) Insert<T>(this DbContext db, T row, string codeOut = "INSERTED.id")
        {
            var query = $@"INSERT INTO [FROM-BEGIN][FROM-END] ([COLUMN]) output {codeOut}  VALUES ([VALUE])";
            var jsonData = JObject.FromObject(row);
            var column = new List<string>();
            var values = new List<string>();
            foreach (var item in jsonData)
            {
                if (item.Key != "id")
                {
                    column.Add(item.Key);
                    switch (item.Value.Type)
                    {
                        case JTokenType.String:
                            if (item.Key == "data")
                            {
                                values.Add($"JSON_QUERY(N'{item.Value.Value<string>().Replace("'", "''")}')");
                            }
                            else
                            {
                                values.Add($"N'{item.Value.Value<string>().Replace("'", "''")}'");
                            }
                            break;
                        case JTokenType.Date:
                            values.Add($"'{item.Value.Value<DateTime>().ToLocalTime().ToISOString()}'");
                            break;
                        default:
                            values.Add($"{item.Value}");
                            break;
                    }
                }
            }

            query = query.Replace("[COLUMN]", string.Join(",", column));
            query = query.Replace("[VALUE]", string.Join(",", values));

            return (db, query);
        }

        public static (DbContext, string) InsertRange<T>(this DbContext db, List<T> lstRow)
        {
            var query = $@"INSERT INTO [FROM-BEGIN][FROM-END] ([COLUMN]) VALUES ([VALUE])";

            var column = new List<string>();
            var lstValue = new List<string>();

            //add col
            var colData = JObject.FromObject(lstRow[0]);
            foreach (var item in colData)
            {
                if (item.Key != "id")
                {
                    column.Add(item.Key);
                }
            }

            foreach (var r in lstRow)
            {
                var jsonData = JObject.FromObject(r);
                var values = new List<string>();
                foreach (var item in jsonData)
                {
                    if (item.Key != "id")
                    {
                        switch (item.Value.Type)
                        {
                            case JTokenType.String:
                                values.Add($"N'{item.Value}'");
                                break;
                            case JTokenType.Date:
                                values.Add($"'{item.Value.ToObject<DateTime>().ToLocalTime().ToISOString()}'");
                                break;
                            default:
                                values.Add($"{item.Value}");
                                break;
                        }
                    }
                }
                lstValue.Add($"({string.Join(",", values)})");
            }

            query = query.Replace("[COLUMN]", string.Join(",", column));
            query = query.Replace("[VALUE]", string.Join(",", lstValue));

            return (db, query);
        }

        public static async Task<(JArray data, PagingModel paging)> ToPaging(this (DbContext db, string query) source, PagingModel model)
        {
            if (source.query.IndexOf("[ORDER-BY-BEGIN][ORDER-BY-END]") != -1)
            {
                source = source.OrderBy(model.order);
            }

            //check group by
            var queryCount = "";
            if (source.query.IndexOf("[GROUP-BY-BEGIN][GROUP-BY-END]") == -1)
            {
                queryCount = source.query.Substring(0, source.query.IndexOf("[ORDER-BY-BEGIN]"));
                queryCount = $"SELECT COUNT(*) FROM ({queryCount}) as t";
            }
            else
            {
                queryCount = $"SELECT COUNT(*) " + source.query.Substring(source.query.IndexOf("[FROM-BEGIN]"));
                //var queryCount = $"SELECT COUNT(*) FROM (select top {model.size*4 + 1} * " + source.query.Substring(source.query.IndexOf("FROM"));
                queryCount = queryCount.Substring(0, queryCount.IndexOf("[ORDER-BY-BEGIN]"));
            }

            ClearTag(ref queryCount);
            //queryCount += " ) as paging";
            var skip = model.page * model.size - model.size;
            //DUCNV: tong so trang            
            long totalRecord = model.count;//select count
            if (model.page == 1 || totalRecord == 0)
            {
                using (var cm = source.db.Database.GetDbConnection().CreateCommand())
                {
                    cm.CommandText = queryCount;
                    if (source.db.Database.CurrentTransaction != null) cm.Transaction = source.db.Database.CurrentTransaction.GetDbTransaction();
                    await source.db.Database.OpenConnectionAsync();
                    using (var r = await cm.ExecuteReaderAsync())
                    {
                        if (r.HasRows)
                        {
                            while (await r.ReadAsync())
                            {
                                totalRecord = (int)r.GetValue(0);
                            }
                        }
                    }
                }
            }
            int totalPage = (int)(totalRecord / model.size);
            if (totalRecord % model.size != 0)
            {
                totalPage++;
            }
            var data = new JArray();
            source.query = source.query.Insert(source.query.IndexOf("[PAGING-END]"), $"OFFSET {skip} ROWS FETCH NEXT {model.size} ROWS ONLY");
            ClearTag(ref source.query);
            using (var cm = source.db.Database.GetDbConnection().CreateCommand())
            {
                cm.CommandText = source.query;
                if (source.db.Database.CurrentTransaction != null) cm.Transaction = source.db.Database.CurrentTransaction.GetDbTransaction();
                await source.db.Database.OpenConnectionAsync();
                using (var r = await cm.ExecuteReaderAsync())
                {
                    if (r.HasRows)
                    {
                        while (await r.ReadAsync())
                        {
                            var row = new JObject();
                            for (int i = 0; i < r.FieldCount; i++)
                            {
                                row.Add(r.GetName(i), JToken.FromObject(r.GetValue(i)));
                            }
                            data.Add(row);
                        }
                    }
                }
            }
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

        public static (DbContext, string) Where(this (DbContext db, string query) source, JObject whereString = null, string tagAs = "")
        {
            tagAs = tagAs.HasValue() ? $"{tagAs}." : "";
            var where = new List<string>();
            if (whereString != null)
            {
                try
                {
                    var where_av = whereString.RenderFilter(tagAs);
                    if (where_av.HasValue())
                    {
                        where.Add(where_av);
                    }
                }
                catch (Exception) { }
            }
            if (whereString == null || !whereString.ToString().ToLower().Contains("data_db.status"))
            {
                where.Add($"JSON_VALUE({tagAs}data_db,'$.status') = {(byte)Enums.Status_db.Nomal }");
            }
            if (where.Count > 0)
            {
                var part = source.query.IndexOf("[WHERE-BEGIN][WHERE-END]") == -1 ? " AND " : "";
                foreach (var w in where)
                {
                    source.query = source.query.Insert(source.query.IndexOf("[WHERE-END]"), $"{part}({w})");
                    part = " AND ";
                }
            }

            return source;
        }

        public static (DbContext db, string query) WhereRaw(this (DbContext db, string query) source, string where = null)
        {
            if (where.HasValue())
            {
                if (source.query.IndexOf("[WHERE-BEGIN][WHERE-END]") != -1)
                {
                    source.query = source.query.Insert(source.query.IndexOf("[WHERE-END]"), $"({where})");
                }
                else
                {
                    source.query = source.query.Insert(source.query.IndexOf("[WHERE-END]"), $" AND ({where})");
                }
            }
            return source;
        }

        public static (DbContext, string) GroupBy(this (DbContext db, string query) source, string group = null)
        {
            if (group.HasValue())
            {
                if (source.query.IndexOf("[GROUP-BY-BEGIN][GROUP-BY-END]") != -1)
                {
                    source.query = source.query.Insert(source.query.IndexOf("[GROUP-BY-END]"), group);
                }
                else
                {
                    source.query = source.query.Insert(source.query.IndexOf("[GROUP-BY-END]"), $",{group}");
                }
            }
            return source;
        }

        public static (DbContext, string) Having(this (DbContext db, string query) source, string having = null)
        {
            if (having.HasValue())
            {
                if (source.query.IndexOf("[HAVING-BEGIN][HAVING-END]") != -1)
                {
                    source.query = source.query.Insert(source.query.IndexOf("[HAVING-END]"), having);
                }
                else
                {
                    source.query = source.query.Insert(source.query.IndexOf("[HAVING-END]"), $",{having}");
                }
            }
            return source;
        }

        public static (DbContext, string) OrderBy(this (DbContext db, string query) source, string order)
        {
            if (order.HasValue())
            {
                if (source.query.IndexOf("[ORDER-BY-BEGIN][ORDER-BY-END]") != -1)
                {
                    source.query = source.query.Insert(source.query.IndexOf("[ORDER-BY-END]"), order);
                }
                else
                {
                    source.query = source.query.Insert(source.query.IndexOf("[ORDER-BY-END]"), $",{order}");
                }
            }
            return source;
        }

        public static (DbContext, string) From(this (DbContext db, string query) source, string from)
        {
            if (from.HasValue())
            {
                source.query = source.query.Insert(source.query.IndexOf("[FROM-END]"), from);
            }
            return source;
        }

        public static (DbContext db, string query) From<T>(this (DbContext db, string query) source)
        {
            var mapping = source.db.Model.FindEntityType(typeof(T)).Relational();
            source.query = source.query.Insert(source.query.IndexOf("[FROM-END]"), $"{mapping.Schema}.{mapping.TableName}");
            return source;
        }

        private static void ClearTag(ref string query)
        {
            if (query.StartsWith("SELECT"))
            {
                //clear select
                if (query.IndexOf("[SELECT-BEGIN][SELECT-END]") != -1)
                {
                    query = query.Replace("[SELECT-BEGIN][SELECT-END]", "*");
                }
                else if (query.IndexOf("[SELECT-BEGIN]") != -1)
                {
                    query = query.Replace("[SELECT-BEGIN]", "");
                    query = query.Replace("[SELECT-END]", "");
                }
                //clear from
                if (query.IndexOf("[FROM-BEGIN]") != -1)
                {
                    query = query.Replace("[FROM-BEGIN]", "FROM ");
                    query = query.Replace("[FROM-END]", "");
                }
                //clear where
                if (query.IndexOf("[WHERE-BEGIN][WHERE-END]") != -1)
                {
                    query = query.Replace("[WHERE-BEGIN]", "");
                    query = query.Replace("[WHERE-END]", "");
                }
                else if (query.IndexOf("[WHERE-BEGIN]") != -1)
                {
                    query = query.Replace("[WHERE-BEGIN]", "WHERE ");
                    query = query.Replace("[WHERE-END]", "");
                }
                //clear group by
                if (query.IndexOf("[GROUP-BY-BEGIN][GROUP-BY-END]") != -1)
                {
                    query = query.Replace("[GROUP-BY-BEGIN]", "");
                    query = query.Replace("[GROUP-BY-END]", "");
                }
                else if (query.IndexOf("[GROUP-BY-BEGIN]") != -1)
                {
                    query = query.Replace("[GROUP-BY-BEGIN]", "GROUP BY ");
                    query = query.Replace("[GROUP-BY-END]", "");
                }
                //clear having
                if (query.IndexOf("[HAVING-BEGIN][HAVING-END]") != -1)
                {
                    query = query.Replace("[HAVING-BEGIN]", "");
                    query = query.Replace("[HAVING-END]", "");
                }
                else if (query.IndexOf("[HAVING-BEGIN]") != -1)
                {
                    query = query.Replace("[HAVING-BEGIN]", "HAVING ");
                    query = query.Replace("[HAVING-END]", "");
                }
                //clear order by
                if (query.IndexOf("[ORDER-BY-BEGIN][ORDER-BY-END]") != -1)
                {
                    query = query.Replace("[ORDER-BY-BEGIN]", "");
                    query = query.Replace("[ORDER-BY-END]", "");
                }
                else if (query.IndexOf("[ORDER-BY-BEGIN]") != -1)
                {
                    query = query.Replace("[ORDER-BY-BEGIN]", "ORDER BY ");
                    query = query.Replace("[ORDER-BY-END]", "");
                }
                //clear PAGING
                if (query.IndexOf("[PAGING-BEGIN]") != -1)
                {
                    query = query.Replace("[PAGING-BEGIN]", "");
                    query = query.Replace("[PAGING-END]", "");
                }
            }
            else if (query.StartsWith("UPDATE"))
            {
                //clear from
                if (query.IndexOf("[FROM-BEGIN]") != -1)
                {
                    query = query.Replace("[FROM-BEGIN]", "");
                    query = query.Replace("[FROM-END]", "");
                }
                //clear where
                if (query.IndexOf("[WHERE-BEGIN][WHERE-END]") != -1)
                {
                    query = query.Replace("[WHERE-BEGIN]", "");
                    query = query.Replace("[WHERE-END]", "");
                }
                else if (query.IndexOf("[WHERE-BEGIN]") != -1)
                {
                    query = query.Replace("[WHERE-BEGIN]", "WHERE ");
                    query = query.Replace("[WHERE-END]", "");
                }
                //clear set
                if (query.IndexOf("[SET-BEGIN]") != -1)
                {
                    query = query.Replace("[SET-BEGIN]", "SET ");
                    query = query.Replace("[SET-END]", "");
                }
            }
            else if (query.StartsWith("DELETE"))
            {
                //clear from
                if (query.IndexOf("[FROM-BEGIN]") != -1)
                {
                    query = query.Replace("[FROM-BEGIN]", "");
                    query = query.Replace("[FROM-END]", "");
                }
                //clear where
                if (query.IndexOf("[WHERE-BEGIN][WHERE-END]") != -1)
                {
                    query = query.Replace("[WHERE-BEGIN]", "");
                    query = query.Replace("[WHERE-END]", "");
                }
                else if (query.IndexOf("[WHERE-BEGIN]") != -1)
                {
                    query = query.Replace("[WHERE-BEGIN]", "WHERE ");
                    query = query.Replace("[WHERE-END]", "");
                }
            }
            else if (query.StartsWith("INSERT"))
            {
                //clear from
                if (query.IndexOf("[FROM-BEGIN]") != -1)
                {
                    query = query.Replace("[FROM-BEGIN]", "");
                    query = query.Replace("[FROM-END]", "");
                }
            }
        }

        public static async Task<JArray> Excute(this (DbContext db, string query) source)
        {
            var result = new JArray();
            ClearTag(ref source.query);
            using (var cm = source.db.Database.GetDbConnection().CreateCommand())
            {
                cm.CommandText = source.query;
                if (source.db.Database.CurrentTransaction != null) cm.Transaction = source.db.Database.CurrentTransaction.GetDbTransaction();
                await source.db.Database.OpenConnectionAsync();
                using (var r = await cm.ExecuteReaderAsync())
                {
                    if (r.HasRows)
                    {
                        while (await r.ReadAsync())
                        {
                            var row = new JObject();
                            for (int i = 0; i < r.FieldCount; i++)
                            {
                                row.Add(r.GetName(i), JToken.FromObject(r.GetValue(i)));
                            }
                            result.Add(row);
                        }
                    }
                }
            }

            return result;
        }

        public static (DbContext, string) Query(this DbContext db, string query)
        {
            return (db, query);
        }

        public static async Task<T> First<T>(this (DbContext db, string query) source)
        {
            JObject result = null;
            source.query = source.query.Replace("SELECT", "SELECT TOP(1) ");
            ClearTag(ref source.query);
            using (var cm = source.db.Database.GetDbConnection().CreateCommand())
            {
                cm.CommandText = source.query;
                if (source.db.Database.CurrentTransaction != null) cm.Transaction = source.db.Database.CurrentTransaction.GetDbTransaction();
                await source.db.Database.OpenConnectionAsync();
                using (var r = await cm.ExecuteReaderAsync())
                {
                    if (r.HasRows)
                    {
                        while (await r.ReadAsync())
                        {
                            var row = new JObject();
                            for (int i = 0; i < r.FieldCount; i++)
                            {
                                row.Add(r.GetName(i), JToken.FromObject(r.GetValue(i)));
                            }
                            result = row;
                        }
                    }
                }
            }

            return result.ToObject<T>();
        }

        public static async Task<JObject> First(this (DbContext db, string query) source)
        {
            JObject result = null;
            source.query = $"SELECT TOP (1) {source.query.Substring(6)}";
            ClearTag(ref source.query);
            using (var cm = source.db.Database.GetDbConnection().CreateCommand())
            {
                cm.CommandText = source.query;
                if (source.db.Database.CurrentTransaction != null) cm.Transaction = source.db.Database.CurrentTransaction.GetDbTransaction();
                await source.db.Database.OpenConnectionAsync();
                using (var r = await cm.ExecuteReaderAsync())
                {
                    if (r.HasRows)
                    {
                        while (await r.ReadAsync())
                        {
                            var row = new JObject();
                            for (int i = 0; i < r.FieldCount; i++)
                            {
                                row.Add(r.GetName(i), JToken.FromObject(r.GetValue(i)));
                            }
                            result = row;
                        }
                    }
                }
            }

            return result;
        }

        public static string AsQuery(this (DbContext db, string query) source)
        {
            ClearTag(ref source.query);
            return source.query;
        }

        public static async Task<(string, PagingModel)> AsQueryPaging(this (DbContext db, string query) source, PagingModel model)
        {
            if (source.query.IndexOf("[ORDER-BY-BEGIN][ORDER-BY-END]") != -1)
            {
                source = source.OrderBy(model.order);
            }

            //check group by
            var queryCount = "";
            if (source.query.IndexOf("[GROUP-BY-BEGIN][GROUP-BY-END]") == -1)
            {
                queryCount = source.query.Substring(0, source.query.IndexOf("[ORDER-BY-BEGIN]"));
                queryCount = $"SELECT COUNT(*) FROM ({queryCount}) as t";
            }
            else
            {
                queryCount = $"SELECT COUNT(*) " + source.query.Substring(source.query.IndexOf("[FROM-BEGIN]"));
                //var queryCount = $"SELECT COUNT(*) FROM (select top {model.size*4 + 1} * " + source.query.Substring(source.query.IndexOf("FROM"));
                queryCount = queryCount.Substring(0, queryCount.IndexOf("[ORDER-BY-BEGIN]"));
            }

            ClearTag(ref queryCount);
            //queryCount += " ) as paging";
            var skip = model.page * model.size - model.size;
            //DUCNV: tong so trang            
            long totalRecord = model.count;//select count
            if (model.page == 1 || totalRecord == 0)
            {
                using (var cm = source.db.Database.GetDbConnection().CreateCommand())
                {
                    cm.CommandText = queryCount;
                    if (source.db.Database.CurrentTransaction != null) cm.Transaction = source.db.Database.CurrentTransaction.GetDbTransaction();
                    await source.db.Database.OpenConnectionAsync();
                    using (var r = await cm.ExecuteReaderAsync())
                    {
                        if (r.HasRows)
                        {
                            while (await r.ReadAsync())
                            {
                                totalRecord = (int)r.GetValue(0);
                            }
                        }
                    }
                }
            }
            int totalPage = (int)(totalRecord / model.size);
            if (totalRecord % model.size != 0)
            {
                totalPage++;
            }

            source.query = source.query.Insert(source.query.IndexOf("[PAGING-END]"), $"OFFSET {skip} ROWS FETCH NEXT {model.size} ROWS ONLY");
            ClearTag(ref source.query);
           
            var paging = new PagingModel()
            {
                page = model.page,
                size = model.size,
                totalPage = totalPage,
                count = totalRecord,
                order = model.order
            };
            return (source.query, paging);
        }

        public static async Task<int> ExecuteNonQuery(this (DbContext db, string query) source)
        {
            ClearTag(ref source.query);
            int result = 0;
            using (var cm = source.db.Database.GetDbConnection().CreateCommand())
            {
                cm.CommandText = source.query;
                cm.CommandTimeout = 500;
                if (source.db.Database.CurrentTransaction != null) cm.Transaction = source.db.Database.CurrentTransaction.GetDbTransaction();
                await source.db.Database.OpenConnectionAsync();
                result = await cm.ExecuteNonQueryAsync();
            }

            return result;
        }

        public static async Task<T> ExecuteScalar<T>(this (DbContext db, string query) source)
        {
            ClearTag(ref source.query);
            T result = default(T);
            using (var cm = source.db.Database.GetDbConnection().CreateCommand())
            {
                cm.CommandText = source.query;
                if (source.db.Database.CurrentTransaction != null) cm.Transaction = source.db.Database.CurrentTransaction.GetDbTransaction();
                await source.db.Database.OpenConnectionAsync();
                result = (T)await cm.ExecuteScalarAsync();
            }

            return result;
        }

        public static async Task<long> Count(this (DbContext db, string query) source)
        {
            var queryCount = $"SELECT COUNT(*) " + source.query.Substring(source.query.IndexOf("[FROM-BEGIN]"));
            if (queryCount.IndexOf("[ORDER-BY-END]") != -1)
            {
                queryCount = queryCount.Substring(0, queryCount.IndexOf("[ORDER-BY-BEGIN]"));
            }

            ClearTag(ref queryCount);
            //DUCNV: tong so trang            
            int totalRecord = 0;//select count
            using (var cm = source.db.Database.GetDbConnection().CreateCommand())
            {
                cm.CommandText = queryCount;
                if (source.db.Database.CurrentTransaction != null) cm.Transaction = source.db.Database.CurrentTransaction.GetDbTransaction();
                await source.db.Database.OpenConnectionAsync();
                using (var r = await cm.ExecuteReaderAsync())
                {
                    if (r.HasRows)
                    {
                        while (await r.ReadAsync())
                        {
                            totalRecord = (int)r.GetValue(0);
                        }
                    }
                }
            }
            return totalRecord;
        }
    }
}
