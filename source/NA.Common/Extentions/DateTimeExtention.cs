﻿using System;
using System.Collections.Generic;
using System.Text;

namespace NA.Common.Extentions
{
    public static class DateTimeExtention
    {
        public static long ToUnixEpochDate(this DateTime date)
        {
            return new DateTimeOffset(date).ToUniversalTime().ToUnixTimeSeconds();
        }

        public static string ToISOString(this DateTime time, string format = "o")
        {
            return time.ToString(format);
        }
    }
}
