using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace NA.WebApi.Bases.JWT
{
    public static class JwtOption
    {
        public static readonly string secretKey = "mysupersecret_secretkey!123";
        public static readonly string issuer = "Fiver.Security.Bearer";
        public static readonly string audience = "Fiver.Security.Bearer";
        public static readonly int expiry = 1440;
        public static readonly int rememberme = 10000;
        public static SymmetricSecurityKey SigningKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));
        }
    }
}
