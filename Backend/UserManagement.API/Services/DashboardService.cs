using Microsoft.EntityFrameworkCore;
using UserManagement.API.Data;
using UserManagement.API.DTOs;
using UserManagement.API.Interfaces;

namespace UserManagement.API.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly AppDbContext _db;

        public DashboardService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<DashboardDTO> GetDashboardAsync()
        {
            var totalUsers = await _db.Users.CountAsync();
            var totalGroups = await _db.Groups.CountAsync();
            var totalPermissions = await _db.Permissions.CountAsync();

            var mostAssignedGroup = await _db.UserGroups
                .Include(ug => ug.Group)
                .GroupBy(ug => ug.Group.Name)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefaultAsync() ?? "N/A";

            var mostCommonPermission = await _db.GroupPermissions
                .Include(gp => gp.Permission)
                .GroupBy(gp => gp.Permission.Name)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefaultAsync() ?? "N/A";

            return new DashboardDTO
            {
                TotalUsers = totalUsers,
                TotalGroups = totalGroups,
                TotalPermissions = totalPermissions,
                MostAssignedGroup = mostAssignedGroup,
                MostCommonPermission = mostCommonPermission
            };
        }
    }
}
