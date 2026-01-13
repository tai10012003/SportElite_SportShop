using WebService.Models;

namespace WebService.Interfaces.Users
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        Task<User?> GetByMaNguoiDungAsync(string maNguoiDung);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByUsernameAsync(string username);
        Task<User> CreateAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> EmailExistsAsync(string email, int? excludeUserId = null);
        Task<bool> UsernameExistsAsync(string username, int? excludeUserId = null);
    }
}