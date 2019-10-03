using NA.DataAccess.Bases;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace NA.DataAccess.Models
{
    public partial class Template
    {
        public virtual Guid id { get; set; } = Guid.NewGuid();
        public virtual InfoJson info { get; set; }
        public virtual AddressJson address { get; set; }
        public virtual string data_db { get; set; }
        public virtual string files { get; set; }

        public class InfoJson
        {
            public virtual string name { get; set; }

            public virtual int age { get; set; }
        }

        public class AddressJson
        {
            public virtual string address1 { get; set; }

            public virtual string address2 { get; set; }

            public virtual string address3 { get; set; }
        }
    }
}
