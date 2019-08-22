using Microsoft.SqlServer.Management.Smo;
using NA.Common.Extentions;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace NA.WebApi.Bases.Swagger
{
    public class SwaggerFilter : ISchemaFilter
    {
        public void Apply(Swashbuckle.AspNetCore.Swagger.Schema model, SchemaFilterContext context)
        {
            if (model?.Properties == null)
                return;

            var excludedProperties = context.SystemType.GetProperties()
                                         .Where(t => t.GetCustomAttribute<IngoreAttribute>() != null);

            foreach (var excludedProperty in excludedProperties)
            {
                if (model.Properties.ContainsKey(excludedProperty.Name.FirstCharToLoower()))
                    model.Properties.Remove(excludedProperty.Name.FirstCharToLoower());
            }
        }
    }
}
