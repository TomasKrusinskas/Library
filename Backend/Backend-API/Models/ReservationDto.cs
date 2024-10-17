using System.ComponentModel.DataAnnotations;

namespace Backend_API.Models
{
    public class ReservationDto
    {
        [Required]
        public int BookId { get; set; }
        [Required]
        public string UserId { get; set; }

        [Required]
        [RegularExpression("Book|Audiobook", ErrorMessage = "Type must be either 'Book' or 'Audiobook'.")]
        public string Type { get; set; }

        [Required]
        public int Days { get; set; }
        public bool QuickPickup { get; set; }
        public decimal TotalPrice { get; set; }
        public string BookName { get; set; }
    }
}
