using MovieCatalog.DAL.Models;
using System.ComponentModel.DataAnnotations;

namespace MovieCatalog.DTO
{
    public class ProfileDTO
    {
        [Required]
        public Guid id { get; set; }

        [Required]
        public string nickName { get; set; }

        [Required]
        [EmailAddress]
        public string email { get; set; }
        public string avatarLink { get; set; }

        [Required]
        public string name { get; set; }
        public DateTime? birthDate { get; set; }
        public Gender? gender { get; set; }
    }
}
