using WebService.Models;

namespace WebService.Interfaces.Wishlists
{
    public interface IWishlistRepository
    {
        Task<Wishlist?> GetByUserAndProductAsync(string maNguoiDung, string maSanPham);
        Task<List<Wishlist>> GetByMaNguoiDungAsync(string maNguoiDung);
        Task<bool> ExistsAsync(string maNguoiDung, string maSanPham);
        Task AddAsync(Wishlist wishlist);
        Task RemoveAsync(Wishlist wishlist);
        Task<int> CountByMaNguoiDungAsync(string maNguoiDung);
        Task SaveChangesAsync();
    }
}