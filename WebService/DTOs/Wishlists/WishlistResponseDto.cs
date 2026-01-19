namespace WebService.DTOs.Wishlists
{
    public class WishlistResponseDto
    {
        public int TongSanPham { get; set; }
        public List<WishlistItemDto> SanPham { get; set; } = new();
    }
}