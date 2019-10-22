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
using tci.common.Enums;
using NA.Common.Extentions;

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
        private ICacheService _cache;
        private readonly NATemplateContext _db;
        public TempService(IUnitOfWork unit, ILogger<TempService> log, NATemplateContext db)
        {
            _unit = unit; _log = log; _db = db;
        }

        public (dynamic data, List<ErrorModel> errors, PagingModel paging) Get(Search_TemplateServiceModel model)
        {
            var errors = new List<ErrorModel>();

            var query = _db.Template.AsQueryable();

            query = query.Where(x =>
                    (int)(object)DbFunction.JsonValue((string)(object)x.data_db, "$.status") == 0);
            if (model.where != null)
            {
                query = query.WhereLoopback(model.whereLoopback);

                if (!model.whereLoopback.HaveWhereStatusDb()) //default where statusdb is active
                {
                    var statusActive = (int)Enums.Status_db.Nomal;
                    query = query.Where(x =>
                    (int)(object)DbFunction.JsonValue((string)(object)x.data_db, "$.status") == statusActive);
                }
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
