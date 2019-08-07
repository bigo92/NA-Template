using System;
using Microsoft.Extensions.Logging;
using NA.Domain.Bases;
using NA.Domain.Interfaces;

namespace NA.Domain.Services
{
    public interface ITempService2 : ICRUDService
    {

    }

    public class TempService2 : ITempService2, IDisposable
    {
        private readonly ILogger<TempService2> _log;
        public TempService2(ILogger<TempService2> log)
        {
            _log = log;
        }

        public void Get<T>(T model)
        {
            throw new NotImplementedException();
        }

        public string FindOne()
        {
            return "1231";
        }

        public void Add<T>(T model)
        {
            throw new NotImplementedException();
        }

        public void Delete<T>(T model)
        {
            throw new NotImplementedException();
        }

        public void Edit<T>(T model)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _log.LogError("Dispose Service 2");
        }
    }
}
