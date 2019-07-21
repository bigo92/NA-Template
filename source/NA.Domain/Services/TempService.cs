using System;
using NA.Domain.Interfaces;
using NA.Domain.Bases;
using Microsoft.Extensions.Logging;

namespace NA.Domain.Services
{
    public interface ITempService : ICRUDService
    {

    }

    public class TempService : ITempService, IDisposable
    {
        private readonly IDispatcherFactory _dp;
        private readonly ILogger<TempService> _log;
        public TempService(IDispatcherFactory dp, ILogger<TempService> log, string id)
        {
            _dp = dp;_log = log;
            log.LogError($"TempService:${id}");
        }
        public void Get<T>(T model)
        {
            throw new NotImplementedException();
        }

        public string FindOne()
        {
            return _dp.Service<TempService2>().FindOne();
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
            _log.LogError("Dispose Service");
        }
    }
}
