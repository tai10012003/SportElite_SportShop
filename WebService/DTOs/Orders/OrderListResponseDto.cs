using WebService.Enums;

namespace WebService.DTOs.Orders
{
    public class OrderListResponseDto
    {
        public int Id { get; set; }
        public string MaDonHang { get; set; } = string.Empty;
        public string TenNguoiNhan { get; set; } = string.Empty;
        public decimal TongThanhToan { get; set; }
        public string PhuongThucThanhToan { get; set; } = string.Empty;
        public OrderStatus TrangThai { get; set; }
        public bool DaThanhToan { get; set; }
        public DateTime NgayTao { get; set; }
        public int SoLuongSanPham { get; set; }
    }
}