using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieCatalog.DAL;
using MovieCatalog.DAL.Models;
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
        public async Task<IActionResult> getProfile()
        {
            var user = await _context.Users.Where(x => x.Id.ToString() == User.Identity.Name).SingleOrDefaultAsync();
            var profile = new ProfileDTO
            {
                id = user.Id,
                name = user.Name,
                nickName = user.Username,
                email = user.Email,
                birthDate = user.BirthDate,
                gender = (GenderDTO)user.Gender,
                avatarLink = user.AvatarLink
            };
            return StatusCode(200, profile);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> editProfile(ProfileDTO profileDTO)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ModelState);
            }

            if (User.Identity.Name != profileDTO.id.ToString())
            {
                return StatusCode(400, "You can only edit your own profile");
            }

            var user = await _context.Users.Where(x => x.Id == profileDTO.id).SingleOrDefaultAsync();
            {
                user.AvatarLink = profileDTO.avatarLink;
                user.Gender = (Gender)profileDTO.gender;
                user.Name = profileDTO.name;
                user.Username = profileDTO.nickName;
                user.Email = profileDTO.email;
                user.BirthDate = profileDTO.birthDate;
            }
            await _context.SaveChangesAsync();

            return StatusCode(200, "Profile successfully edited");
        }
    }
}
