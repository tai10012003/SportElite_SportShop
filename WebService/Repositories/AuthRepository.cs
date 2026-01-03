using Microsoft.EntityFrameworkCore;
using WebService.Data;
using WebService.Models;
using WebService.Interfaces.Auth;
using BCrypt.Net;

namespace WebService.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly AppDbContext _context;

        public AuthRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.TenDangNhap == username);
        }

        public async Task<User?> GetUserByMaNguoiDungAsync(string maNguoiDung)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.MaNguoiDung == maNguoiDung);
        }

        public async Task<User> CreateUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public Task<bool> ValidatePasswordAsync(User user, string password)
        {
            return Task.FromResult(BCrypt.Net.BCrypt.Verify(password, user.MatKhau));
        }
    }
}