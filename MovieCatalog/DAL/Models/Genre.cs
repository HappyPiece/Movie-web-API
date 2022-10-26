using System.ComponentModel.DataAnnotations;

namespace MovieCatalog.DAL.Models
{
    public class Genre
    {
        // mandatory fields >>>
        [Required]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Name { get; set; }
        // <<< mandatory fields

        // linking fields >>>
        public List<Movie> Movies { get; set; } = new List<Movie>();
        // <<< linking fields
    }
}
