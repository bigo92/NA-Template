using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NA.Common.Extentions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tci.common.Enums;

namespace NA.Common.Models
{
    public interface IPagingSizeModel
    {
        string order { get; set; }

        int page { get; set; }

        int size { get; set; }

    }

    public interface IPagingCountModel
    {
        int? totalPage { get; set; }
        long? count { get; set; }
    }

    public class PagingModel : IPagingSizeModel, IPagingCountModel
    {
        
        public int? totalPage { get; set; }
        public long? count { get; set; }
        public string order { get; set; }
        public int page { get; set; }
        public int size { get; set; }

    }    

    public class WhereModel
    {
        public virtual string where { get; set; }

        [JsonIgnore]
        public virtual JObject whereLoopback
        {
            get { return where.HasValue() ? JsonConvert.DeserializeObject<JObject>(where) : null; }
        }

        public virtual string select { get; set; }

        [JsonIgnore]
        public virtual JObject selectLoopback
        {
            get { return select.HasValue() ? JsonConvert.DeserializeObject<JObject>(select) : null; }
        }
    }

    public class SearchModel : WhereModel, IPagingSizeModel
    {
        [JsonIgnore]
        public virtual JArray orderLoopback
        {
            get { return order.HasValue() ? JsonConvert.DeserializeObject<JArray>(order) : null; }
        }

        public virtual string order { get; set; }
        public virtual int page { get; set; }

        private int _size;
        public virtual int size { get { return _size; } set { if (value > 100) { _size = 100; } else { _size = value; } } }
    }

    public class ResultModel<T>
    {
        public SerializableError error { get; set; }

        public T data { get; set; }

        public PagingModel paging { get; set; }
    }

    public class ErrorModel
    {
        public string key { get; set; }

        public string value { get; set; }
    }

    public class FileModel
    {
        public virtual long id { get; set; }

        public virtual string name { get; set; }

        public virtual string extension { get; set; }

        public virtual string flag { get; set; }

        public virtual string url { get; set; }

        public virtual long size { get; set; }

        public virtual DateTime date { get; set; } = DateTime.Now;
    }

    public class DataDb
    {
        public DataDb()
        {
            this.creationTime = DateTime.Now;
            this.status = (byte)Enums.Status_db.Nomal;
        }

        public virtual short status { get; set; }
        public virtual DateTime creationTime { get; set; }
        public virtual long creationBy { get; set; }
        public virtual DateTime? modificationTime { get; set; }
        public virtual long? modificationBy { get; set; }
        public virtual DateTime? delectationTime { get; set; }
        public virtual long? delectationBy { get; set; }
    }
}
