using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UserManagement.API.Data;
using UserManagement.API.DTOs;
using UserManagement.API.Models;
using UserManagement.API.Services;


namespace UserManagement.Test.IntegrationTests
{
    public class GroupService_I : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly AppDbContext _context;
        private readonly GroupService _service;

        public GroupService_I()
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

            // Seed test data (permissions)
            _context.Permissions.AddRange(
                new Permission { Name = "Read" },
                new Permission { Name = "Write" }
            );
            _context.SaveChanges();

            // Logger
            var logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<GroupService>();

            // Service instance
            _service = new GroupService(_context, logger);
        }

        public void Dispose()
        {
            _context.Dispose();
            _connection.Close();
            _connection.Dispose();
        }

        // ------------------- CREATE GROUP -------------------
        [Fact]
        public async Task CreateGroupAsync_CreatesGroupInDb()
        {
            var dto = new GroupCreateDTO
            {
                Name = "Admins",
                PermissionIds = new List<int> { 1, 2 }
            };

            var result = await _service.CreateGroupAsync(dto);

            Assert.NotNull(result);
            Assert.Equal("Admins", result.Name);
            Assert.Equal(2, result.Permissions.Count);

            // Verify in DB
            var groupInDb = await _context.Groups.Include(g => g.GroupPermissions).FirstOrDefaultAsync(g => g.Id == result.Id);
            Assert.NotNull(groupInDb);
            Assert.Equal(2, groupInDb.GroupPermissions.Count);
        }

        // ------------------- GET GROUP -------------------
        [Fact]
        public async Task GetGroupByIdAsync_ReturnsGroup_WhenExists()
        {
            var group = new Group { Name = "TestGroup" };
            _context.Groups.Add(group);
            _context.SaveChanges();

            var result = await _service.GetGroupByIdAsync(group.Id);

            Assert.NotNull(result);
            Assert.Equal("TestGroup", result.Name);
        }

        [Fact]
        public async Task GetAllGroupsAsync_ReturnsGroupsList()
        {
            _context.Groups.Add(new Group { Name = "Group1" });
            _context.Groups.Add(new Group { Name = "Group2" });
            _context.SaveChanges();

            var result = await _service.GetAllGroupsAsync();
            Assert.NotNull(result);
            Assert.True(result.Count() >= 2);
        }

        // ------------------- UPDATE GROUP -------------------
        [Fact]
        public async Task UpdateGroupAsync_UpdatesGroupInDb()
        {
            var group = new Group { Name = "OldGroup" };
            _context.Groups.Add(group);
            _context.SaveChanges();

            var dto = new GroupUpdateDTO
            {
                Name = "NewGroup",
                PermissionIds = new List<int> { 1 } // Update permissions
            };

            var result = await _service.UpdateGroupAsync(group.Id, dto);
            Assert.True(result);

            var updated = await _context.Groups.Include(g => g.GroupPermissions).FirstOrDefaultAsync(g => g.Id == group.Id);
            Assert.Equal("NewGroup", updated.Name);
            Assert.Single(updated.GroupPermissions);
        }

        // ------------------- DELETE GROUP -------------------
        [Fact]
        public async Task DeleteGroupAsync_RemovesGroupFromDb()
        {
            var group = new Group { Name = "DeleteGroup" };
            _context.Groups.Add(group);
            _context.SaveChanges();

            var result = await _service.DeleteGroupAsync(group.Id);
            Assert.True(result);

            var deleted = await _context.Groups.FindAsync(group.Id);
            Assert.Null(deleted);
        }
    }
}
