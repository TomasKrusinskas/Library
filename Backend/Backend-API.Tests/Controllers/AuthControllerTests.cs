using Backend_API.Controllers;
using Backend_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace Backend_API.Tests.Controllers
{
    public class AuthControllerTests
    {
        private readonly AuthController _controller;
        private readonly Mock<UserManager<IdentityUser>> _userManagerMock;
        private readonly Mock<SignInManager<IdentityUser>> _signInManagerMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly Mock<IUserClaimsPrincipalFactory<IdentityUser>> _claimsFactoryMock;

        public AuthControllerTests()
        {
            var store = new Mock<IUserStore<IdentityUser>>();
            _userManagerMock = new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);

            _claimsFactoryMock = new Mock<IUserClaimsPrincipalFactory<IdentityUser>>();
            _claimsFactoryMock.Setup(x => x.CreateAsync(It.IsAny<IdentityUser>()))
                .ReturnsAsync(new ClaimsPrincipal(new ClaimsIdentity()));

            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();

            _signInManagerMock = new Mock<SignInManager<IdentityUser>>(
                _userManagerMock.Object,
                _httpContextAccessorMock.Object,
                _claimsFactoryMock.Object,
                null, null, null, null
            );

            _controller = new AuthController(_userManagerMock.Object, _signInManagerMock.Object);
        }

        [Fact]
        public async Task Register_UserAlreadyExists_ReturnsBadRequest()
        {
            var userDto = new UserDto { Name = "testuser", Password = "password123" };

            _userManagerMock.Setup(u => u.FindByNameAsync(userDto.Name)).ReturnsAsync(new IdentityUser());

            var result = await _controller.Register(userDto);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("User already exists.", badRequestResult.Value);
        }

        [Fact]
        public async Task Login_InvalidCredentials_ReturnsUnauthorized()
        {
            var userDto = new UserDto { Name = "testuser", Password = "wrongpassword" };

            _signInManagerMock
                .Setup(sm => sm.PasswordSignInAsync(userDto.Name, userDto.Password, false, false))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);

            var result = await _controller.Login(userDto);

            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("Invalid credentials.", unauthorizedResult.Value);
        }
    }
}
