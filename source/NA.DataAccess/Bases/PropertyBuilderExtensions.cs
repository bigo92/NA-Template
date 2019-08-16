using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace NA.DataAccess.Bases
{
    public static class PropertyBuilderExtensions
    {
        public static PropertyBuilder<T> IsJson<T>(this PropertyBuilder<T> build) where T: class
        {
            return build.HasConversion(
                    v => JsonConvert.SerializeObject(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }),
                    v => JsonConvert.DeserializeObject<T>(v, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
        }
    }
}
