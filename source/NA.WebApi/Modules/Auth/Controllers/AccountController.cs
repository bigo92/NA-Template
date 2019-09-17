using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using static tci.server.Modules.Aut.Models.AccountModel;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Configuration;
using System.Text;
using tci.server.Modules.Aut.Models;
using System.Text.Encodings.Web;
using NA.WebApi.Bases;
using NA.DataAccess.Models;
using NA.DataAccess.Bases;
using Na.Common.Enums;
using NA.Domain.Services;
using NA.Common.Extentions;
using NA.WebApi.Bases.JWT;

namespace tci.server.Modules.Aut.Controllers
{
    [Route("/api/[controller]/[action]")]
    public class AccountController : ApiController
    {
        private readonly ILogger _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUserService _userService;
        private readonly IConfiguration _config;

        private readonly UrlEncoder _urlEncoder;
        private const string AuthenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";
        private const string RecoveryCodesKey = nameof(RecoveryCodesKey);

        public AccountController(
            ILogger<AccountController> logger,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager,
            IUserService userService,
            IConfiguration configuration,
            UrlEncoder urlEncoder
            )
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _config = configuration;
            _urlEncoder = urlEncoder;
            _userService = userService;
        }

        /// <summary>
        /// Post data login userName step 1
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("/api/[controller]/Login")]
        public async Task<object> LoginData([FromBody]LoginAccountModel model)
        {
            if (ModelState.IsValid)
            {
                var queryWhere = new JObject {{ "and", new JArray {
                    new JObject {{ "userName", model.userName.Trim() }}
                }}};
                var userData = await _userManager.FindByEmailAsync(model.userName);
                //var userData = await unit.db.Select("id,passwordHash,twoFactorEnabled,securityStamp,phoneNumber,lockoutEnd,accessFailedCount,userName,email,JSON_VALUE(data,'$.qrCode') as qrCode,JSON_VALUE(data_db,'$.status') as status").Where(queryWhere).From("aut.Users").First();
                if (userData == null) //check userName exist
                {
                    ModelState.AddModelError("", "login_form.not_match");
                    return await BindData();
                }

                //check userName block                
                if (userData.data_db.status != (int)Enums.Status_db.Nomal)
                {
                    ModelState.AddModelError("", "login_form.locked_account");
                    return await BindData();
                }
                //check userName time block  //Todo datetimeoffset ???      
                var timeLock = userData.LockoutEnd;
                if (timeLock.HasValue && timeLock.Value > DateTime.UtcNow)
                {
                    ModelState.AddModelError("", $"login_form.locked_account");
                    return await BindData(new
                    {
                        blockTime = (timeLock.Value - DateTime.UtcNow).TotalSeconds
                    });
                }

                var user = new ApplicationUser
                {
                    PhoneNumber = userData.PhoneNumber,
                    UserName = userData.UserName,
                    Email = userData.Email,
                    PasswordHash = userData.PasswordHash,
                    SecurityStamp = userData.SecurityStamp,
                };
                var loginResult = await _signInManager.CheckPasswordSignInAsync(user, model.password, false);

                if (!loginResult.Succeeded)
                {
                    using (var tran = TransactionScopeExtention.BeginTransactionScope())
                    {
                        int maxAccessFailedCount = 5;
                        //login failed set SetLockoutEndDate
                        var accessFailedCount = userData.AccessFailedCount;
                        accessFailedCount++;
                        if (accessFailedCount >= maxAccessFailedCount)
                        {
                            await _userService.UpadteLockoutEnd(userData, DateTime.UtcNow.AddMinutes(1 + (accessFailedCount - maxAccessFailedCount) * 2));
                        }
                        await _userService.AccessFailedCount(userData, accessFailedCount);
                        ModelState.AddModelError("", "login_form.not_match");

                        tran.Complete();
                    }

                    return await BindData();
                }

                using (var tran = TransactionScopeExtention.BeginTransactionScope())
                {
                    //login success reset AccessFailed
                    await _userService.AccessFailedCount(userData, 0);

                    // check twoFactorEnabled verify
                    if (userData.TwoFactorEnabled)
                    {
                        //check has qrCode
                        if (!userData.data.qrCode.HasValue())
                        {
                            //Generate or reset qrcode                        
                            var result = new EnableAuthenticatorViewModel();
                            await LoadSharedKeyAndQrCodeUriAsync(user.UserName, result, true);
                            return await BindData(new
                            {
                                twoFactorEnabled = userData.TwoFactorEnabled,
                                authenticatorUri = result.AuthenticatorUri
                            });
                        }

                        return await BindData(new
                        {
                            twoFactorEnabled = userData.TwoFactorEnabled,
                            authenticatorUri = default(string)
                        });
                    }

                    tran.Complete();
                }

                // Generate token login
                user.Id = userData.Id;
                var roles = new JArray(); // await unit.db.Select("roleId").From("aut.UserRoles").WhereRaw($"userId = {user.Id}").Excute();
                var token = await BuildToken(user, model.RememberMe, roles);
                return await BindData(new { access_token = $"Bearer {token.Value}", expiry = token.ValidTo });
            }
            return await BindData();
        }

        /// <summary>
        /// Get info data user login by Authorize
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<object> Info()
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.FindOne(GetUserId());
                return await BindData(result.data, result.errors);
            }
            return await BindData();
        }

        /// <summary>
        /// get lst GetClaims user login
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<object> GetClaims()
        {
            if (ModelState.IsValid)
            {
                var result = User.Claims;
                return await BindData(result);
            }
            return await BindData();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<object> ResetQRCode(string userName)
        {
            if (ModelState.IsValid)
            {
                var userId = await _userService.GetId(userName);
                var userIdentity = await _userManager.FindByIdAsync(userId.ToString());
                if (userIdentity != null)
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(userIdentity);
                    token = System.Web.HttpUtility.UrlEncode(token.Replace("+", " "));
                    //this.hf.Enqueue<BackgroundJobs>(x => x.ResetQRCode(userIdentity, token));
                    return await BindData(true);
                }
                ModelState.AddModelError("error", "userName.not_exist");
            }
            return await BindData();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<object> CheckResetQRCode(string userName, string token)
        {
            if (ModelState.IsValid)
            {
                token = System.Web.HttpUtility.UrlDecode(token).Replace(" ", "+");
                var userIdentity = await _userManager.FindByNameAsync(userName);
                if (userIdentity != null)
                {
                    var result = await _userManager.ConfirmEmailAsync(userIdentity, token);
                    if (result.Succeeded)
                    {
                        await _userService.UpdateQrCode(userIdentity, "");
                        await _userService.AccessFailedCount(userIdentity, 0);
                        return Redirect($"{GetDomain()}/auth/login");
                    }
                }
            }
            return Redirect($"{GetDomain()}/404");
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<object> CheckForgotPassword([FromBody] CheckForgotPassword_AccountModel model)
        {
            if (ModelState.IsValid)
            {
                model.Token = System.Web.HttpUtility.UrlDecode(model.Token).Replace(" ", "+");
                var userIdentity = await _userManager.FindByNameAsync(model.UserName);
                if (userIdentity != null)
                {
                    var result = await _userManager.ResetPasswordAsync(userIdentity, model.Token, model.Password);
                    if (result.Succeeded)
                    {
                        return await BindData(true);
                    }
                }
                ModelState.AddModelError("error", "change_password_form.dialog.failed.content");
            }
            return await BindData();
        }

        [HttpGet]
        public async Task<object> Logout()
        {
            await _signInManager.SignOutAsync();
            return await BindData();
        }

        [HttpGet]
        [AllowAnonymous]
        /// <summary>
        /// Get files
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<object> GetFiles(long id)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.GetFiles(id);
                return await BindData(result.data, result.errors);
            }
            return await BindData();
        }

        [HttpGet]
        [Authorize]
        public async Task<object> EnableAuthenticator()
        {

            var currentUserName = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await _userManager.FindByNameAsync(currentUserName);
            //var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await _userManager.SetTwoFactorEnabledAsync(user, true);

            var model = new EnableAuthenticatorViewModel();
            await LoadSharedKeyAndQrCodeUriAsync(user.UserName, model);

            return await BindData(model);
        }

        /***
         * Disable 2FA
         *       
        */
        [HttpGet]
        [Authorize]
        public async Task<object> Disable2fa()
        {
            var currentUserName = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await _userManager.FindByNameAsync(currentUserName);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var disable2faResult = await _userManager.SetTwoFactorEnabledAsync(user, false);
            if (!disable2faResult.Succeeded)
            {
                throw new ApplicationException($"Unexpected error occured disabling 2FA for user with ID '{user.Id}'.");
            }
            return await BindData(disable2faResult);
        }

        [HttpGet]
        [Authorize]
        public async Task<object> TwoFactorAuthentication()
        {
            var user = await _userManager.FindByIdAsync(_userManager.GetUserId(User));
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var model = new TwoFactorAuthenticationViewModel
            {
                HasAuthenticator = await _userManager.GetAuthenticatorKeyAsync(user) != null,
                Is2faEnabled = user.TwoFactorEnabled,
                RecoveryCodesLeft = await _userManager.CountRecoveryCodesAsync(user),
            };
            return await BindData(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<object> VerificationAuthenticator([FromBody] VerificationAuthenticatorModel model)
        {
            if (ModelState.IsValid)
            {
                var userData = await _userManager.FindByEmailAsync(model.userName);
                if (userData == null)
                { 
                    ModelState.AddModelError("", "login_form.not_match");
                    return await BindData();
                }
                //check userName block                
                if (userData.data_db.status != (int)Enums.Status_db.Nomal)
                {
                    ModelState.AddModelError("", "login_form.locked_account");
                    return await BindData();
                }
                //check userName time block        
                var timeLock = userData.data.lockoutEnd;
                if (timeLock.HasValue && timeLock.Value > DateTime.UtcNow)
                {
                    ModelState.AddModelError("", $"login_form.locked_account");
                    return await BindData(new
                    {
                        blockTime = (timeLock.Value - DateTime.UtcNow).TotalSeconds
                    });
                }

                var userLogin = new ApplicationUser
                {
                    PhoneNumber = userData.PhoneNumber,
                    UserName = userData.UserName,
                    Email = userData.Email,
                    PasswordHash = userData.PasswordHash,
                    SecurityStamp = userData.SecurityStamp,
                };
                var loginResult = await _signInManager.CheckPasswordSignInAsync(userLogin, model.password, false);
                if (!loginResult.Succeeded)
                {
                    ModelState.AddModelError("", "login_form.not_match");
                    return await BindData();
                }

                var user = await _userManager.FindByNameAsync(model.userName);

                var modelQCode = new EnableAuthenticatorViewModel() { Code = model.code };

                await LoadSharedKeyAndQrCodeUriAsync(userLogin.UserName, modelQCode, false);

                // Strip spaces and hypens
                var verificationCode = modelQCode.Code.Replace(" ", string.Empty).Replace("-", string.Empty);
                var is2faTokenValid = await _userManager.VerifyTwoFactorTokenAsync(user, _userManager.Options.Tokens.AuthenticatorTokenProvider, verificationCode);
                if (is2faTokenValid)
                {
                    //done      
                    userLogin.Id = userData.Id;
                    //get role
                    var roles = new JArray(); //await unit.db.Select("roleId").From("aut.UserRoles").WhereRaw($"userId = {userLogin.Id}").Excute();
                    var token = await BuildToken(userLogin, true, roles);
                    if (!userData.data.qrCode.HasValue())
                    {
                        //save db
                        await _userService.UpdateQrCode(userData, modelQCode.AuthenticatorUri);
                    }
                    //TODO: AccessFailedCount for Token
                    await _userService.AccessFailedCount(userData, 0);
                    return await BindData(new { access_token = $"Bearer {token.Value}", expiry = token.ValidTo });
                }

                //login failed set SetLockoutEndDate
                int maxAccessFailedCount = 5;
                var accessFailedCount = userData.data.accessFailedCount.HasValue ? userData.data.accessFailedCount.Value : 0;
                accessFailedCount++;
                if (accessFailedCount >= maxAccessFailedCount)
                {
                    //TODO: UpadteLockoutEnd for Token
                    await _userService.UpadteLockoutEnd(userData, DateTime.UtcNow.AddMinutes(1 + (accessFailedCount - maxAccessFailedCount) * 2));
                }

                //TODO: AccessFailedCount for Token
                await _userService.AccessFailedCount(userData, accessFailedCount);
                ModelState.AddModelError("error", "login_form.qrCode.not_match");
            }
            return await BindData();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<object> Register([FromBody] RegisterAccountModel model)
        {
            if (ModelState.IsValid)
            {
               var result = await _userService.RegisterAccount(model);
                return await BindData(result.data, result.errors);
            }
            return await BindData();
        }

        #region Helpers


        private string FormatKey(string unformattedKey)
        {
            var result = new StringBuilder();
            int currentPosition = 0;
            while (currentPosition + 4 < unformattedKey.Length)
            {
                result.Append(unformattedKey.Substring(currentPosition, 4)).Append(" ");
                currentPosition += 4;
            }
            if (currentPosition < unformattedKey.Length)
            {
                result.Append(unformattedKey.Substring(currentPosition));
            }

            return result.ToString().ToLowerInvariant();
        }

        private string GenerateQrCodeUri(string userName, string email, string unformattedKey)
        {
            return string.Format(AuthenticatorUriFormat, _urlEncoder.Encode(userName), _urlEncoder.Encode(email), unformattedKey);
        }

        private async Task LoadSharedKeyAndQrCodeUriAsync(string userName, EnableAuthenticatorViewModel model, bool reset = false)
        {
            var user = await _userManager.FindByNameAsync(userName);
            var unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
            if (reset || string.IsNullOrEmpty(unformattedKey))
            {
                await _userManager.ResetAuthenticatorKeyAsync(user);
                unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
                if (string.IsNullOrEmpty(unformattedKey))
                {
                    throw new Exception("unformattedKey is empty");
                }
            }

            model.SharedKey = FormatKey(unformattedKey);
            model.AuthenticatorUri = GenerateQrCodeUri(userName, user.Email, unformattedKey);
        }

        private async Task<JwtToken> BuildToken(ApplicationUser user, bool remember, JArray roles = null)
        {
            //var roles = await _userManager.GetRolesAsync(user);
            if (roles == null) roles = new JArray();
            //if remember add time one year
            var timeExpiry = remember ? JwtOption.expiry + 525600 : JwtOption.expiry;
            //build token
            var tokenBuild = new JwtTokenBuilder()
                                .AddSecurityKey(JwtOption.SigningKey())
                                .AddSubject(user.UserName)
                                .AddIssuer(JwtOption.issuer)
                                .AddAudience(JwtOption.audience)
                                .AddClaim("MembershipId", user.Id.ToString())
                                .AddClaim("Roles", roles.Select(x => x.Value<long>("roleId")).JsonToString())
                                .AddExpiry(timeExpiry);
            return tokenBuild.Build();
        }
        #endregion
    }
}