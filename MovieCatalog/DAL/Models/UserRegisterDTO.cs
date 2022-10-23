using System.ComponentModel.DataAnnotations;

namespace MovieCatalog.DAL.Models
{
    public class UserRegisterDTO
    {
        [Required]
        public string userName { get; set; }

        [Required]
        public string name { get; set; }

        [Required]
        public string password { get; set; }

        [Required]
        [EmailAddress]
        public string email { get; set; }

        public DateTime birthDate { get; set; }

        [Range(0,1)]
        public int gender { get; set; }
    }
}
