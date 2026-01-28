using Microsoft.AspNetCore.Mvc;
using UserManagement.API.DTOs;
using UserManagement.API.Interfaces;

namespace UserManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;


        // ------------------- GET DASHBORD INFO -------------------

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }
         
        [HttpGet]
        public async Task<ActionResult<DashboardDTO>> GetDashboard()
        {
            var dashboard = await _dashboardService.GetDashboardAsync();
            return Ok(dashboard);
        }
    }
}
