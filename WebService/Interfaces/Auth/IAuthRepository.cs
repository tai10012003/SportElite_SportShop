using WebService.DTOs.Auth;
using WebService.Models;

namespace WebService.Interfaces.Auth
{
    public interface IAuthRepository
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByUsernameAsync(string username);
        Task<User?> GetUserByMaNguoiDungAsync(string maNguoiDung);
        Task<User> CreateUserAsync(User user);
        Task<bool> ValidatePasswordAsync(User user, string password);
    }
}