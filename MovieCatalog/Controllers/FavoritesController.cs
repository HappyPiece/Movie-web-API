using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieCatalog.DAL;
using MovieCatalog.DTO;

namespace MovieCatalog.Controllers
{
    [Route("api/favorites")]
    [ApiController]
    public class FavoritesController : ControllerBase
    {
        private readonly MovieCatalogDbContext _context;
        public FavoritesController(MovieCatalogDbContext context)
        {
            _context = context;
        }

        //[HttpGet]

        [HttpPost("{id}/add")]
        [Authorize]
        public async Task<IActionResult> addToFavorites(Guid id)
        {
            var user = await _context.Users.Where(x => x.Id.ToString() == User.Identity.Name).SingleOrDefaultAsync();
            user.FavouriteMovies.Add(await _context.Movies.Where(x => x.Id == id).SingleOrDefaultAsync());
            await _context.SaveChangesAsync();
            return StatusCode(200, "Movie added to favorites");
        }

        [HttpDelete("{id}/delete")]
        [Authorize]
        public async Task<IActionResult> deleteFromFavorites(Guid id)
        {
            var user = await _context.Users.Where(x => x.Id.ToString() == User.Identity.Name).Include(x => x.FavouriteMovies).SingleOrDefaultAsync();
            if (user.FavouriteMovies.Remove(user.FavouriteMovies.Where(x => x.Id == id).First()))
            {
                await _context.SaveChangesAsync();
                return StatusCode(200, "Movie removed from favorites");
            }
            return StatusCode(500, "Unable to return movie from favorites");
        }
    }
}
