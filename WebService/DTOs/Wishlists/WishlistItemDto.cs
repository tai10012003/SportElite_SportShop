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
        public string? MaDanhMuc { get; set; }
        public string? MaThuongHieu { get; set; }
        public DateTime NgayThem { get; set; }
        public bool ConHang { get; set; }
        public string TenDanhMuc { get; set; } = string.Empty;
        public string TenThuongHieu { get; set; } = string.Empty;
        public double AverageRating { get; set; } = 0;
        public int TotalReviews { get; set; } = 0;
        public string? KichThuoc { get; set; }
        public string? MauSac { get; set; }
        public string Slug { get; set; } = string.Empty;
        public List<string> HinhAnhList { get; set; } = new();
    }
}