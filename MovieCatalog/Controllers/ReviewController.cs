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
    [Route("api/movie/{movieId}/review")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly MovieCatalogDbContext _context;
        public ReviewController(MovieCatalogDbContext context)
        {
            _context = context;
        }

        [HttpPost("add")]
        [Authorize]
        public async Task<IActionResult> AddReview(Guid movieId, ReviewModifyDTO reviewModifyDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(400, ModelState);
                }

                if (!(await _context.Movies.AnyAsync(x => x.Id == movieId)))
                {
                    return StatusCode(404, GenericConstants.NoSuchMovie);
                }

                var movie = await _context.Movies.Where(x => x.Id == movieId).Include(x => x.Reviews).SingleOrDefaultAsync();
                var review = new Review
                {
                    ReviewText = reviewModifyDTO.reviewText,
                    Rating = reviewModifyDTO.rating,
                    IsAnonymous = reviewModifyDTO.isAnonymous,
                    UserId = Guid.Parse(User.Identity.Name),
                    User = await _context.Users.Where(x => x.Id == Guid.Parse(User.Identity.Name)).SingleOrDefaultAsync(),
                    MovieId = movieId,
                    Movie = movie
                };

                movie.Reviews.Add(review);
                _context.Entry(review).State = EntityState.Added;
                await _context.SaveChangesAsync();

                return StatusCode(200, GenericConstants.ReviewAdded);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return StatusCode(500, GenericConstants.InternalError);
            }
        }

        [HttpPut("{id}/edit")]
        [Authorize]
        public async Task<IActionResult> EditReview(Guid movieId, Guid id, ReviewModifyDTO reviewModifyDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return StatusCode(400, ModelState);
                }

                if (!(await _context.Movies.AnyAsync(x => x.Id == movieId)))
                {
                    return StatusCode(404, GenericConstants.NoSuchMovie);
                }

                var movie = await _context.Movies.Where(x => x.Id == movieId).Include(x => x.Reviews).ThenInclude(x => x.User).SingleOrDefaultAsync();
                if (!movie.Reviews.Any(x => x.Id == id))
                {
                    return StatusCode(404, GenericConstants.NoSuchReview);
                }

                var review = movie.Reviews.Where(x => x.Id == id).SingleOrDefault();
                if (review.User.Id.ToString() != User.Identity.Name)
                {
                    Console.WriteLine(review.User.ToString());
                    Console.WriteLine(User.Identity.Name);
                    return StatusCode(403, GenericConstants.NotYourReview);
                }

                {
                    review.ReviewText = reviewModifyDTO.reviewText;
                    review.Rating = reviewModifyDTO.rating;
                    review.IsAnonymous = reviewModifyDTO.isAnonymous;
                }
                await _context.SaveChangesAsync();

                return StatusCode(200, GenericConstants.ReviewEdited);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return StatusCode(500, GenericConstants.InternalError);
            }
        }

        [HttpDelete("{id}/delete")]
        [Authorize]
        public async Task<IActionResult> DeleteReview(Guid movieId, Guid id)
        {
            try
            {
                if (!(await _context.Movies.AnyAsync(x => x.Id == movieId)))
                {
                    return StatusCode(404, GenericConstants.NoSuchMovie);
                }

                var movie = await _context.Movies.Where(x => x.Id == movieId).Include(x => x.Reviews).ThenInclude(x => x.User).SingleOrDefaultAsync();
                if (!movie.Reviews.Any(x => x.Id == id))
                {
                    return StatusCode(404, GenericConstants.NoSuchReview);
                }

                var review = movie.Reviews.Where(x => x.Id == id).SingleOrDefault();
                if (review.User.Id.ToString() != User.Identity.Name)
                {
                    return StatusCode(403, GenericConstants.NotYourReview);
                }

                movie.Reviews.Remove(review);
                await _context.SaveChangesAsync();
                return StatusCode(200, GenericConstants.ReviewDeleted);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return StatusCode(500, GenericConstants.InternalError);
            }
        }
    }
}
