using UserManagement.API.DTOs;

namespace UserManagement.API.Interfaces
{
    public interface IPermissionService
    {
        Task<PermissionDTO> CreatePermissionAsync(PermissionCreateDTO dto);
        Task<PermissionDTO> GetPermissionByIdAsync(int id);
        Task<IEnumerable<PermissionDTO>> GetAllPermissionsAsync();
        Task<bool> UpdatePermissionAsync(int id, PermissionUpdateDTO dto);
        Task<bool> DeletePermissionAsync(int id);
    }
}
