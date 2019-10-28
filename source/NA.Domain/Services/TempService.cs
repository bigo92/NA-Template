using System;
using Microsoft.Extensions.Logging;
using NA.DataAccess.Bases;
using NA.DataAccess.Contexts;
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
        private readonly ILogger<TempService> _log;
        private readonly NATemplateContext _db;
        public TempService(ILogger<TempService> log, NATemplateContext db)
        {
            _log = log; _db = db;
        }

        public (dynamic data, List<ErrorModel> errors, PagingModel paging) Get(Search_TemplateServiceModel model)
        {
            var errors = new List<ErrorModel>();
            var statusActive = (int)Enums.Status_db.Nomal;

            var query = _db.Template.AsQueryable();
            
            if (model.where != null)
            {
                query = query.WhereLoopback(model.whereLoopback);

                if (!model.whereLoopback.HaveWhereStatusDb()) //default where statusdb is active
                {
                    query = query.Where(x =>
                   (int)DbFunction.JsonValue(x.data_db, "$.status") == statusActive);
                }
            }
            else
            {
                query = query.Where(x =>
                   (int)DbFunction.JsonValue(x.data_db, "$.status") == statusActive);
            }

            query = query.OrderByLoopback(model.orderLoopback);                      
            var result = query.ToPaging(model);
            return (result.data, errors, result.paging);
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
