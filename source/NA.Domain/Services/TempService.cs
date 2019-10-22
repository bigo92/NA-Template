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
using NA.Common.Models;

namespace NA.Domain.Services
{
    public interface ITempService
    {
        (dynamic data,List<ErrorModel> errors, PagingModel paging) Get(Search_TemplateServiceModel model);
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
        public (dynamic data, List<ErrorModel> errors, PagingModel paging) Get(Search_TemplateServiceModel model)
        {
            var errors = new List<ErrorModel>();

            var query = _db.Template.AsQueryable();
            
            if (model.where != null)
            {
                query = query.WhereLoopback(model.whereLoopback);
            }
            query = query.OrderByLoopback(model.orderLoopback);                      
            var result = query.ToPaging(model);
            return (result.data, errors, result.paging);
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
