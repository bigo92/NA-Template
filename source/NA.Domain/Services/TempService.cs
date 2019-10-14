using System;
using NA.Domain.Interfaces;
using NA.Domain.Bases;
using Microsoft.Extensions.Logging;
using NA.DataAccess.Bases;
using NA.DataAccess.Contexts;
using Newtonsoft.Json.Linq;
using System.Transactions;
using NA.Domain.Cache;
using System.Linq;
using System.Collections.Generic;
using NA.Domain.Models;

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
        private readonly NATemplateContext _db;
        public TempService(IUnitOfWork unit, ILogger<TempService> log, ITempService2 sv2, ICacheService cache, NATemplateContext db)
        {
            _unit = unit; _log = log; _sv2 = sv2; _cache = cache; _db = db;
        }
        public List<Template> Get()
        {
            var query = _db.Template.AsQueryable();
            query = query.Where(x => DbFunction.JsonValue((string)(object)x.data, "$.name") == "68a77924-0191-4db3-87b7-7da2299a49d6");
            query = query.WhereLoopback("files", "68a77924-0191-4db3-87b7-7da2299a49d6");
            query = query.OrderBy("id", true);
            return query.ToList();
        }


        public List<Template> Test_Cache()
        {
            return _cache.GetOrAdd("ALL_Template", () => { return GetAllTemplate(); }, TimeSpan.FromDays(30));
        }

        private List<Template> GetAllTemplate()
        {
            return _unit.Repository<Template>().GetAll().ToList();
        }
        public string FindOne()
        {
            return _sv2.FindOne();
        }

        public void Add(Add_TemplateServiceModel model)
        {
            _db.Template.Add(new Template
            {
                data = new Template.DataJson { name = Guid.NewGuid().ToString()},
                files = new List<Common.Models.FileModel>(),
                language = default,
                data_db = new Common.Models.DataDb(),
                tag = default
            });
            _db.SaveChanges();
            //var data = model as Template;
            //_unit.Repository<Template>().Insert(data);
            //_unit.Save();
        }

        public void AddTransaction(Add_TemplateServiceModel model)
        {
            using (var tran = _unit.BeginTransaction())
            {
                _unit.Repository<Template>().Insert(model);
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

        public void Add(Template model)
        {
            throw new NotImplementedException();
        }
    }
}
