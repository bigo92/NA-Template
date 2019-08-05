using System;
using System.Collections.Generic;

namespace NA.DataAccess.ModelsRender
{
    public partial class Template
    {
        public Guid Id { get; set; }
        public string Info { get; set; }
        public string Address { get; set; }
        public string DataDb { get; set; }
        public string Files { get; set; }
    }
}
