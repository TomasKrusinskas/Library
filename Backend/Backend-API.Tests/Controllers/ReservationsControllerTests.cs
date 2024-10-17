using Backend_API.Controllers;
using Backend_API.Data;
using Backend_API.Models;
using Backend_API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Backend_API.Tests.Controllers
{
    public class ReservationsControllerTests
    {
        private readonly ReservationsController _controller;
        private readonly LibraryContext _context;
        private readonly Mock<PricingService> _pricingServiceMock;
        private readonly IMapper _mapper;

        public ReservationsControllerTests()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new LibraryContext(options);
            _pricingServiceMock = new Mock<PricingService>();

            var config = new MapperConfiguration(cfg => cfg.CreateMap<ReservationDto, Reservation>());
            _mapper = config.CreateMapper();

            _controller = new ReservationsController(_context, _pricingServiceMock.Object, _mapper);
        }

        [Fact]
        public void CreateReservation_BookDoesNotExist_ReturnsNotFound()
        {
            var reservationDto = new ReservationDto
            {
                BookId = 999,
                UserId = "test-user",
                Days = 3,
                Type = "Book",
                QuickPickup = false
            };

            var result = _controller.CreateReservation(reservationDto);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Book with ID 999 does not exist.", notFoundResult.Value);
        }

        [Fact]
        public void CreateReservation_InvalidModelState_ReturnsBadRequest()
        {
            var reservationDto = new ReservationDto
            {
                BookId = 0,
                UserId = "test-user",
                Days = 3,
                Type = "InvalidType",
                QuickPickup = false
            };

            _controller.ModelState.AddModelError("BookId", "Invalid Book ID");

            var result = _controller.CreateReservation(reservationDto);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }

        [Fact]
        public void GetReservations_UserIdRequired_ReturnsBadRequest()
        {
            var result = _controller.GetReservations(null);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("UserId is required.", badRequestResult.Value);
        }


        [Fact]
        public void GetReservations_NoReservationsForUser_ReturnsEmptyList()
        {
            var userId = "test-user";

            var result = _controller.GetReservations(userId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var reservationDtos = Assert.IsType<List<ReservationDto>>(okResult.Value);
            Assert.Empty(reservationDtos);
        }
    }
}
