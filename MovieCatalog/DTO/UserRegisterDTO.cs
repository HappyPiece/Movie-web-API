﻿using System.ComponentModel.DataAnnotations;

namespace MovieCatalog.DTO
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

        public DateTime? birthDate { get; set; }

        public GenderDTO? gender { get; set; }
    }
}
