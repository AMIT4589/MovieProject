using System.ComponentModel.DataAnnotations;

namespace MovieBookingApplication.BookingModels.DataTransferObjects
{
    public class ForgotDataTransferObject
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}
