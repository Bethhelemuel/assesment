
namespace UserManagement.API.DTOs
{
    // GET REQUEST FOR ALL USERS
    public class UserDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public List<GroupDTO> Groups { get; set; } = new();
    }

    // POST REQUEST FOR CREATE
    public class UserCreateDTO
    {
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public List<int> GroupIds { get; set; } = new(); // assign groups by ID
    }

    // PUT REQUEST FOR UPDATE
    public class UserUpdateDTO
    {
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public List<int> GroupIds { get; set; } = new();
    }
}
