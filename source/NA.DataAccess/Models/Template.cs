using NA.DataAccess.Bases;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace NA.DataAccess.Models
{
    public partial class Template
    {
        public Guid Id { get; set; }
        public virtual InfoJson Info { get; set; }
        public virtual AddressJson Address { get; set; }
        public virtual string DataDb { get; set; }
        public virtual string Files { get; set; }

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
