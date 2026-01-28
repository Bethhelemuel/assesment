using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using UserManagement.API.Data;
using UserManagement.API.DTOs;
using UserManagement.API.Models;
using UserManagement.API.Services;

namespace UserManagement.Test.UnitTests.Services
{
    public class PermissionService_U
    {
        private readonly PermissionService _service;
        private readonly AppDbContext _context;

        public PermissionService_U()
        {
            // Unique in-memory database
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);

            // Logger
            var mockLogger = new Mock<ILogger<PermissionService>>();

            // Service instance
            _service = new PermissionService(_context, mockLogger.Object);
        }

        // ------------------- CREATE PERMISSION -------------------
        [Fact]
        public async Task CreatePermissionAsync_Throws_WhenNameEmpty()
        {
            var dto = new PermissionCreateDTO { Name = "" };
            await Assert.ThrowsAsync<ArgumentException>(() => _service.CreatePermissionAsync(dto));
        }

        [Fact]
        public async Task CreatePermissionAsync_ReturnsPermissionDTO_WhenValid()
        {
            var dto = new PermissionCreateDTO { Name = "Read" };
            var result = await _service.CreatePermissionAsync(dto);

            Assert.NotNull(result);
            Assert.Equal("Read", result.Name);
            Assert.True(result.Id > 0);
        }

        // ------------------- GET ALL PERMISSIONS -------------------
        [Fact]
        public async Task GetAllPermissionsAsync_ReturnsList()
        {
            // Seed a permission
            _context.Permissions.Add(new Permission { Name = "Write" });
            _context.SaveChanges();

            var result = await _service.GetAllPermissionsAsync();
            Assert.NotNull(result);
            Assert.True(result.Count() >= 1);
        }

        // ------------------- GET PERMISSION BY ID -------------------
        [Fact]
        public async Task GetPermissionByIdAsync_ReturnsNull_WhenNotFound()
        {
            var result = await _service.GetPermissionByIdAsync(999);
            Assert.Null(result);
        }

        [Fact]
        public async Task GetPermissionByIdAsync_ReturnsPermissionDTO_WhenExists()
        {
            var permission = new Permission { Name = "Delete" };
            _context.Permissions.Add(permission);
            _context.SaveChanges();

            var result = await _service.GetPermissionByIdAsync(permission.Id);
            Assert.NotNull(result);
            Assert.Equal("Delete", result.Name);
        }

        // ------------------- UPDATE PERMISSION -------------------
        [Fact]
        public async Task UpdatePermissionAsync_Throws_WhenNameEmpty()
        {
            var permission = new Permission { Name = "Old" };
            _context.Permissions.Add(permission);
            _context.SaveChanges();

            var dto = new PermissionUpdateDTO { Name = "" };
            await Assert.ThrowsAsync<ArgumentException>(() => _service.UpdatePermissionAsync(permission.Id, dto));
        }

        [Fact]
        public async Task UpdatePermissionAsync_ReturnsFalse_WhenNotFound()
        {
            var dto = new PermissionUpdateDTO { Name = "New" };
            var result = await _service.UpdatePermissionAsync(999, dto);
            Assert.False(result);
        }

        [Fact]
        public async Task UpdatePermissionAsync_UpdatesPermission_WhenValid()
        {
            var permission = new Permission { Name = "Old" };
            _context.Permissions.Add(permission);
            _context.SaveChanges();

            var dto = new PermissionUpdateDTO { Name = "New" };
            var result = await _service.UpdatePermissionAsync(permission.Id, dto);

            Assert.True(result);
            var updated = await _context.Permissions.FindAsync(permission.Id);
            Assert.Equal("New", updated.Name);
        }

        // ------------------- DELETE PERMISSION -------------------
        [Fact]
        public async Task DeletePermissionAsync_ReturnsFalse_WhenNotFound()
        {
            var result = await _service.DeletePermissionAsync(999);
            Assert.False(result);
        }

        [Fact]
        public async Task DeletePermissionAsync_DeletesPermission_WhenExists()
        {
            var permission = new Permission { Name = "DeleteMe" };
            _context.Permissions.Add(permission);
            _context.SaveChanges();

            var result = await _service.DeletePermissionAsync(permission.Id);
            Assert.True(result);

            var deleted = await _context.Permissions.FindAsync(permission.Id);
            Assert.Null(deleted);
        }
    }
}
