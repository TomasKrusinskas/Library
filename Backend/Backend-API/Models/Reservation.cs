namespace Backend_API.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string UserId { get; set; }
        public string Type { get; set; }
        public int Days { get; set; }
        public bool QuickPickup { get; set; }
        public decimal TotalPrice { get; set; }
        public Book Book { get; set; }
    }
}
