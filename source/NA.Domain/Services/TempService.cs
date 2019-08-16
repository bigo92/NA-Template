using System;
using NA.Domain.Interfaces;
using NA.Domain.Bases;
using Microsoft.Extensions.Logging;
using NA.DataAccess.Bases;
using NA.DataAccess.Models;
using Newtonsoft.Json.Linq;
using System.Transactions;

namespace NA.Domain.Services
{
    public interface ITempService : ICRUDService
    {

    }

    public class TempService : ITempService, IDisposable
    {
        private readonly IUnitOfWork _unit;
        private readonly ILogger<TempService> _log;
        private readonly ITempService2 _sv2;
        public TempService(IUnitOfWork unit, ILogger<TempService> log, ITempService2 sv2)
        {
            _unit = unit; _log = log; _sv2 = sv2;
        }
        public void Get<T>(T model)
        {
            throw new NotImplementedException();
        }

        public string FindOne()
        {
            return _sv2.FindOne();
        }

        public void Add<T>(T model)
        {
            using (var tran = _unit.BeginTransaction())
            {
                _unit.Repository<Template>().Insert(new Template
                {
                    Id = Guid.NewGuid(),
                    Info = new Template.InfoJson
                    {
                        name = "Nguyen Van A",
                        age = 12
                    },
                    Address = new Template.AddressJson
                    {
                        address1 = "bac giang",
                        address2 = "01734935934"
                    }
                });
                _unit.Save();
                tran.Complete();
            }
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
