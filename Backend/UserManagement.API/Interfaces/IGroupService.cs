using UserManagement.API.DTOs;

namespace UserManagement.API.Interfaces
{
    public interface IGroupService
    {
        Task<GroupDTO> CreateGroupAsync(GroupCreateDTO dto);
        Task<GroupDTO> GetGroupByIdAsync(int id);
        Task<IEnumerable<GroupDTO>> GetAllGroupsAsync();
        Task<bool> UpdateGroupAsync(int id, GroupUpdateDTO dto);
        Task<bool> DeleteGroupAsync(int id);
    }
}
