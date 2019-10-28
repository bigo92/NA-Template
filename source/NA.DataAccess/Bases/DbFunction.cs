using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;

namespace NA.DataAccess.Bases
{
    public static class DbFunction
    {     
        public static string JsonValue(string column,[NotParameterized] string path)
        {
            throw new NotSupportedException();
        }
    }
}
