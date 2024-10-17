using Backend_API.Controllers;
using Backend_API.Data;
using Backend_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend_API.Tests.Controllers
{
    public class BooksControllerTests
    {
        private readonly BooksController _controller;
        private readonly LibraryContext _context;

        public BooksControllerTests()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new LibraryContext(options);
            _controller = new BooksController(_context, null);

            _context.Books.AddRange(new List<Book>
            {
                new Book { Id = 1, Name = "Book1", PictureUrl = "http://example.com/book1.jpg", Year = 2021, Type = "Book" },
                new Book { Id = 2, Name = "Book2", PictureUrl = "http://example.com/book2.jpg", Year = 2022, Type = "Audiobook" }
            });
            _context.SaveChanges();
        }

        [Fact]
        public void GetBooks_NoFilters_ReturnsAllBooks()
        {
            var result = _controller.GetBooks(null, null, null);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnBooks = Assert.IsType<List<Book>>(okResult.Value);
            Assert.Equal(2, returnBooks.Count);
        }
    }
}
