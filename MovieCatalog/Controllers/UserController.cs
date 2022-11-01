using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieCatalog.DAL;
using MovieCatalog.DAL.Models;
using MovieCatalog.DTO;
using MovieCatalog.Properties;

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
        public async Task<IActionResult> GetProfile()
        {
            try
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
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return StatusCode(500, GenericConstants.InternalError);
            }
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> EditProfile(ProfileDTO profileDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(400, ModelState);
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

                return StatusCode(200, GenericConstants.ProfileEdited);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return StatusCode(500, GenericConstants.InternalError);
            }
        }
    }
}
