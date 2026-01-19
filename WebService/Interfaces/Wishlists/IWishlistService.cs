using WebService.DTOs.Wishlists;

namespace WebService.Interfaces.Wishlists
{
    public interface IWishlistService
    {
        Task<WishlistActionResultDto> AddToWishlistAsync(string maNguoiDung, AddToWishlistDto dto);
        Task<WishlistActionResultDto> RemoveFromWishlistAsync(string maNguoiDung, string maSanPham);
        Task<WishlistActionResultDto> ToggleWishlistAsync(string maNguoiDung, string maSanPham);
        Task<WishlistResponseDto> GetWishlistAsync(string maNguoiDung);
        Task<CheckWishlistDto> CheckProductInWishlistAsync(string maNguoiDung, string maSanPham);
        Task<List<CheckWishlistDto>> CheckMultipleProductsAsync(string maNguoiDung, List<string> maSanPhams);
    }
}