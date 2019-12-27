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
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace NA.Domain.Services
{
    public interface ITempService
    {
        Task<(dynamic data, List<ErrorModel> errors, PagingModel paging)> Get(Search_TemplateServiceModel model);
        Task<(dynamic data, List<ErrorModel> errors)> Add(Add_TemplateServiceModel model);

        Task<(dynamic data, List<ErrorModel> errors)> Edit(Edit_TemplateServiceModel model);

        Task<(dynamic data, List<ErrorModel> errors)> Patch(Guid id, JObject model);

        Task<(dynamic data, List<ErrorModel> errors)> Delete(Guid id);

        Task<(int data, List<ErrorModel> errors)> Count(Count_TemplateServiceModel model);
    }

    public class TempService : ITempService
    {
        private readonly ILogger<TempService> _log;
        private readonly NATemplateContext _db;
        public TempService(ILogger<TempService> log, NATemplateContext db)
        {
            _log = log; _db = db;
        }

        public async Task<(dynamic data, List<ErrorModel> errors, PagingModel paging)> Get(Search_TemplateServiceModel model)
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


        public async Task<(dynamic data, List<ErrorModel> errors)> Add(Add_TemplateServiceModel model)
        {
            var errors = new List<ErrorModel>();
            _db.Template.Add(model);
            var result = await _db.SaveChangesAsync();
            return (result, errors);
        }

        public async Task<(dynamic data, List<ErrorModel> errors)> Edit(Edit_TemplateServiceModel model)
        {
            var errors = new List<ErrorModel>();
            _db.Entry(await _db.Template.FirstOrDefaultAsync(x => x.id == model.id)).CurrentValues.SetValues(model);
            var result = await _db.SaveChangesAsync();
            return (result, errors);
        }

        public async Task<(dynamic data, List<ErrorModel> errors)> Patch(Guid id, JObject model)
        {
            var errors = new List<ErrorModel>();
            _db.Entry(await _db.Template.FirstOrDefaultAsync(x => x.id == id)).CurrentValues.SetValues(model);
            var result = await _db.SaveChangesAsync();
            return (result, errors);
        }

        public async Task<(dynamic data, List<ErrorModel> errors)> Delete(Guid id)
        {
            var errors = new List<ErrorModel>();
            var data = new Template() { id = id };
            _db.Template.Attach(data);
            _db.Template.Remove(data);
            var result = await _db.SaveChangesAsync();
            return (result, errors);
        }

        public async Task<(int data, List<ErrorModel> errors)> Count(Count_TemplateServiceModel model)
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

            var result = await query.CountAsync();
            return (result, errors);
        }
    }
}
