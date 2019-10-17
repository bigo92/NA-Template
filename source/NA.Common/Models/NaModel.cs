using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tci.common.Enums;

namespace NA.Common.Models
{
    public class IPagingModel
    {
        public IPagingModel()
        {
            order = JArray.FromObject(new List<object> { JObject.FromObject(new {
                id = false
            })});
            page = 1;
            size = 25;
        }

        public virtual JArray order { get; set; }

        public virtual int page { get; set; }

        private int _size;

        public virtual int size
        {
            get { return _size; }
            set
            {
                if (_size != value)
                {
                    _size = value;
                    if (_size >= 100)
                    {
                        _size = 100;
                    }
                }
            }
        }

    }

    public class PagingModel : IPagingModel
    {
        public virtual int? totalPage { get; set; }
        public virtual long? count { get; set; }
    }

    public class SearchModel: IPagingModel
    {
        public virtual JObject where { get; set; }

        public virtual JArray select { get; set; }
    }

    public class ResultModel<T>
    {
        public ResultModel()
        {
            success = true;
        }

        public bool success { get; set; }

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
