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

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> getFavorites()
        {
            var user = await _context.Users.Where(x => x.Id.ToString() == User.Identity.Name)
                .Include(x => x.FavouriteMovies)
                    .ThenInclude(x => x.Reviews)
                .Include(x => x.FavouriteMovies)
                    .ThenInclude(x => x.Genres)
            .SingleOrDefaultAsync();

            var favorites = new MoviesListDTO
            {
                movies = user.FavouriteMovies.Select(x => new MovieElementDTO
                {
                    id = x.Id,
                    name = x.Name,
                    country = x.Country,
                    poster = x.Poster,
                    year = x.Year,
                    genres = x.Genres.Select(y => new GenreDTO
                    {
                        id = y.Id,
                        name = y.Name
                    }).ToList(),
                    reviews = x.Reviews.Select(y => new ReviewShortDTO
                    {
                        id = y.Id,
                        rating = Convert.ToInt32(y.Rating)
                    }).ToList()
                }).ToList()
            };
            return StatusCode(200, favorites);
        }

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
