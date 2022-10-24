using MovieCatalog.DAL.Models;
using System.ComponentModel.DataAnnotations;

namespace MovieCatalog.DTO
{
    public class ProfileDTO
    {
        // mandatory fields >>>
        [Required]
        public Guid id { get; set; }

        [Required]
        public string nickName { get; set; }

        [Required]
        [EmailAddress]
        public string email { get; set; }

        [Required]
        public string name { get; set; }
        // <<< mandatory fields

        // optional fields >>>
        public string? avatarLink { get; set; }
        public DateTime? birthDate { get; set; }
        public GenderDTO? gender { get; set; }
        // <<< optional fields
    }
}
