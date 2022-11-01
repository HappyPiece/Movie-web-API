using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using MovieCatalog.DAL;
using MovieCatalog.DAL.Models;
using MovieCatalog.DTO;
using MovieCatalog.Properties;
using MovieCatalog.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace MovieCatalog.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly MovieCatalogDbContext _context;
        private readonly ILogoutService _logoutService;
        private readonly IIdentityProvider _identityProvider;

        public AuthController(MovieCatalogDbContext context, ILogoutService loggedOutService, IIdentityProvider identityProvider)
        {
            _context = context;
            _logoutService = loggedOutService;
            _identityProvider = identityProvider;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDTO userRegisterDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(400, ModelState);
                }

                var flaws = await ValidateRegisterCredentials(userRegisterDTO);
                if (flaws.Count > 0)
                {
                    return StatusCode(400, flaws);
                }

                await _context.Users.AddAsync(new User
                {
                    Name = userRegisterDTO.name,
                    Email = userRegisterDTO.email,
                    Username = userRegisterDTO.userName,
                    Password = userRegisterDTO.password,
                    BirthDate = userRegisterDTO.birthDate.HasValue ? userRegisterDTO.birthDate : GenericConstants.DefaultBirthday,
                    Gender = userRegisterDTO.gender.HasValue ? (Gender)userRegisterDTO.gender : GenericConstants.DefaultGender,
                    AvatarLink = GenericConstants.DefaultAvatar
                });
                await _context.SaveChangesAsync();

                return await Login(new LoginCredentialsDTO
                {
                    username = userRegisterDTO.userName,
                    password = userRegisterDTO.password
                });
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return StatusCode(500, GenericConstants.InternalError);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginCredentialsDTO loginCredentialsDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(400, ModelState);
                }

                var identity = await _identityProvider.GetIdentity(loginCredentialsDTO.username, loginCredentialsDTO.password);
                if (identity == null)
                {
                    return StatusCode(400, GenericConstants.InvalidCredentials);
                }

                var jwt = new JwtSecurityToken(
                    issuer: JwtConfigurations.Issuer,
                    audience: JwtConfigurations.Audience,
                    notBefore: DateTime.UtcNow,
                    claims: identity.Claims,
                    expires: DateTime.UtcNow.AddMinutes(JwtConfigurations.Lifetime),
                    signingCredentials: new SigningCredentials(JwtConfigurations.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
                    );

                return StatusCode(200, new { token = new JwtSecurityTokenHandler().WriteToken(jwt) });
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return StatusCode(500, GenericConstants.InternalError);
            }
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _logoutService.InvalidateToken(Request);
                return StatusCode(200, new
                {
                    token = "",
                    message = "Logged out"
                });
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return StatusCode(500, GenericConstants.InternalError);
            }
        }

        private async Task<List<string>> ValidateRegisterCredentials(UserRegisterDTO userRegisterDTO)
        {
            List<string> flaws = new List<string>();

            if (await _context.Users.AnyAsync(x => x.Username == userRegisterDTO.userName))
            {
                flaws.Add(GenericConstants.UsernameTaken);
            }

            if (await _context.Users.AnyAsync(x => x.Email == userRegisterDTO.email))
            {
                flaws.Add(GenericConstants.EmailTaken);
            }

            if (!Regex.Match(userRegisterDTO.password, GenericConstants.PasswordRegex).Success)
            {
                flaws.Add(GenericConstants.InappropriatePassword);
            }

            return flaws;
        }
    }

}
