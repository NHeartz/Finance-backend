using Finance.Api.DTOs;
using Finance.Api.Models;

namespace Finance.Api.Services;

public interface IUserService
{
    Task<User?> CreateUserAsync(UserCreateDto userDto);
    Task<User?> CreateUserWithRoleAsync(AdminUserCreateDto userDto);
    Task<bool> ChangePasswordAsync(int userId, string oldPassword, string newPassword);
    Task<bool> AdminResetPasswordAsync(int userId, string newPassword);
    Task<string?> LoginAsync(LoginRequestDto loginDto);
    Task<User?> GetUserByIdAsync(int userId);
    Task<bool> UpdateDisplayNameAsync(int userId, string displayName);
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<bool> UpdateUserRoleAsync(int userId, string role);
    Task<bool> DeleteUserAsync(int userId);
}