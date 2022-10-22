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
    }
}
