using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NA.Common.Models
{
    public class IPagingModel
    {
        public IPagingModel()
        {
            order = "id desc";
            page = 1;
            size = 25;
        }

        public virtual string order { get; set; }

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
        public int totalPage { get; set; }
        public long count { get; set; }
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

    public class DataDbModel
    {
        public virtual int status { get; set; }

        public virtual DateTime creationTime { get; set; }

        public virtual long creationBy { get; set; }

        public virtual DateTime modificationTime { get; set; }

        public virtual long modificationBy { get; set; }

        public virtual DateTime delectationTime { get; set; }

        public virtual long delectationBy { get; set; }
    }

    public class DataFilesModel
    {
        public virtual Guid id { get; set; }

        public virtual string name { get; set; }

        public virtual string keyName { get; set; }

        public virtual string extension { get; set; }

        public virtual string flag { get; set; }

        public virtual string url { get; set; }

        public virtual long size { get; set; }

        public virtual DateTime date { get; set; }
    }
}
