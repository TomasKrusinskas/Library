using Backend_API.Data;
using Backend_API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Backend_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly LibraryContext _context;
        private readonly IMapper _mapper;

        public BooksController(LibraryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetBooks([FromQuery] string? name, [FromQuery] int? year, [FromQuery] string? type)
        {
            var books = _context.Books.AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                books = books.Where(b => b.Name.Contains(name));
            }
            if (year.HasValue)
            {
                books = books.Where(b => b.Year == year.Value);
            }
            if (!string.IsNullOrEmpty(type))
            {
                books = books.Where(b => b.Type == type);
            }

            return Ok(books.ToList());
        }

        [HttpGet("{id}")]
        public IActionResult GetBookById(int id)
        {
            var book = _context.Books.Find(id);

            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBook([FromBody] Book bookDto)
        {
            if (bookDto == null)
            {
                return BadRequest("Book is null.");
            }

            var book = _mapper.Map<Book>(bookDto);

            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBookById), new { id = book.Id }, book);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] Book updatedBookDto)
        {
            if (updatedBookDto == null || id != updatedBookDto.Id)
            {
                return BadRequest("Book data is invalid.");
            }

            var existingBook = await _context.Books.FindAsync(id);
            if (existingBook == null)
            {
                return NotFound();
            }

            _mapper.Map(updatedBookDto, existingBook);

            await _context.SaveChangesAsync();

            return Ok(existingBook);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
