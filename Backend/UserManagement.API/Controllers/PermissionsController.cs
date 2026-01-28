using Microsoft.AspNetCore.Mvc;
using UserManagement.API.DTOs;
using UserManagement.API.Interfaces;

namespace UserManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PermissionsController : ControllerBase
    {
        private readonly IPermissionService _permissionService;
        private readonly ILogger<PermissionsController> _logger;

        public PermissionsController(IPermissionService permissionService, ILogger<PermissionsController> logger)
        {
            _permissionService = permissionService;
            _logger = logger;
        }
        // -------------------- GET ALL PERMISSIONS  --------------------

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PermissionDTO>>> GetPermissions()
        {
            _logger.LogInformation("GET /api/permissions called");
            var permissions = await _permissionService.GetAllPermissionsAsync();
            return Ok(permissions);
        }

        // ------------------- GET PERMISSION BY ID  -------------------

        [HttpGet("{id}")]
        public async Task<ActionResult<PermissionDTO>> GetPermission(int id)
        {
            _logger.LogInformation("GET /api/permissions/{PermissionId} called", id);
            var permission = await _permissionService.GetPermissionByIdAsync(id);
            if (permission == null) return NotFound();
            return Ok(permission);
        }

        // ------------------- CREATE PERMISSIONS   -------------------

        [HttpPost]
        public async Task<ActionResult<PermissionDTO>> CreatePermission(PermissionCreateDTO dto)
        {
            _logger.LogInformation("POST /api/permissions called with Name {PermissionName}", dto.Name);
            var permission = await _permissionService.CreatePermissionAsync(dto);
            return CreatedAtAction(nameof(GetPermission), new { id = permission.Id }, permission);
        }

        // ------------------- UPDATE PERMISSION   -------------------


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePermission(int id, PermissionUpdateDTO dto)
        {
            _logger.LogInformation("PUT /api/permissions/{PermissionId} called", id);
            var success = await _permissionService.UpdatePermissionAsync(id, dto);
            if (!success) return NotFound();
            return NoContent();
        }

        // ------------------- CREATE PERMISSIONS   -------------------

        [HttpDelete("{id}")] 
        public async Task<IActionResult> DeletePermission(int id)
        {
            _logger.LogInformation("DELETE /api/permissions/{PermissionId} called", id);
            var success = await _permissionService.DeletePermissionAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }

    }
}
