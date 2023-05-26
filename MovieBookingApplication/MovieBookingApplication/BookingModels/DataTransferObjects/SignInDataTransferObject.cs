﻿using System.ComponentModel.DataAnnotations;

namespace MovieBookingApplication.BookingModels.DataTransferObjects
{
    public class SignInDataTransferObject
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
