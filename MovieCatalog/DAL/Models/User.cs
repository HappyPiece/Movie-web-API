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
        public DateTime? BirthDate { get; set; }
        public bool? IsAdmin { get; set; } = false;
        public Gender? Gender { get; set; }
        public string? AvatarLink { get; set; }
        // <<< optional fields

        // linking fields >>>
        public List<Review> Reviews { get; set; } = new List<Review>();
        public List<Movie> FavouriteMovies { get; set; } = new List<Movie>();
        // <<< linking fields
    }
    
    public enum Gender
    {
        Female = 0,
        Male = 1,
        ApacheAttackHelicopter = 69
    }
}
