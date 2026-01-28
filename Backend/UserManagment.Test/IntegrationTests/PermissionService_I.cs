using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UserManagement.API.Data;
using UserManagement.API.DTOs;
using UserManagement.API.Models;
using UserManagement.API.Services;

namespace UserManagement.Test.IntegrationTests
{
    public class PermissionService_I : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly AppDbContext _context;
        private readonly PermissionService _service;

        public PermissionService_I()
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

            // Logger
            var logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<PermissionService>();

            // Service instance
            _service = new PermissionService(_context, logger);
        }

        public void Dispose()
        {
            _context.Dispose();
            _connection.Close();
            _connection.Dispose();
        }

        // ------------------- CREATE PERMISSION -------------------
        [Fact]
        public async Task CreatePermissionAsync_CreatesPermissionInDb()
        {
            var dto = new PermissionCreateDTO
            {
                Name = "Read"
            };

            var result = await _service.CreatePermissionAsync(dto);

            Assert.NotNull(result);
            Assert.Equal("Read", result.Name);

            // Verify in DB
            var permissionInDb = await _context.Permissions.FindAsync(result.Id);
            Assert.NotNull(permissionInDb);
            Assert.Equal("Read", permissionInDb.Name);
        }

        // ------------------- GET PERMISSION -------------------
        [Fact]
        public async Task GetPermissionByIdAsync_ReturnsPermission_WhenExists()
        {
            var permission = new Permission { Name = "Write" };
            _context.Permissions.Add(permission);
            _context.SaveChanges();

            var result = await _service.GetPermissionByIdAsync(permission.Id);

            Assert.NotNull(result);
            Assert.Equal("Write", result.Name);
        }

        [Fact]
        public async Task GetAllPermissionsAsync_ReturnsPermissionsList()
        {
            _context.Permissions.Add(new Permission { Name = "Delete" });
            _context.Permissions.Add(new Permission { Name = "Update" });
            _context.SaveChanges();

            var result = await _service.GetAllPermissionsAsync();
            Assert.NotNull(result);
            Assert.True(result.Count() >= 2);
        }

        // ------------------- UPDATE PERMISSION -------------------
        [Fact]
        public async Task UpdatePermissionAsync_UpdatesPermissionInDb()
        {
            var permission = new Permission { Name = "OldPermission" };
            _context.Permissions.Add(permission);
            _context.SaveChanges();

            var dto = new PermissionUpdateDTO
            {
                Name = "NewPermission"
            };

            var result = await _service.UpdatePermissionAsync(permission.Id, dto);
            Assert.True(result);

            var updated = await _context.Permissions.FindAsync(permission.Id);
            Assert.Equal("NewPermission", updated.Name);
        }

        // ------------------- DELETE PERMISSION ------------------- 
        [Fact]
        public async Task DeletePermissionAsync_RemovesPermissionFromDb()
        {
            var permission = new Permission { Name = "DeletePermission" };
            _context.Permissions.Add(permission);
            _context.SaveChanges();

            var result = await _service.DeletePermissionAsync(permission.Id);
            Assert.True(result);

            var deleted = await _context.Permissions.FindAsync(permission.Id);
            Assert.Null(deleted);
        }
    }
}
