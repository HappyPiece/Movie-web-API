using System.ComponentModel.DataAnnotations;

namespace MovieCatalog.DAL.Models
{
    public class User
    {
        // mandatory fields >>>
        [Required]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
        // <<< mandatory fields

        // optional fields >>>
        public string? Email { get; set; }
        public string? Name { get; set; }
        public bool? IsAdmin { get; set; } = false;
        public Gender? Gender { get; set; }
        // <<< optional fields

        // linking fields >>>
        public List<Review> Reviews { get; set; }
        public List<Movie> FavouriteMovies { get; set; }
        // <<< linking fields
    }
    
    public enum Gender
    {
        Female = 0,
        Male = 1
    }
}
