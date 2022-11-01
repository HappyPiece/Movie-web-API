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
        public async Task<IActionResult> GetFavorites()
        {
            try
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
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return StatusCode(500, GenericConstants.InternalError);
            }
        }

        [HttpPost("{id}/add")]
        [Authorize]
        public async Task<IActionResult> AddToFavorites(Guid id)
        {
            try
            {
                var movie = await _context.Movies.Where(x => x.Id == id).SingleOrDefaultAsync();
                if (movie == null)
                {
                    return StatusCode(404, GenericConstants.NoSuchMovie);
                }

                var user = await _context.Users.Where(x => x.Id.ToString() == User.Identity.Name).Include(x => x.FavouriteMovies).SingleOrDefaultAsync();
                if (user.FavouriteMovies.Contains(movie))
                {
                    return StatusCode(400, GenericConstants.MovieAlreadyInFavorites);
                }

                user.FavouriteMovies.Add(movie);
                await _context.SaveChangesAsync();
                return StatusCode(200, GenericConstants.MovieAddedToFavorites);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return StatusCode(500, GenericConstants.InternalError);
            }
        }

        [HttpDelete("{id}/delete")]
        [Authorize]
        public async Task<IActionResult> DeleteFromFavorites(Guid id)
        {
            try
            {
                var movie = await _context.Movies.Where(x => x.Id == id).SingleOrDefaultAsync();
                if (movie == null)
                {
                    return StatusCode(404, GenericConstants.NoSuchMovie);
                }

                var user = await _context.Users.Where(x => x.Id.ToString() == User.Identity.Name).Include(x => x.FavouriteMovies).SingleOrDefaultAsync();
                if (!user.FavouriteMovies.Contains(movie))
                {
                    return StatusCode(400, GenericConstants.UnableToRemoveFromFavorites);
                }

                user.FavouriteMovies.Remove(movie);
                await _context.SaveChangesAsync();
                return StatusCode(200, GenericConstants.MovieRemovedFromFavorites);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return StatusCode(500, GenericConstants.InternalError);
            }
        }
    }
}
