using Microsoft.EntityFrameworkCore;
using Backend_API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Backend_API.Data
{
    public class LibraryContext : IdentityDbContext<IdentityUser>
    {
        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options) { }

        public DbSet<Book> Books { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        public static void SeedData(LibraryContext context)
        {
            if (!context.Books.Any())
            {
                context.Books.AddRange(
                    new Book { Name = "The Great Gatsby", PictureUrl = "https://eurotechcenter.eu/wp-content/uploads/2024/04/canstockphoto22402523-arcos-creator.com_-1024x1024-1.jpg", Year = 1925, Type = "Book" },
                    new Book { Name = "1984", PictureUrl = "https://eurotechcenter.eu/wp-content/uploads/2024/04/canstockphoto22402523-arcos-creator.com_-1024x1024-1.jpg", Year = 1949, Type = "Book" },
                    new Book { Name = "Becoming", PictureUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRuwAjENpDBsnhb91rzlQxF39KqlCZxHqHVig&s", Year = 2018, Type = "Audiobook" }
                );
                context.SaveChanges();
            }
        }
    }
}