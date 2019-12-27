using casln.Application.Common.Interfaces;
using System;

namespace casln.WebUI.IntegrationTests
{
    public class TestDateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
