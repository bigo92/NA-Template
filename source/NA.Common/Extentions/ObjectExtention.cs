using System;
using System.Collections.Generic;
using System.Text;

namespace NA.Common.Extentions
{
    public static class ObjectExtention
    {
        public static bool IsTypeBase(dynamic source)
        {
            if (source is null || 
                source is string || 
                source is bool || 
                source is byte || 
                source is int || 
                source is long || 
                source is float || 
                source is double || 
                source is decimal || 
                source is DateTime)
            {
                return true;
            }
            return false;
        }
    }
}
