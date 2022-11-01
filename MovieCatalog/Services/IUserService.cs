using MovieCatalog.DTO;
using System.Security.Claims;

namespace MovieCatalog.Services
{
    public interface IUserService
    {
        public Task<ClaimsIdentity> GetUserIdentity(string username, string password);
        public Task RegisterUser(UserRegisterDTO userRegisterDTO);

    }
}
