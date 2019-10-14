using NA.Common.Models;
using NA.DataAccess.Bases;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace NA.DataAccess.Contexts
{
    public partial class Template
    {
        public virtual Guid id { get; set; } = Guid.NewGuid();
        public virtual DataDb data_db { get; set; }
        public virtual List<FileModel> files { get; set; }
        public virtual DataJson data { get; set; }
        public virtual string language { get; set; }
        public virtual string tag { get; set; }

        public class DataJson
        {
            public virtual string name { get; set; }
        }
    }
}