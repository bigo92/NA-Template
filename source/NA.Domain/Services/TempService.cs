using System;
using NA.Domain.Interfaces;
using NA.Domain.Bases;
using Microsoft.Extensions.Logging;
using NA.DataAccess.Bases;
using NA.DataAccess.Models;
using Newtonsoft.Json.Linq;

namespace NA.Domain.Services
{
    public interface ITempService : ICRUDService
    {

    }

    public class TempService : ITempService, IDisposable
    {
        private readonly IDispatcherFactory _dp;
        private readonly IUnitOfWork _unit;
        private readonly ILogger<TempService> _log;
        public TempService(IDispatcherFactory dp, IUnitOfWork unit, ILogger<TempService> log, string id)
        {
            _dp = dp; _unit = unit; _log = log;
            log.LogError($"TempService:${id}");
        }
        public void Get<T>(T model)
        {
            throw new NotImplementedException();
        }

        public string FindOne()
        {
            return _dp.Service<ITempService2,TempService2>().FindOne();
        }

        public void Add<T>(T model)
        {
            _unit.Repository<Template>().Insert(new Template
            {
                Info = JObject.FromObject(new
                {
                    name = "Nguyen Van A",
                    age = 12
                }).ToString(),
                Address = JObject.FromObject(new {
                    address = "bac giang",
                    phone = "01734935934"
                }).ToString()
            });
            _unit.Save();
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
