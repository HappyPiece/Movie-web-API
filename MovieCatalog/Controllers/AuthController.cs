using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieCatalog.DAL;
using MovieCatalog.DAL.Models;

namespace MovieCatalog.Controllers
{
    [Route("api/account/register")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly MovieCatalogDbContext _context;

        public AuthController(MovieCatalogDbContext context)
        {
            _context = context;
        }

        [HttpPost]
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

            return StatusCode(500, new NotImplementedException());
        }
    }
}
