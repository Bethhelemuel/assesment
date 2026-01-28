namespace UserManagement.API.DTOs
{
    // GET REQUEST FOR ALL PERMISSIONS
    public class PermissionDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public List<GroupDTO> Groups { get; set; } = new();
    }

    // POST REQUEST FOR CREATE
    public class PermissionCreateDTO
    {
        public string Name { get; set; } = null!;
        public List<int> GroupIds { get; set; } = new();
    }

    // PUT REQUEST FOR UPDATE 
    public class PermissionUpdateDTO
    {
        public string Name { get; set; } = null!;
        public List<int> GroupIds { get; set; } = new();
    }
}
