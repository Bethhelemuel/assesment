

namespace UserManagement.API.DTOs
{
    // Returned in GET requests
    public class GroupDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public List<UserDTO> Users { get; set; } = new();
        public List<PermissionDTO> Permissions { get; set; } = new();
    }

    // POST REQUEST FOR CREATE
    public class GroupCreateDTO
    {
        public string Name { get; set; } = null!;
        public List<int> UserIds { get; set; } = new();
        public List<int> PermissionIds { get; set; } = new();
    }

    // PUT REQUEST  FOR UPDATE
    public class GroupUpdateDTO
    {
        public string Name { get; set; } = null!;
        public List<int> UserIds { get; set; } = new();
        public List<int> PermissionIds { get; set; } = new();
    }
}
