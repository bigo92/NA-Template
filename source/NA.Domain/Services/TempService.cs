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
using NA.Domain.Models;
using NA.Common.Extentions;
using Na.DataAcess.Bases;
using System.Threading.Tasks;

namespace NA.Domain.Services
{
    public interface ITempService : ICRUDService<Template>
    {
        void Add(Add_TemplateServiceModel model);
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

        public Task<JArray> Test_Cache()
        {
            return _cache.GetOrAdd("ALL_Template", () => { return  GetAllTemplate(); },TimeSpan.FromDays(30));
        }

        private async Task<JArray> GetAllTemplate()
        {
            return await _unit.Repository<Template>().Select().Excute();
        }

        public string FindOne()
        {
            return _sv2.FindOne();
        }

        public void Add(Add_TemplateServiceModel model)
        {
            _unit.Repository<Template>().Add(model);
            _unit.SaveChange();
        }

        public void AddTransaction(Add_TemplateServiceModel model)
        {
            using (var tran = TransactionScopeExtention.BeginTransactionScope())
            {
                _unit.Repository<Template>().Add(model);
                _unit.SaveChange();
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

        public void Add(Template model)
        {
            throw new NotImplementedException();
        }
    }
}
