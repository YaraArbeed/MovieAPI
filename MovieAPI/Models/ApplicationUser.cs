﻿using Microsoft.AspNetCore.Identity;

namespace MovieAPI.Models
{
    public class ApplicationUser:IdentityUser
    {
        [MaxLength(50)]
        public string FirstName { get; set; }

        [MaxLength(50)]
        public string LastName { get; set; }

    }
}
