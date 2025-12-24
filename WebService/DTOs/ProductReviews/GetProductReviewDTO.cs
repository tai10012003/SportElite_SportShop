namespace WebService.DTOs.ProductReviews
{
    public class GetProductReviewDTO
    {
        public int Id { get; set; }
        public string MaSanPham { get; set; } = string.Empty;
        public string MaNguoiDung { get; set; } = string.Empty;
        public string HoTen { get; set; } = string.Empty;
        public int DiemDanhGia { get; set; }
        public string? NoiDung { get; set; }
        public DateTime NgayTao { get; set; }
        public DateTime NgayCapNhat { get; set; }
    }
}
