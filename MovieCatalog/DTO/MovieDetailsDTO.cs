using MovieCatalog.DAL.Models;
using System.ComponentModel.DataAnnotations;

namespace MovieCatalog.DTO
{
    public class MovieDetailsDTO
    {
        // mandatory fields >>>
        [Required]
        public Guid id { get; set; }

        [Required]
        public string name { get; set; }
        // <<< mandatory fields

        // optional fields >>>
        public string? poster { get; set; }
        public string? description { get; set; }
        public int? year { get; set; }
        public string? country { get; set; }
        public int? time { get; set; }
        public string? tagline { get; set; }
        public string? director { get; set; }
        public int? budget { get; set; }
        public int? fees { get; set; }
        public int? ageLimit { get; set; }
        // <<< optional fields

        // linking fields >>>
        public List<GenreDTO>? genres { get; set; }
        public List<ReviewDTO>? reviews { get; set; }
        // <<< linking fields
    }
}
