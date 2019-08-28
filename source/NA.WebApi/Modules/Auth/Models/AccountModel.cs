using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static tci.repository.Models.UserRepositoryModel;

namespace tci.server.Modules.Aut.Models
{
    public class AccountModel
    {
        public class LoginAccountModel
        {
            [Required(ErrorMessage = "login_form.user_name.required")]
            public string userName { get; set; }

            [Required(ErrorMessage = "login_form.password.required")]
            public string password { get; set; }

            public string TwoFactorCode { get; set; }

            public bool RememberMe { get; set; }
        }

        public class VerificationAuthenticatorModel
        {
            [Required(ErrorMessage = "login_form.user_name.required")]
            public string userName { get; set; }

            [Required(ErrorMessage = "login_form.password.required")]
            public string password { get; set; }

            public string code { get; set; }

            public bool rememberMe { get; set; }
        }

        public class GetPassword_AccountModel
        {
            [Required(ErrorMessage = "recover_password_form.email.required")]
            [EmailAddress(ErrorMessage = "recover_password_form.email.format")]
            public string Email { get; set; }
        }

        public class CheckForgotPassword_AccountModel
        {
            [Required(ErrorMessage = "change_password_form.userName.required")]
            public string UserName { get; set; }

            [Required(ErrorMessage = "change_password_form.token.required")]
            public string Token { get; set; }

            [Required(ErrorMessage = "change_password_form.new_password.required")]
            public string Password { get; set; }

            [Required(ErrorMessage = "change_password_form.confirm_password.required")]
            [Compare("Password", ErrorMessage = "change_password_form.confirm_password.not_match")]
            public string ConfirmPassword { get; set; }
        }
    }
}
