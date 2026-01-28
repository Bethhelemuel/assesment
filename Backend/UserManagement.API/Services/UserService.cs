using Microsoft.EntityFrameworkCore;
using UserManagement.API.Data;
using UserManagement.API.DTOs;
using UserManagement.API.Interfaces;
using UserManagement.API.Models;

namespace UserManagement.API.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _db;
        private readonly ILogger<UserService> _logger;

        public UserService(AppDbContext db, ILogger<UserService> logger)
        {
            _db = db;
            _logger = logger;
        }

        // ------------------------------------ CREATE USER  ------------------------------------
        public async Task<UserDTO> CreateUserAsync(UserCreateDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.FullName))
                throw new ArgumentException("Full name cannot be empty.");
            if (string.IsNullOrWhiteSpace(dto.Email))
                throw new ArgumentException("Email cannot be empty.");

            try
            {
                _logger.LogInformation("Creating user with email {Email}", dto.Email);

                // Ensure groups exist
                var validGroupIds = await _db.Groups
                    .Where(g => dto.GroupIds.Contains(g.Id))
                    .Select(g => g.Id)
                    .ToListAsync();

                var invalidGroupIds = dto.GroupIds.Except(validGroupIds).ToList();
                if (invalidGroupIds.Any())
                    throw new ArgumentException($"These group IDs do not exist: {string.Join(", ", invalidGroupIds)}");

                var user = new User
                {
                    FullName = dto.FullName,
                    Email = dto.Email
                };

                _db.Users.Add(user);
                await _db.SaveChangesAsync();

                // Assign Groups
                foreach (var groupId in validGroupIds)
                {
                    _db.Set<UserGroup>().Add(new UserGroup
                    {
                        UserId = user.Id,
                        GroupId = groupId
                    });
                }

                await _db.SaveChangesAsync();

                _logger.LogInformation("User created with ID {UserId}", user.Id);

                return new UserDTO
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    Groups = validGroupIds.Select(id => new GroupDTO
                    {
                        Id = id,
                        Name = _db.Groups.Find(id)?.Name ?? ""
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating user with email {Email}", dto.Email);
                throw;
            }
        }

        // ------------------------------------ GETUSER BY ID   ------------------------------------
        public async Task<UserDTO> GetUserByIdAsync(int id)
        {
            _logger.LogInformation("Fetching user with ID {UserId}", id);

            var user = await _db.Users
                .Include(u => u.UserGroups)
                    .ThenInclude(ug => ug.Group)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                _logger.LogWarning("User with ID {UserId} not found", id);
                return null!;
            }

            return new UserDTO
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Groups = user.UserGroups.Select(ug => new GroupDTO
                {
                    Id = ug.Group.Id,
                    Name = ug.Group.Name
                }).ToList()
            };
        }

        // ------------------------------------ GET ALL USERS  ------------------------------------
        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            _logger.LogInformation("Fetching all users");

            var users = await _db.Users
                .Include(u => u.UserGroups)
                    .ThenInclude(ug => ug.Group)
                .ToListAsync();

            return users.Select(u => new UserDTO
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email,
                Groups = u.UserGroups.Select(ug => new GroupDTO
                {
                    Id = ug.Group.Id,
                    Name = ug.Group.Name
                }).ToList()
            });
        }

        // ------------------------------------ UPDATE  USERS  ------------------------------------
        public async Task<bool> UpdateUserAsync(int id, UserUpdateDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.FullName))
                throw new ArgumentException("Full name cannot be empty.");
            if (string.IsNullOrWhiteSpace(dto.Email))
                throw new ArgumentException("Email cannot be empty.");

            try
            {
                _logger.LogInformation("Updating user with ID {UserId}", id);

                var user = await _db.Users.FindAsync(id);
                if (user == null)
                {
                    _logger.LogWarning("User with ID {UserId} not found for update", id);
                    return false;
                }

                // Ensure groups exist
                var validGroupIds = await _db.Groups
                    .Where(g => dto.GroupIds.Contains(g.Id))
                    .Select(g => g.Id)
                    .ToListAsync();

                var invalidGroupIds = dto.GroupIds.Except(validGroupIds).ToList();
                if (invalidGroupIds.Any())
                    throw new ArgumentException($"These group IDs do not exist: {string.Join(", ", invalidGroupIds)}");

                user.FullName = dto.FullName;
                user.Email = dto.Email;

                // Remove old group relations
                _db.Set<UserGroup>().RemoveRange(_db.Set<UserGroup>().Where(ug => ug.UserId == id));

                // Add new groups
                foreach (var groupId in validGroupIds)
                {
                    _db.Set<UserGroup>().Add(new UserGroup
                    {
                        UserId = id,
                        GroupId = groupId
                    });
                }

                await _db.SaveChangesAsync();

                _logger.LogInformation("User with ID {UserId} updated successfully", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating user with ID {UserId}", id);
                throw;
            }
        }

        // ------------------------------------ DELETE USERS  ------------------------------------
        public async Task<bool> DeleteUserAsync(int id)
        {
            try
            {
                _logger.LogInformation("Deleting user with ID {UserId}", id);

                var user = await _db.Users.FindAsync(id);
                if (user == null)
                {
                    _logger.LogWarning("User with ID {UserId} not found for deletion", id);
                    return false;
                }

                // Remove group relations first
                _db.Set<UserGroup>().RemoveRange(_db.Set<UserGroup>().Where(ug => ug.UserId == id));

                _db.Users.Remove(user);
                await _db.SaveChangesAsync();

                _logger.LogInformation("User with ID {UserId} deleted successfully", id); 
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting user with ID {UserId}", id);
                throw;
            }
        }
    }
}
