using MovieCatalog.DAL.Models;
using System.ComponentModel.DataAnnotations;

namespace MovieCatalog.DTO
{
    public class MovieElementDTO
    {
        // mandatory fields >>>
        [Required]
        public Guid id { get; set; }

        [Required]
        public string? name { get; set; }
        // <<< mandatory fields

        // optional fields >>>
        public string? poster { get; set; }
        public int? year { get; set; }
        public string? country { get; set; }
        // <<< optional fields

        // linking fields >>>
        public List<GenreDTO>? genres { get; set; }
        public List<ReviewShortDTO>? reviews { get; set; }
        // <<< linking fields
    }
}
