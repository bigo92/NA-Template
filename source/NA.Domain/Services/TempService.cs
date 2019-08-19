using System;
using NA.Domain.Interfaces;
using NA.Domain.Bases;
using Microsoft.Extensions.Logging;
using NA.DataAccess.Bases;
using NA.DataAccess.Models;
using Newtonsoft.Json.Linq;
using System.Transactions;
using NA.Domain.Cache;
using System.Linq;
using System.Collections.Generic;

namespace NA.Domain.Services
{
    public interface ITempService : ICRUDService<Template>
    {

    }

    public class TempService : ITempService, IDisposable
    {
        private readonly IUnitOfWork _unit;
        private readonly ILogger<TempService> _log;
        private readonly ITempService2 _sv2;
        private ICacheService _cache;
        public TempService(IUnitOfWork unit, ILogger<TempService> log, ITempService2 sv2, ICacheService cache)
        {
            _unit = unit; _log = log; _sv2 = sv2;_cache = cache;
        }
        public void Get(Template model)
        {
            throw new NotImplementedException();
            
        }


        public List<Template> Test_Cache()
        {
            return _cache.GetOrAdd("ALL_Template", () => { return GetAllTemplate(); },TimeSpan.FromDays(30));
        }

        private List<Template> GetAllTemplate()
        {
            return _unit.Repository<Template>().GetAll().ToList();
        }
        public string FindOne()
        {
            return _sv2.FindOne();
        }

        public void Add(Template model)
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

        public void Delete(Template model)
        {
        }

        public void Edit(Template model)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _log.LogError("Dispose Service");
        }
    }
}
