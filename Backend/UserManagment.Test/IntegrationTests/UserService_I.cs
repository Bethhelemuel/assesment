using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.API.Data;
using UserManagement.API.DTOs;
using UserManagement.API.Models;
using UserManagement.API.Services;
using Xunit;

namespace UserManagement.Test.IntegrationTests
{
    public class UserService_I : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly AppDbContext _context;
        private readonly UserService _service;

        public UserService_I()
        {
            // Create in-memory SQLite connection
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(_connection)
                .Options;

            // Create DbContext using the real AppDbContext
            _context = new AppDbContext(options);

            // Create tables in memory
            _context.Database.EnsureCreated();

            // Seed test data (groups)
            _context.Groups.AddRange(
                new Group { Name = "Admin" },
                new Group { Name = "User" }
            );
            _context.SaveChanges();

            // Logger
            var logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<UserService>();

            // Service instance
            _service = new UserService(_context, logger);
        }

        public void Dispose()
        {
            _context.Dispose();
            _connection.Close();
            _connection.Dispose();
        }

        // ------------------- CREATE USER -------------------
        [Fact]
        public async Task CreateUserAsync_CreatesUserInDb()
        {
            var dto = new UserCreateDTO
            {
                FullName = "Thato",
                Email = "thato@test.com",
                GroupIds = new List<int> { 1 } // Admin
            };

            var result = await _service.CreateUserAsync(dto);

            Assert.NotNull(result);
            Assert.Equal("Thato", result.FullName);
            Assert.Equal("thato@test.com", result.Email);
            Assert.Single(result.Groups);
            Assert.Equal(1, result.Groups.First().Id);

            // Verify user exists in DB
            var userInDb = _context.Users.Include(u => u.UserGroups).FirstOrDefault(u => u.Id == result.Id);
            Assert.NotNull(userInDb);
            Assert.Single(userInDb.UserGroups);
        }

        // ------------------- GET USER -------------------
        [Fact]
        public async Task GetUserByIdAsync_ReturnsUser_WhenExists()
        {
            var user = new User { FullName = "Jane", Email = "jane@test.com" };
            _context.Users.Add(user);
            _context.SaveChanges();

            var result = await _service.GetUserByIdAsync(user.Id);

            Assert.NotNull(result);
            Assert.Equal("Jane", result.FullName);
        }

        [Fact]
        public async Task GetAllUsersAsync_ReturnsUsersList()
        {
            _context.Users.Add(new User { FullName = "Alice", Email = "alice@test.com" });
            _context.Users.Add(new User { FullName = "Bob", Email = "bob@test.com" });
            _context.SaveChanges();

            var result = await _service.GetAllUsersAsync();
            Assert.NotNull(result);
            Assert.True(result.Count() >= 2);
        }

        // ------------------- UPDATE USER -------------------
        [Fact]
        public async Task UpdateUserAsync_UpdatesUserInDb()
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

            var updated = await _context.Users.Include(u => u.UserGroups).FirstOrDefaultAsync(u => u.Id == user.Id);
            Assert.Equal("NewName", updated.FullName);
            Assert.Equal("new@test.com", updated.Email);
            Assert.Equal(2, updated.UserGroups.Count);
        }

        // ------------------- DELETE USER -------------------
        [Fact]
        public async Task DeleteUserAsync_RemovesUserFromDb()
        {
            var user = new User { FullName = "DeleteMe", Email = "delete@test.com" };
            _context.Users.Add(user);
            _context.SaveChanges();

            var result = await _service.DeleteUserAsync(user.Id);
            Assert.True(result);

            var deleted = await _context.Users.FindAsync(user.Id);
            Assert.Null(deleted);
        }
    }
}
