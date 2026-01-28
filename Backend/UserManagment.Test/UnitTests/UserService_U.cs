using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using UserManagement.API.Data;
using UserManagement.API.DTOs;
using UserManagement.API.Models;
using UserManagement.API.Services;

namespace UserManagement.Test.UnitTests.Services
{
    public class UserService_U
    {
        private readonly UserService _service;
        private readonly AppDbContext _context;

        public UserService_U()
        { 
            // Use a unique in-memory database for isolation
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);

            // Seed some groups for testing
            _context.Groups.AddRange(
                new Group { Name = "Admin" },
                new Group { Name = "User" }
            );
            _context.SaveChanges();

            // Setup logger
            var mockLogger = new Mock<ILogger<UserService>>();

            // Create service instance
            _service = new UserService(_context, mockLogger.Object);
        }

        // ------------------- CREATE USER -------------------
        [Fact]
        public async Task CreateUserAsync_Throws_WhenFullNameIsEmpty()
        {
            var dto = new UserCreateDTO
            {
                FullName = "",
                Email = "test@test.com",
                GroupIds = new List<int> { 1 }
            };

            await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateUserAsync(dto));
        }

        [Fact]
        public async Task CreateUserAsync_Throws_WhenGroupIdsInvalid()
        {
            var dto = new UserCreateDTO
            {
                FullName = "Thato",
                Email = "thato@test.com",
                GroupIds = new List<int> { 999 } // non-existing group
            };

            await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateUserAsync(dto));
        }

        [Fact]
        public async Task CreateUserAsync_ReturnsUserDTO_WhenValid()
        {
            var dto = new UserCreateDTO
            {
                FullName = "Thato",
                Email = "thato@test.com",
                GroupIds = new List<int> { 1 }
            };

            var result = await _service.CreateUserAsync(dto);

            Assert.NotNull(result);
            Assert.Equal("Thato", result.FullName);
            Assert.Equal("thato@test.com", result.Email);
            Assert.Single(result.Groups);
            Assert.Equal(1, result.Groups.First().Id);
        }

        // ------------------- GET USER BY ID -------------------
        [Fact]
        public async Task GetUserByIdAsync_ReturnsNull_WhenNotFound()
        {
            var result = await _service.GetUserByIdAsync(999);
            Assert.Null(result);
        }

        [Fact]
        public async Task GetUserByIdAsync_ReturnsUserDTO_WhenExists()
        {
            var user = new User { FullName = "Jane", Email = "jane@test.com" };
            _context.Users.Add(user);
            _context.SaveChanges();

            var result = await _service.GetUserByIdAsync(user.Id);

            Assert.NotNull(result);
            Assert.Equal("Jane", result.FullName);
        }

        // ------------------- GET ALL USERS -------------------
        [Fact]
        public async Task GetAllUsersAsync_ReturnsList()
        {
            var result = await _service.GetAllUsersAsync();
            Assert.NotNull(result);
            Assert.True(result.Count() >= 0);
        }

        // ------------------- UPDATE USER -------------------
        [Fact]
        public async Task UpdateUserAsync_ReturnsFalse_WhenUserNotFound()
        {
            var dto = new UserUpdateDTO
            {
                FullName = "Updated",
                Email = "updated@test.com",
                GroupIds = new List<int> { 1 }
            };

            var result = await _service.UpdateUserAsync(999, dto);
            Assert.False(result);
        }

        [Fact]
        public async Task UpdateUserAsync_UpdatesUser_WhenValid()
        {
            var user = new User { FullName = "OldName", Email = "old@test.com" };
            _context.Users.Add(user);
            _context.SaveChanges();

            var dto = new UserUpdateDTO
            {
                FullName = "NewName",
                Email = "new@test.com",
                GroupIds = new List<int> { 1, 2 }
            };

            var result = await _service.UpdateUserAsync(user.Id, dto);
            Assert.True(result);

            var updatedUser = await _context.Users.FindAsync(user.Id);
            Assert.Equal("NewName", updatedUser.FullName);
            Assert.Equal("new@test.com", updatedUser.Email);
        }

        // ------------------- DELETE USER -------------------
        [Fact]
        public async Task DeleteUserAsync_ReturnsFalse_WhenUserNotFound()
        {
            var result = await _service.DeleteUserAsync(999);
            Assert.False(result);
        }

        [Fact]
        public async Task DeleteUserAsync_DeletesUser_WhenExists()
        {
            var user = new User { FullName = "DeleteMe", Email = "delete@test.com" };
            _context.Users.Add(user);
            _context.SaveChanges();

            var result = await _service.DeleteUserAsync(user.Id);
            Assert.True(result);

            var deletedUser = await _context.Users.FindAsync(user.Id);
            Assert.Null(deletedUser);
        }
    }
}
