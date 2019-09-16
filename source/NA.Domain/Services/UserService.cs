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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NA.Domain.Models.UsserServiceModel;

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
            var errors = new List<ErrorModel>();

            var result = await _unit.db.Select("*").From("aut.Users").WhereRaw($"id = {id}").First();
            return (result, errors);
        }

        public async Task<(JObject data, List<ErrorModel> errors)> GetFiles(long id)
        {
            var errors = new List<ErrorModel>();
            var result = await _unit.db.Select("files, JSON_VALUE(data, '$.fullName') as fullName").From("aut.Users").WhereRaw($"id = '{id}'").First();

            return (result, errors);
        }

        public async Task<(long data, List<ErrorModel> errors)> GetId(string email)
        {
            var errors = new List<ErrorModel>();

            var result = await _unit.db.Select("id").From("aut.Users").WhereRaw($"email = {email}").First();
            return (result != null ? result.Value<long>("id") : 0, errors);
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

        public async Task<(JObject data, List<ErrorModel> errors)> RegisterAccount(RegisterAccountModel model)
        {
            var errors = new List<ErrorModel>();
            var checkUser = await _unit.db.Select("id").From("aut.Users").WhereRaw($"email = '{model.Email}'").First();
            if (checkUser != null)
            {
                errors.Add(new ErrorModel { key = "data.setting.email", value = "staff.email.exist" });
                return (null, errors);
            }
            checkUser = await _unit.db.Select("id").From("aut.Users").WhereRaw($"userName = '{model.UserName}'").First();
            if (checkUser != null)
            {
                errors.Add(new ErrorModel { key = "data.setting.userName", value = "staff.userName.exist" });
                return (null, errors);
            }

            var result = await _userManager.CreateAsync(model, model.password);
            if (result.Succeeded)
            {
                var roleData = await _unit.db.Select("id").From("aut.Roles").WhereRaw($"id = {model.roleType}").First();
                if (roleData == null)
                {
                    roleData = await _unit.db.Select("id").From("aut.Roles").WhereRaw($"normalizedName = 'user'").First();
                }
                if (roleData != null)
                {
                    await _unit.db.Insert(new { userId = model.Id, roleId = roleData.Value<long>("id") }, "INSERTED.userId").From("aut.UserRoles").ExecuteScalar<long>();
                }
            }
            else
            {
                errors.Add(new ErrorModel { key = "data.setting.password", value = result.Errors.ElementAt(0).Description });
            }

            return (null, errors);
        }
    }
}
