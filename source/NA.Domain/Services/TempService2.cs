using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using NA.DataAccess.Contexts;
using NA.Domain.Bases;
using NA.Domain.Interfaces;

namespace NA.Domain.Services
{
    public interface ITempService2 : ICRUDService<Template>
    {

    }

    public class TempService2 : ITempService2, IDisposable
    {
        private readonly ILogger<TempService2> _log;
        public TempService2(ILogger<TempService2> log)
        {
            _log = log;
        }



        public string FindOne()
        {
            return "1231";
        }

        public void Add(Template model)
        {
            throw new NotImplementedException();
        }

        public void Delete(Template model)
        {
            throw new NotImplementedException();
        }

        public void Edit(Template model)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _log.LogError("Dispose Service 2");
        }

        public List<Template> Get()
        {
            throw new NotImplementedException();
        }
    }
}
