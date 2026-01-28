namespace UserManagement.API.DTOs
{
    // BASIC DASHBOARD DISPLAY
    public class DashboardDTO
    {
        public int TotalUsers { get; set; }
        public int TotalGroups { get; set; }
        public int TotalPermissions { get; set; }

        public string MostAssignedGroup { get; set; } = string.Empty;
        public string MostCommonPermission { get; set; } = string.Empty;
    }
}