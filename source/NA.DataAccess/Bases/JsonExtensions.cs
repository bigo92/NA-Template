using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;

namespace NA.DataAccess.Bases
{
    public static class JsonExtensions
    {
        [DbFunction("JSON_VALUE", Schema = "")]
        public static string JsonValue(string column,[NotParameterized] string path)
        {
            throw new NotSupportedException();
        }
    }
}
