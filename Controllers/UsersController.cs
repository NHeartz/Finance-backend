using Finance.Api.DTOs;
using Finance.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Finance.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser(UserCreateDto userDto)
    {
        var newUser = await _userService.CreateUserAsync(userDto);

        if (newUser == null)
        {
            return BadRequest("Username already exists.");
        }

        // ไม่ส่ง PasswordHash กลับไปหา Client เพื่อความปลอดภัย
        return CreatedAtAction(nameof(CreateUser), new { id = newUser.Id }, new { newUser.Id, newUser.Username });
    }

    [Authorize]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] UserChangePasswordDto changePasswordDto)
    {
        // ดึง UserId จาก Token ที่แนบมากับ Request
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            return Unauthorized();

        var result = await _userService.ChangePasswordAsync(userId, changePasswordDto.OldPassword, changePasswordDto.NewPassword);

        if (!result)
        {
            return BadRequest("Could not change password. Please check user ID and old password.");
        }

        return Ok("Password changed successfully.");
    }

    // Endpoint สำหรับให้ Admin เปลี่ยนรหัสผ่านให้ User คนอื่น
    [Authorize(Roles = "Admin")]
    [HttpPost("admin-reset-password")]
    public async Task<IActionResult> AdminResetPassword([FromBody] AdminResetPasswordDto resetPasswordDto)
    {
        var result = await _userService.AdminResetPasswordAsync(resetPasswordDto.UserId, resetPasswordDto.NewPassword);

        if (!result)
        {
            return BadRequest("Could not reset password. User not found.");
        }

        return Ok("Password for user has been reset successfully.");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequestDto loginDto)
    {
        var token = await _userService.LoginAsync(loginDto);

        if (token == null)
        {
            return Unauthorized("Invalid username or password.");
        }

        return Ok(new { Token = token });
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetProfile()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            return Unauthorized();

        var user = await _userService.GetUserByIdAsync(userId);
        if (user == null) return NotFound();

        return Ok(new { user.Id, user.Username, user.DisplayName, user.Role });
    }

    [Authorize]
    [HttpPut("me/name")]
    public async Task<IActionResult> UpdateName([FromBody] UserUpdateNameDto dto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            return Unauthorized();

        var result = await _userService.UpdateDisplayNameAsync(userId, dto.DisplayName);
        if (!result) return BadRequest("User not found.");

        return Ok();
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("admin-create")]
    public async Task<IActionResult> AdminCreateUser([FromBody] AdminUserCreateDto userDto)
    {
        var newUser = await _userService.CreateUserWithRoleAsync(userDto);
        if (newUser == null) return BadRequest("Username already exists.");
        
        return Ok(new { newUser.Id, newUser.Username, newUser.Role });
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        // คืนค่าไปเฉพาะข้อมูลที่ไม่ใช่ความลับ
        return Ok(users.Select(u => new { u.Id, u.Username, u.DisplayName, u.Role }));
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}/role")]
    public async Task<IActionResult> UpdateRole(int id, [FromBody] UserUpdateRoleDto dto)
    {
        var result = await _userService.UpdateUserRoleAsync(id, dto.Role);
        if (!result) return NotFound("User not found.");
        return Ok();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var result = await _userService.DeleteUserAsync(id);
        if (!result) return NotFound("User not found.");
        return NoContent();
    }
}