using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using MovieCatalog.DAL;
using MovieCatalog.Properties;
using System.Security.Claims;

namespace MovieCatalog.Services
{
    public class IdentityProvider : IIdentityProvider
    {
        private readonly IServiceScopeFactory _scopeFactory;
        public IdentityProvider(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }
        public async Task<ClaimsIdentity> GetIdentity(string username, string password)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var _context = scope.ServiceProvider.GetRequiredService<MovieCatalogDbContext>();
                var user = await _context.Users.Where(x => x.Username == username && x.Password == password).SingleOrDefaultAsync();
                if (user == null)
                {
                    return null;
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Id.ToString()),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, Convert.ToBoolean(user.IsAdmin)?JwtConfigurations.Roles.Admin.ToString():JwtConfigurations.Roles.User.ToString())
                };

                return new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            }
        }
    }
}
