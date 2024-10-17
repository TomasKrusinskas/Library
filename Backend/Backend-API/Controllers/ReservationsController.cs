using Backend_API.Data;
using Backend_API.Models;
using Backend_API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationsController : ControllerBase
    {
        private readonly LibraryContext _context;
        private readonly PricingService _pricingService;
        private readonly IMapper _mapper;

        public ReservationsController(LibraryContext context, PricingService pricingService, IMapper mapper)
        {
            _context = context;
            _pricingService = pricingService;
            _mapper = mapper;
        }

        [HttpPost]
        public IActionResult CreateReservation([FromBody] ReservationDto reservationDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var bookExists = _context.Books.Any(b => b.Id == reservationDto.BookId);
            if (!bookExists)
            {
                return NotFound($"Book with ID {reservationDto.BookId} does not exist.");
            }

            var reservation = _mapper.Map<Reservation>(reservationDto);
            reservation.TotalPrice = _pricingService.CalculatePrice(reservationDto.Type, reservationDto.Days, reservationDto.QuickPickup);

            _context.Reservations.Add(reservation);
            _context.SaveChanges();

            return Ok(reservation);
        }


        [HttpGet]
        public IActionResult GetReservations([FromQuery] string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("UserId is required.");
            }

            var reservations = _context.Reservations
                .Include(r => r.Book)
                .Where(r => r.UserId == userId)
                .ToList();

            var reservationDtos = _mapper.Map<List<ReservationDto>>(reservations);

            return Ok(reservationDtos);
        }

    }
}
