using UserManagement.API.DTOs;

namespace UserManagement.API.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardDTO> GetDashboardAsync();
    }
}
