using System.ComponentModel.DataAnnotations;

namespace MovieCatalog.DAL.Models
{
    public class Review
    {
        // mandatory fields >>>
        [Required]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string ReviewText { get; set; }
        // <<< mandatory fields

        // optional fields >>>
        public int? Rating { get; set; }
        public bool? IsAnonymous { get; set; }
        public DateTime? CreationDateTime { get; set; } = DateTime.UtcNow;
        // <<< optional fields

        // linking fields >>>
        public Movie Movie { get; set; }
        public Guid MovieId { get; set; }

        public User User { get; set; }
        public Guid UserId { get; set; }
        // <<< linking fields
    }
}
