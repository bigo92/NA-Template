using NA.Common.Models;
using NA.DataAccess.Contexts;
using System;
using System.Collections.Generic;
using System.Text;
using static NA.DataAccess.Contexts.Template;

namespace NA.Domain.Models
{
    public class Search_TemplateServiceModel: SearchModel
    { 
    
    }

    public class Add_TemplateServiceModel: Template
    {

    }

    public class Edit_TemplateServiceModel: Template
    {
       
    }

    public class Delete_TemplateServiceModel
    {
        public virtual Guid id { get; set; }
    }

    public class Count_TemplateServiceModel: SearchModel
    {      
    }

}
