namespace NA.Domain.Cache
{
    using System;

    public static class CacheSetting
    {
        public static class KhachHang
        {
            public const string Key = "KhachHang";
            public static readonly TimeSpan SlidingExpiration = TimeSpan.FromDays(30);
            public static readonly TimeSpan SlidingUpdate = TimeSpan.FromDays(5);
        }
    }
}