using Microsoft.AspNetCore.Mvc;
using UserManagement.API.DTOs;
using UserManagement.API.Interfaces;

namespace UserManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    { 
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger; 

        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }
        // ------------------- GET ALL USERS   -------------------

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            _logger.LogInformation("GET /api/users called");

            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        // ------------------- GET USER BY ID  -------------------


        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUser(int id)
        {
            _logger.LogInformation("GET /api/users/{UserId} called", id);

            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning("User with ID {UserId} not found", id);
                return NotFound();
            }

            return Ok(user);
        }

        // -------------------- CREATE USER  -------------------

        [HttpPost]
        public async Task<ActionResult<UserDTO>> CreateUser(UserCreateDTO dto)
        {
            try
            {
                var user = await _userService.CreateUserAsync(dto);
                return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // fallback for unexpected errors
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }

        // ------------------- UPDATE USER   -------------------


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserUpdateDTO dto)
        {
            try
            {
                var success = await _userService.UpdateUserAsync(id, dto);
                if (!success) return NotFound(new { message = $"User with ID {id} not found." });
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }

        // ------------------- DELETE USER   -------------------

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            _logger.LogInformation("DELETE /api/users/{UserId} called", id);

            var success = await _userService.DeleteUserAsync(id);
            if (!success)
            {
                _logger.LogWarning("Failed to delete user with ID {UserId}", id);
                return NotFound();
            }

            return NoContent();
        }

 
    }
}
