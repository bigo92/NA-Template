using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace NA.Common.Interfaces
{
    public interface IModule
    {
        void ConfigureServices(IServiceCollection services);
    }
}
