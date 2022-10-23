using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MovieCatalog.DAL;
using MovieCatalog.DAL.Models;
using MovieCatalog.DTO;
using MovieCatalog.Properties;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MovieCatalog.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly MovieCatalogDbContext _context;

        public AuthController(MovieCatalogDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> register(UserRegisterDTO userRegisterDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _context.Users.AddAsync(new User
            {
                Name = userRegisterDTO.name,
                Email = userRegisterDTO.email,
                Username = userRegisterDTO.userName,
                Password = userRegisterDTO.password,
                BirthDate = userRegisterDTO.birthDate,
                Gender = (Gender)userRegisterDTO.gender
            });
            await _context.SaveChangesAsync();

            return login(new LoginCredentialsDTO
            {
                username = userRegisterDTO.userName,
                password = userRegisterDTO.password
            });
        }

        [HttpPost("login")]
        public IActionResult login(LoginCredentialsDTO loginCredentialsDTO)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ModelState);
            }
            var identity = GetIdentity(loginCredentialsDTO.username, loginCredentialsDTO.password);
            if (identity == null)
            {
                return StatusCode(400, "Invalid username or password");
            }
            var jwt = new JwtSecurityToken(
                issuer: JwtConfigurations.Issuer,
                audience: JwtConfigurations.Audience,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(JwtConfigurations.Lifetime),
                signingCredentials: new SigningCredentials(JwtConfigurations.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
                );
            return StatusCode(200, new { token = new JwtSecurityTokenHandler().WriteToken(jwt) });
        }

        private ClaimsIdentity GetIdentity(string username, string password)
        {
            var user = _context.Users.Where(x => x.Username == username && x.Password == password).FirstOrDefault();
            if (user == null)
            {
                return null;
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Id.ToString()),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, Convert.ToBoolean(user.IsAdmin)?"Admin":"User")
            };

            return new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
        }
    }

}
