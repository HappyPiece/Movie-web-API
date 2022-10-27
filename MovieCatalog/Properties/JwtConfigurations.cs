using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MovieCatalog.Properties
{
    public class JwtConfigurations
    {
        public const string Issuer = "ApiJwtIssuer";
        public const string Audience = "ApiJwtAudience";
        public const string Key = "Kupil muzhik shlyapu, a ona yemu kak raz";
        public const int Lifetime = 10;
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
        }
    }
}
