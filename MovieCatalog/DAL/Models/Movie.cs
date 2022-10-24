using System.ComponentModel.DataAnnotations;

namespace MovieCatalog.DAL.Models
{
    public class Movie
    {
        // mandatory fields >>>
        [Required]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Name { get; set; } = "Untitled";
        // <<< mandatory fields

        // optional fields >>>
        public string? Poster { get; set; }
        public string? Description { get; set; }
        public int? Year { get; set; }
        public string? Country { get; set; }
        public int? Time { get; set; }
        public string? Tagline { get; set; }
        public string? Director { get; set; }
        public int? Budget { get; set; }
        public int? Fees { get; set; }
        public int? AgeLimit { get; set; }
        // <<< optional fields

        // linking fields >>>
        public List<Genre> Genres { get; set; } = new List<Genre>();
        public List<Review> Reviews { get; set; } = new List<Review>();
        public List<User> Users { get; set; } = new List<User>();
        // <<< linking fields
    }
}
