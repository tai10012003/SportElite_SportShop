using Microsoft.EntityFrameworkCore;
using WebService.Data;
using WebService.Interfaces.Wishlists;
using WebService.Models;

namespace WebService.Repositories
{
    public class WishlistRepository : IWishlistRepository
    {
        private readonly AppDbContext _context;

        public WishlistRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Wishlist?> GetByUserAndProductAsync(string maNguoiDung, string maSanPham)
        {
            return await _context.Wishlists.Include(w => w.SanPham).ThenInclude(p => p!.HinhAnh).FirstOrDefaultAsync(w => w.MaNguoiDung == maNguoiDung && w.MaSanPham == maSanPham);
        }

        public async Task<List<Wishlist>> GetByMaNguoiDungAsync(string maNguoiDung)
        {
            return await _context.Wishlists.Include(w => w.SanPham).ThenInclude(p => p!.HinhAnh).Where(w => w.MaNguoiDung == maNguoiDung).OrderByDescending(w => w.NgayThem).ToListAsync();
        }

        public async Task<bool> ExistsAsync(string maNguoiDung, string maSanPham)
        {
            return await _context.Wishlists.AnyAsync(w => w.MaNguoiDung == maNguoiDung && w.MaSanPham == maSanPham);
        }

        public async Task AddAsync(Wishlist wishlist)
        {
            await _context.Wishlists.AddAsync(wishlist);
        }

        public async Task RemoveAsync(Wishlist wishlist)
        {
            _context.Wishlists.Remove(wishlist);
            await Task.CompletedTask;
        }

        public async Task<int> CountByMaNguoiDungAsync(string maNguoiDung)
        {
            return await _context.Wishlists.CountAsync(w => w.MaNguoiDung == maNguoiDung);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}