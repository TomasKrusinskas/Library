using System.ComponentModel.DataAnnotations;

namespace Backend_API.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PictureUrl { get; set; }
        public int Year { get; set; }
        [Required]
        [RegularExpression("Book|Audiobook", ErrorMessage = "Type must be either 'Book' or 'Audiobook'.")]
        public string Type { get; set; }
    }
}
