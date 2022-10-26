using System.ComponentModel.DataAnnotations;

namespace MovieCatalog.DTO
{
    public class LoginCredentialsDTO
    {
        [Required]
        public string username { get; set; }

        [Required]
        public string password { get; set; }
    }
}
