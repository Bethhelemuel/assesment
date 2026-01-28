using Microsoft.EntityFrameworkCore;
using UserManagement.API.Data;
using UserManagement.API.DTOs;
using UserManagement.API.Interfaces;
using UserManagement.API.Models;

namespace UserManagement.API.Services
{
    public class GroupService : IGroupService
    {
        private readonly AppDbContext _db;
        private readonly ILogger<GroupService> _logger;

        public GroupService(AppDbContext db, ILogger<GroupService> logger)
        {
            _db = db;
            _logger = logger;
        }

        // ------------------------------------ CREATE GROUP  ------------------------------------

        public async Task<GroupDTO> CreateGroupAsync(GroupCreateDTO dto)
        {
            if (dto.PermissionIds == null || !dto.PermissionIds.Any())
                throw new ArgumentException("A group must have at least one permission.");

            _logger.LogInformation("Creating group {GroupName}", dto.Name);

            // Validate permission IDs
            var validPermissions = await _db.Permissions
                .Where(p => dto.PermissionIds.Contains(p.Id))
                .Select(p => p.Id)
                .ToListAsync();

            var invalidPermissions = dto.PermissionIds.Except(validPermissions).ToList();
            if (invalidPermissions.Any())
            {
                throw new ArgumentException($"These permission IDs do not exist: {string.Join(", ", invalidPermissions)}");
            }

            var group = new Group { Name = dto.Name };
            _db.Groups.Add(group);
            await _db.SaveChangesAsync();

            // Assign permissions
            foreach (var permId in validPermissions)
            {
                _db.Set<GroupPermission>().Add(new GroupPermission
                {
                    GroupId = group.Id,
                    PermissionId = permId
                });
            }
            await _db.SaveChangesAsync();

            _logger.LogInformation("Group {GroupName} created with ID {GroupId}", dto.Name, group.Id);

            return new GroupDTO
            {
                Id = group.Id,
                Name = group.Name,
                Permissions = validPermissions.Select(id => new PermissionDTO
                {
                    Id = id,
                    Name = _db.Permissions.Find(id)?.Name ?? ""
                }).ToList()
            };
        }

        // ------------------------------------ GET ALL GROUPS  ------------------------------------

        public async Task<IEnumerable<GroupDTO>> GetAllGroupsAsync()
        {
            _logger.LogInformation("Fetching all groups");

            var groups = await _db.Groups
                .Include(g => g.GroupPermissions)
                    .ThenInclude(gp => gp.Permission)
                .ToListAsync();

            return groups.Select(g => new GroupDTO
            {
                Id = g.Id,
                Name = g.Name,
                Permissions = g.GroupPermissions.Select(gp => new PermissionDTO
                {
                    Id = gp.Permission.Id,
                    Name = gp.Permission.Name
                }).ToList()
            });
        }

        // ------------------------------------ GET GROUP BY ID  ------------------------------------
        public async Task<GroupDTO> GetGroupByIdAsync(int id)
        {
            _logger.LogInformation("Fetching group with ID {GroupId}", id);

            var group = await _db.Groups
                .Include(g => g.GroupPermissions)
                    .ThenInclude(gp => gp.Permission)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (group == null)
            {
                _logger.LogWarning("Group with ID {GroupId} not found", id);
                return null!;
            }

            return new GroupDTO
            {
                Id = group.Id,
                Name = group.Name,
                Permissions = group.GroupPermissions.Select(gp => new PermissionDTO
                {
                    Id = gp.Permission.Id,
                    Name = gp.Permission.Name
                }).ToList()
            };
        }

        // ------------------------------------ UPDATE GROUP  ------------------------------------
        public async Task<bool> UpdateGroupAsync(int id, GroupUpdateDTO dto)
        {
            try
            {
                _logger.LogInformation("Updating group with ID {GroupId}", id);

                var group = await _db.Groups.FindAsync(id);
                if (group == null)
                {
                    _logger.LogWarning("Group with ID {GroupId} not found for update", id);
                    return false;
                }

                if (dto.PermissionIds == null || !dto.PermissionIds.Any())
                    throw new ArgumentException("A group must have at least one permission.");

                // Validate permission IDs
                var validPermissions = await _db.Permissions
                    .Where(p => dto.PermissionIds.Contains(p.Id))
                    .Select(p => p.Id)
                    .ToListAsync();

                var invalidPermissions = dto.PermissionIds.Except(validPermissions).ToList();
                if (invalidPermissions.Any())
                    throw new ArgumentException($"These permission IDs do not exist: {string.Join(", ", invalidPermissions)}");

                group.Name = dto.Name;

                // Remove old permissions
                _db.Set<GroupPermission>().RemoveRange(
                    _db.Set<GroupPermission>().Where(gp => gp.GroupId == id)
                );

                // Add new permissions
                foreach (var permId in validPermissions)
                {
                    _db.Set<GroupPermission>().Add(new GroupPermission
                    {
                        GroupId = id,
                        PermissionId = permId
                    });
                }

                await _db.SaveChangesAsync();

                _logger.LogInformation("Group with ID {GroupId} updated successfully", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating group {GroupId}", id);
                throw;
            }
        }

        // ------------------------------------ DELETE GROUP  ------------------------------------
        public async Task<bool> DeleteGroupAsync(int id)
        {
            try
            {
                _logger.LogInformation("Deleting group with ID {GroupId}", id);

                var group = await _db.Groups.FindAsync(id);
                if (group == null)
                {
                    _logger.LogWarning("Group with ID {GroupId} not found for deletion", id);
                    return false;
                }

                // Remove associated permissions
                _db.Set<GroupPermission>().RemoveRange(
                    _db.Set<GroupPermission>().Where(gp => gp.GroupId == id)
                );

                _db.Groups.Remove(group);
                await _db.SaveChangesAsync();

                _logger.LogInformation("Group with ID {GroupId} deleted successfully", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting group {GroupId}", id);
                throw;
            }
        }

    }
}
