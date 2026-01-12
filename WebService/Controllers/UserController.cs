using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebService.DTOs.Users;
using WebService.Interfaces.Users;

namespace WebService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<IEnumerable<GetUserDTO>>> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<GetUserDTO>> GetById(int id)
        {
            try
            {
                var user = await _userService.GetByIdAsync(id);
                return Ok(user);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<ActionResult<GetUserDTO>> GetProfile()
        {
            try
            {
                var userIdStr = User.FindFirstValue("uid");
                if (string.IsNullOrEmpty(userIdStr))
                    return Unauthorized(new { message = "Token không chứa thông tin người dùng" });
                if (!int.TryParse(userIdStr, out var userId))
                    return Unauthorized(new { message = "user id trong token không hợp lệ" });
                var profile = await _userService.GetProfileAsync(userId);
                return Ok(profile);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("profile")]
        [Authorize]
        public async Task<ActionResult<UserResponseDTO>> UpdateProfile([FromBody] UpdateUserDTO updateDto)
        {
            try
            {
                var userIdStr = User.FindFirstValue("uid");
                if (string.IsNullOrEmpty(userIdStr))
                    return Unauthorized(new { message = "Token không chứa thông tin người dùng" });
                if (!int.TryParse(userIdStr, out var userId))
                    return Unauthorized(new { message = "User id trong token không hợp lệ" });
                var result = await _userService.UpdateProfileAsync(userId, new UpdateUserDTO
                {
                    HoTen = updateDto.HoTen,
                    Email = updateDto.Email,
                    SoDienThoai = updateDto.SoDienThoai,
                    DiaChi = updateDto.DiaChi
                });
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<UserResponseDTO>> Create([FromBody] CreateUserDTO createDto)
        {
            try
            {
                var result = await _userService.CreateAsync(createDto);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<UserResponseDTO>> Update(int id, [FromBody] UpdateUserDTO updateDto)
        {
            try
            {
                var result = await _userService.UpdateAsync(id, updateDto);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<UserResponseDTO>> Delete(int id)
        {
            try
            {
                var result = await _userService.DeleteAsync(id);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}