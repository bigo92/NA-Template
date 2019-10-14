using System;
using System.Collections.Generic;

namespace NA.DataAccess.ModelsRender
{
    public partial class Template
    {
        public Guid Id { get; set; }
        public string DataDb { get; set; }
        public string Files { get; set; }
        public string Data { get; set; }
        public string Language { get; set; }
        public string Tag { get; set; }
    }
}
