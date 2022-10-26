using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.EntityFrameworkCore;
using MovieCatalog.DAL;
using MovieCatalog.DTO;
using System.Runtime.CompilerServices;

namespace MovieCatalog.Controllers
{
    [Route("api/movies")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly MovieCatalogDbContext _context;
        private readonly int MaxPageSize;
        public MoviesController(MovieCatalogDbContext context)
        {
            _context = context;
            MaxPageSize = 6;
        }

        [HttpGet("{page}")]
        public IActionResult getMoviesPage(int page = 1)
        {
            if (page < 1)
            {
                page = 1;
            }

            var movies = _context.Movies.Include(x => x.Genres).Include(x => x.Reviews).Select(x => new MovieElementDTO
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
                }).ToList(),
            }).ToList();

            List<MovieElementDTO> moviesPage = new List<MovieElementDTO>();
            for (int i = (page - 1) * MaxPageSize; i <= (page - 1) * MaxPageSize + 5; i++)
            {
                if (movies.Count > i)
                {
                    moviesPage.Add(movies[i]);
                }
                else
                {
                    i = (page - 1) * MaxPageSize + 6;
                }
            }

            var pageInfo = new PageInfoDTO
            {
                pageSize = moviesPage.Count,
                pageCount = (movies.Count == 0) ? (1) : (movies.Count + 5) / MaxPageSize,
                currentPage = page
            };

            return StatusCode(200, new MoviesPagedListDTO
            {
                movies = moviesPage,
                pageInfo = pageInfo
            });
        }

        [HttpGet("details/{id}")]
        public IActionResult getMovie(Guid id)
        {
            var movie = _context.Movies.Where(x => x.Id == id).Include(x => x.Genres).Include(x => x.Reviews).ThenInclude(x => x.User).Select(x => new MovieDetailsDTO
            {
                id = x.Id,
                ageLimit = x.AgeLimit,
                budget = x.Budget,
                country = x.Country,
                description = x.Description,
                director = x.Director,
                fees = x.Fees,
                genres = x.Genres.Select(y => new GenreDTO
                {
                    id = y.Id,
                    name = y.Name
                }).ToList(),
                name = x.Name,
                poster = x.Poster,
                reviews = x.Reviews.Select(y => new ReviewDTO
                {
                    id = y.Id,
                    reviewText = y.ReviewText,
                    isAnonymous = Convert.ToBoolean(y.IsAnonymous),
                    rating = Convert.ToInt32(y.Rating),
                    createDateTime = Convert.ToDateTime(y.CreationDateTime),
                    author = new UserShortDTO
                    {
                        userId = y.User.Id,
                        avatar = y.User.AvatarLink,
                        nickName = y.User.Username
                    }
                }).ToList(),
                tagline = x.Tagline,
                time = x.Time,
                year = x.Year
            }).SingleOrDefault();

            return StatusCode(200, movie);
        }
    }
}
