using Microsoft.EntityFrameworkCore;
using UserManagement.API.Data;
using UserManagement.API.DTOs;
using UserManagement.API.Interfaces;
using UserManagement.API.Models;

namespace UserManagement.API.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly AppDbContext _db;
        private readonly ILogger<PermissionService> _logger;

        public PermissionService(AppDbContext db, ILogger<PermissionService> logger)
        {
            _db = db;
            _logger = logger;
        }

        // ------------------------------------ CREATE PERMISSION  ------------------------------------
        public async Task<PermissionDTO> CreatePermissionAsync(PermissionCreateDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Permission name cannot be empty.");

            try
            {
                _logger.LogInformation("Creating permission {PermissionName}", dto.Name);

                var permission = new Permission { Name = dto.Name };
                _db.Permissions.Add(permission);
                await _db.SaveChangesAsync();

                _logger.LogInformation("Permission created with ID {PermissionId}", permission.Id);

                return new PermissionDTO
                {
                    Id = permission.Id,
                    Name = permission.Name
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating permission {PermissionName}", dto.Name);
                throw;
            }
        }

        // ------------------------------------ DELETE PERMISSION  ------------------------------------
        public async Task<bool> DeletePermissionAsync(int id)
        {
            try
            {
                _logger.LogInformation("Deleting permission with ID {PermissionId}", id);

                var permission = await _db.Permissions.FindAsync(id);
                if (permission == null)
                {
                    _logger.LogWarning("Permission with ID {PermissionId} not found for deletion", id);
                    return false;
                }

                // Remove this permission from all groups
                _db.Set<GroupPermission>().RemoveRange(
                    _db.Set<GroupPermission>().Where(gp => gp.PermissionId == id)
                );

                _db.Permissions.Remove(permission);
                await _db.SaveChangesAsync();

                _logger.LogInformation("Permission with ID {PermissionId} deleted successfully", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting permission {PermissionId}", id);
                throw;
            }
        }

        // ------------------------------------ GET ALL PERMISSIONS  ------------------------------------
        public async Task<IEnumerable<PermissionDTO>> GetAllPermissionsAsync()
        {
            _logger.LogInformation("Fetching all permissions");

            var permissions = await _db.Permissions.ToListAsync();
            return permissions.Select(p => new PermissionDTO
            {
                Id = p.Id,
                Name = p.Name
            });
        }

        // ------------------------------------ GET PERMISSION BY ID  ------------------------------------
        public async Task<PermissionDTO> GetPermissionByIdAsync(int id)
        {
            _logger.LogInformation("Fetching permission with ID {PermissionId}", id);

            var permission = await _db.Permissions.FindAsync(id);
            if (permission == null)
            {
                _logger.LogWarning("Permission with ID {PermissionId} not found", id);
                return null!;
            }

            return new PermissionDTO
            {
                Id = permission.Id,
                Name = permission.Name
            };
        }

        // ------------------------------------ UPDATE  PERMISSION  ------------------------------------
        public async Task<bool> UpdatePermissionAsync(int id, PermissionUpdateDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Permission name cannot be empty.");

            try
            {
                _logger.LogInformation("Updating permission with ID {PermissionId}", id);

                var permission = await _db.Permissions.FindAsync(id);
                if (permission == null)
                {
                    _logger.LogWarning("Permission with ID {PermissionId} not found for update", id);
                    return false;
                }

                permission.Name = dto.Name;
                await _db.SaveChangesAsync();

                _logger.LogInformation("Permission with ID {PermissionId} updated successfully", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating permission {PermissionId}", id);
                throw;
            }
        }

    }
}
