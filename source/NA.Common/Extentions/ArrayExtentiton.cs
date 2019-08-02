using System;
using System.Collections.Generic;
using System.Text;

namespace NA.Common.Extentions
{
    public class ArrayExtentiton
    {
        public static bool IsTypeBase(dynamic source)
        {
            var typeArray = source.GetType().GetGenericArguments()[0];
            if (typeArray == typeof(string) ||
                typeArray == typeof(bool) ||
                typeArray == typeof(byte) ||
                typeArray == typeof(int) ||
                typeArray == typeof(long) ||
                typeArray == typeof(float) ||
                typeArray == typeof(double) ||
                typeArray == typeof(decimal) ||
                typeArray == typeof(DateTime))
            {
                return true;
            }
            return false;
        }
    }
}
