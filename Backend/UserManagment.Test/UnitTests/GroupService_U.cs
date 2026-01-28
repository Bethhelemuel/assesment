using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using UserManagement.API.Data;
using UserManagement.API.DTOs;
using UserManagement.API.Models;
using UserManagement.API.Services;

namespace UserManagement.Test.UnitTests.Services
{
    public class GroupService_U
    {
        private readonly GroupService _service;
        private readonly AppDbContext _context;

        public GroupService_U()
        {
            // Unique in-memory database
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);

            // Seed some permissions
            _context.Permissions.AddRange(
                new Permission { Name = "Read" },
                new Permission { Name = "Write" }
            );
            _context.SaveChanges();

            // Logger
            var mockLogger = new Mock<ILogger<GroupService>>();

            // Service instance
            _service = new GroupService(_context, mockLogger.Object);
        }

        // ------------------- CREATE GROUP -------------------
        [Fact]
        public async Task CreateGroupAsync_Throws_WhenNoPermissions()
        {
            var dto = new GroupCreateDTO
            {
                Name = "Admins",
                PermissionIds = new List<int>() // empty
            };

            await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateGroupAsync(dto));
        }

        [Fact]
        public async Task CreateGroupAsync_Throws_WhenInvalidPermissions()
        {
            var dto = new GroupCreateDTO
            {
                Name = "Admins",
                PermissionIds = new List<int> { 999 } // does not exist
            };

            await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateGroupAsync(dto));
        }

        [Fact]
        public async Task CreateGroupAsync_ReturnsGroupDTO_WhenValid()
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
        }

        // ------------------- GET ALL GROUPS -------------------
        [Fact]
        public async Task GetAllGroupsAsync_ReturnsList()
        {
            var result = await _service.GetAllGroupsAsync();
            Assert.NotNull(result);
            Assert.True(result.Count() >= 0);
        }

        // ------------------- GET GROUP BY ID -------------------
        [Fact]
        public async Task GetGroupByIdAsync_ReturnsNull_WhenNotFound()
        {
            var result = await _service.GetGroupByIdAsync(999);
            Assert.Null(result);
        }

        [Fact]
        public async Task GetGroupByIdAsync_ReturnsGroupDTO_WhenExists()
        {
            var group = new Group { Name = "TestGroup" };
            _context.Groups.Add(group);
            _context.SaveChanges();

            var result = await _service.GetGroupByIdAsync(group.Id);

            Assert.NotNull(result);
            Assert.Equal("TestGroup", result.Name);
        }

        // ------------------- UPDATE GROUP -------------------
        [Fact]
        public async Task UpdateGroupAsync_ReturnsFalse_WhenGroupNotFound()
        {
            var dto = new GroupUpdateDTO
            {
                Name = "Updated",
                PermissionIds = new List<int> { 1 }
            };

            var result = await _service.UpdateGroupAsync(999, dto);
            Assert.False(result);
        }

        [Fact]
        public async Task UpdateGroupAsync_Throws_WhenNoPermissions()
        {
            var group = new Group { Name = "TestGroup" };
            _context.Groups.Add(group);
            _context.SaveChanges();

            var dto = new GroupUpdateDTO
            {
                Name = "Updated",
                PermissionIds = new List<int>() // empty
            };

            await Assert.ThrowsAsync<ArgumentException>(() => _service.UpdateGroupAsync(group.Id, dto));
        }

        [Fact]
        public async Task UpdateGroupAsync_UpdatesGroup_WhenValid()
        {
            var group = new Group { Name = "OldName" };
            _context.Groups.Add(group);
            _context.SaveChanges();

            var dto = new GroupUpdateDTO
            {
                Name = "NewName",
                PermissionIds = new List<int> { 1, 2 }
            };

            var result = await _service.UpdateGroupAsync(group.Id, dto);
            Assert.True(result);

            var updatedGroup = await _context.Groups.FindAsync(group.Id);
            Assert.Equal("NewName", updatedGroup.Name);
        }

        // ------------------- DELETE GROUP -------------------
        [Fact]
        public async Task DeleteGroupAsync_ReturnsFalse_WhenNotFound()
        {
            var result = await _service.DeleteGroupAsync(999);
            Assert.False(result);
        }

        [Fact]
        public async Task DeleteGroupAsync_DeletesGroup_WhenExists()
        {
            var group = new Group { Name = "DeleteMe" };
            _context.Groups.Add(group);
            _context.SaveChanges();

            var result = await _service.DeleteGroupAsync(group.Id);
            Assert.True(result);

            var deletedGroup = await _context.Groups.FindAsync(group.Id);
            Assert.Null(deletedGroup);
        }
    }
}
