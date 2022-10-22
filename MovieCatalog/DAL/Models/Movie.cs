using System.ComponentModel.DataAnnotations;

namespace MovieCatalog.DAL.Models
{
    public class Movie
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
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
    }
}
