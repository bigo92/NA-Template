using casln.Application.Common.Interfaces;
using System;

namespace casln.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
