using System.ComponentModel.DataAnnotations;

namespace MovieCatalog.DTO
{
    public class ReviewModifyDTO
    {
        [Required]
        public string reviewText { get; set; }

        [Range(0,10)]
        public int rating { get; set; }

        public bool isAnonymous { get; set; }
    }
}
