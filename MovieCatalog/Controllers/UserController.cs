using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieCatalog.DAL;
using MovieCatalog.DTO;

namespace MovieCatalog.Controllers
{
    [Route("api/account/profile")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly MovieCatalogDbContext _context;
        public UserController(MovieCatalogDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize]
        public IActionResult getProfile()
        {
            var user = _context.Users.Where(x => x.Id.ToString() == User.Identity.Name).SingleOrDefault();
            var profile = new ProfileDTO
            {
                id = user.Id,
                name = user.Name,
                nickName = user.Username,
                email = user.Email,
                birthDate = user.BirthDate,
                gender = user.Gender,
                avatarLink = user.AvatarLink
            };
            return StatusCode(200, profile);
        }
    }
}
