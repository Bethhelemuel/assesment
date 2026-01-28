
using UserManagement.API.DTOs;

namespace UserManagement.API.Interfaces
{
    public interface IUserService
    {
        Task<UserDTO> CreateUserAsync(UserCreateDTO dto);
        Task<UserDTO> GetUserByIdAsync(int id);
        Task<IEnumerable<UserDTO>> GetAllUsersAsync();
        Task<bool> UpdateUserAsync(int id, UserUpdateDTO dto);
        Task<bool> DeleteUserAsync(int id);
    }
}
