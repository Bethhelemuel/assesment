using Microsoft.AspNetCore.Mvc;
using UserManagement.API.DTOs;
using UserManagement.API.Interfaces;

namespace UserManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GroupsController : ControllerBase
    {
        private readonly IGroupService _groupService;
        private readonly ILogger<GroupsController> _logger;

        public GroupsController(IGroupService groupService, ILogger<GroupsController> logger)
        {
            _groupService = groupService;
            _logger = logger;
        }

        // ------------------- GET ALL GROUPS  -------------------

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GroupDTO>>> GetGroups()
        {
            _logger.LogInformation("GET /api/groups called");
            var groups = await _groupService.GetAllGroupsAsync();
            return Ok(groups);
        }

        // ------------------- GET GROUP BY ID  -------------------

        [HttpGet("{id}")]
        public async Task<ActionResult<GroupDTO>> GetGroup(int id)
        {
            _logger.LogInformation("GET /api/groups/{GroupId} called", id);
            var group = await _groupService.GetGroupByIdAsync(id);
            if (group == null) return NotFound();
            return Ok(group);
        }

        // -------------------- CREATE GROUP  --------------------

        [HttpPost]
        public async Task<ActionResult<GroupDTO>> CreateGroup(GroupCreateDTO dto)
        {
            _logger.LogInformation("POST /api/groups called with Name {GroupName}", dto.Name);
            var group = await _groupService.CreateGroupAsync(dto);
            return CreatedAtAction(nameof(GetGroup), new { id = group.Id }, group);
        }

        // -------------------- UPDATE GROUP  --------------------


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGroup(int id, GroupUpdateDTO dto)
        {
            _logger.LogInformation("PUT /api/groups/{GroupId} called", id);
            var success = await _groupService.UpdateGroupAsync(id, dto);
            if (!success) return NotFound();
            return NoContent();
        }

        // -------------------- DELETE GROUP  --------------------

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroup(int id)
        {
            _logger.LogInformation("DELETE /api/groups/{GroupId} called", id);
            var success = await _groupService.DeleteGroupAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }

      
    }
}
