using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieCatalog.DAL;
using MovieCatalog.DAL.Models;
using MovieCatalog.DTO;

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
        public async Task<IActionResult> addReview(Guid movieId, ReviewModifyDTO reviewModifyDTO)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ModelState);
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
            return StatusCode(200, "Review successfully added");
        }

        [HttpPut("{id}/edit")]
        [Authorize]
        public async Task<IActionResult> editReview(Guid movieId, Guid id, ReviewModifyDTO reviewModifyDTO)
        {
            if (!ModelState.IsValid)
            {
                return StatusCode(400, ModelState);
            }

            var movie = await _context.Movies.Where(x => x.Id == movieId).Include(x => x.Reviews).SingleOrDefaultAsync();
            var review = movie.Reviews.Where(x => x.Id == id).SingleOrDefault();
            {
                review.ReviewText = reviewModifyDTO.reviewText;
                review.Rating = reviewModifyDTO.rating;
                review.IsAnonymous = reviewModifyDTO.isAnonymous;
            }
            await _context.SaveChangesAsync();

            return StatusCode(200, "Review successfully edited");
        }

        [HttpDelete("{id}/delete")]
        [Authorize]
        public async Task<IActionResult> deleteReview(Guid movieId, Guid id)
        {
            var movie = await _context.Movies.Where(x => x.Id == movieId).Include(x => x.Reviews).SingleOrDefaultAsync();
            if (movie.Reviews.Remove(movie.Reviews.Where(x => x.Id == id).SingleOrDefault()))
            {
                await _context.SaveChangesAsync();
                return StatusCode(200, "Review successfully deleted");
            }
            return StatusCode(500, "Unable to delete review");
        }
    }
}
