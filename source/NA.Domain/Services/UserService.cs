using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Na.DataAcess.Bases;
using NA.Common.Extentions;
using NA.Common.Models;
using NA.DataAccess.Bases;
using NA.DataAccess.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NA.Domain.Services
{
    public interface IUserService
    {
        Task AccessFailedCount(ApplicationUser user, int value);

        Task UpadteLockoutEnd(ApplicationUser user, DateTime value);

        Task UpdateQrCode(ApplicationUser user, string value);

        Task<(JObject data, List<ErrorModel> errors)> FindOne(long id);

        Task<(long data, List<ErrorModel> errors)> GetId(string email);

        Task<(JObject data, List<ErrorModel> errors)> GetFiles(long id);
    }

    public class UserService : IUserService
    {
        private readonly UnitOfWork<DbContext> _unit;
        private readonly ILogger<UserService> _log;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(
            ILogger<UserService> log,
            UnitOfWork<DbContext> unit,
            UserManager<ApplicationUser> userManager)
        {
            _log = log;
            _unit = unit;
            _userManager = userManager;
        }

        public async Task AccessFailedCount(ApplicationUser user, int value)
        {
            await _unit.db.Set($"accessFailedCount = {value}").From("aut.Users").WhereRaw($"id = {user.Id}").ExecuteNonQuery();
        }

        public async Task<(JObject data, List<ErrorModel> errors)> FindOne(long id)
        {
            var lstError = new List<ErrorModel>();

            var result = await _unit.db.Select("*").From("aut.Users").WhereRaw($"id = {id}").First();
            return (result, lstError);
        }

        public async Task<(JObject data, List<ErrorModel> errors)> GetFiles(long id)
        {
            var lstError = new List<ErrorModel>();
            var result = await _unit.db.Select("files, JSON_VALUE(data, '$.fullName') as fullName").From("aut.Users").WhereRaw($"id = '{id}'").First();

            return (result, lstError);
        }

        public async Task<(long data, List<ErrorModel> errors)> GetId(string email)
        {
            var lstError = new List<ErrorModel>();

            var result = await _unit.db.Select("id").From("aut.Users").WhereRaw($"email = {email}").First();
            return (result != null ? result.Value<long>("id") : 0, lstError);
        }

        public async Task UpadteLockoutEnd(ApplicationUser user, DateTime value)
        {
            await _unit.db.Set($"lockoutEnd = '{value.ToISOString()}'")
                .From("aut.Users").WhereRaw($"id = {user.Id}").ExecuteNonQuery();
        }

        public async Task UpdateQrCode(ApplicationUser user, string value)
        {
            await _unit.db.Set($"data = JSON_MODIFY(data, '$.qrCode', N'{value}')").From("aut.Users").WhereRaw($"id = {user.Id}").ExecuteNonQuery();
        }
    }
}
