using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.EntityFrameworkCore;
using MovieCatalog.DAL;
using MovieCatalog.DTO;
using MovieCatalog.Properties;
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
        public IActionResult GetMoviesPage(int page = 1)
        {
            try
            {
                if (page < 1)
                {
                    return StatusCode(404, GenericConstants.NoSuchPage);
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
                for (int i = (page - 1) * MaxPageSize; i < Math.Min(page * MaxPageSize, movies.Count); i++)
                {
                    moviesPage.Add(movies[i]);
                }

                var pageInfo = new PageInfoDTO
                {
                    pageSize = moviesPage.Count,
                    pageCount = (movies.Count == 0) ? (1) : (movies.Count + MaxPageSize - 1) / MaxPageSize,
                    currentPage = page
                };

                if (pageInfo.pageSize < 1)
                {
                    return StatusCode(404, GenericConstants.NoSuchPage);
                }

                return StatusCode(200, new MoviesPagedListDTO
                {
                    movies = moviesPage,
                    pageInfo = pageInfo
                });
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return StatusCode(500, GenericConstants.InternalError);
            }
        }

        [HttpGet("details/{id}")]
        public async Task<IActionResult> GetMovie(Guid id)
        {
            try
            {
                if (!(await _context.Movies.AnyAsync(x => x.Id == id)))
                {
                    return StatusCode(404, GenericConstants.NoSuchMovie);
                }
                var movie = await _context.Movies.Where(x => x.Id == id).Include(x => x.Genres).Include(x => x.Reviews).ThenInclude(x => x.User).Select(x => new MovieDetailsDTO
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
                        author = Convert.ToBoolean(y.IsAnonymous) ? 
                        new UserShortDTO
                        {
                            userId = Guid.Empty,
                            avatar = GenericConstants.DefaultAvatar,
                            nickName = GenericConstants.DefaultNickname
                        } : 
                        new UserShortDTO
                        {
                            userId = y.User.Id,
                            avatar = y.User.AvatarLink,
                            nickName = y.User.Username
                        }
                    }).ToList(),
                    tagline = x.Tagline,
                    time = x.Time,
                    year = x.Year
                }).SingleOrDefaultAsync();

                return StatusCode(200, movie);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return StatusCode(500, GenericConstants.InternalError);
            }
        }
    }
}
