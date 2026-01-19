namespace WebService.DTOs.Wishlists
{
    public class WishlistItemDto
    {
        public int Id { get; set; }
        public string MaSanPham { get; set; } = string.Empty;
        public string TenSanPham {get; set; } = string.Empty;
        public string? HinhAnh { get; set; }
        public decimal? Gia {get; set; }
        public decimal? GiaKhuyenMai { get; set; }
        public int SoLuong { get; set; }
        public string? MaThuongHieu { get; set; }
        public string? TenThuongHieu { get; set; }
        public DateTime NgayThem { get; set; }
        public bool ConHang { get; set; }
    }
}