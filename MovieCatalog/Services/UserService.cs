using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using MovieCatalog.DAL;
using MovieCatalog.DAL.Models;
using MovieCatalog.DTO;
using MovieCatalog.Properties;
using System.Security.Claims;

namespace MovieCatalog.Services
{
    public class UserService : IUserService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        public UserService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }
        public async Task<ClaimsIdentity> GetUserIdentity(string username, string password)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var _context = scope.ServiceProvider.GetRequiredService<MovieCatalogDbContext>();
                var user = await _context.Users.Where(x => x.Username == username).SingleOrDefaultAsync();
                if (user == null)
                {
                    return null;
                }

                if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
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

        public async Task RegisterUser(UserRegisterDTO userRegisterDTO)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var _context = scope.ServiceProvider.GetRequiredService<MovieCatalogDbContext>();
                await _context.Users.AddAsync(new User
                {
                    Name = userRegisterDTO.name,
                    Email = userRegisterDTO.email,
                    Username = userRegisterDTO.userName,
                    Password = BCrypt.Net.BCrypt.HashPassword(userRegisterDTO.password),
                    BirthDate = userRegisterDTO.birthDate.HasValue ? userRegisterDTO.birthDate : GenericConstants.DefaultBirthday,
                    Gender = userRegisterDTO.gender.HasValue ? (Gender)userRegisterDTO.gender : GenericConstants.DefaultGender,
                    AvatarLink = GenericConstants.DefaultAvatar
                });
                await _context.SaveChangesAsync();
            }
        }
    }
}
