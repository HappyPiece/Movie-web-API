using System.Security.Claims;

namespace MovieCatalog.Services
{
    public interface IIdentityProvider
    {
        public Task<ClaimsIdentity> GetIdentity(string username, string password);
    }
}
