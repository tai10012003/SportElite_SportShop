using WebService.Enums;

namespace WebService.DTOs.Orders
{
    public class OrderResponseDto
    {
        public int Id { get; set; }
        public string MaDonHang { get; set; } = string.Empty;
        public string TenNguoiNhan { get; set; } = string.Empty;
        public string SoDienThoai { get; set; } = string.Empty;
        public string DiaChi { get; set; } = string.Empty;
        public string? GhiChu { get; set; }
        public decimal TamTinh { get; set; }
        public string? MaGiamGia { get; set; }
        public decimal SoTienGiam { get; set; }
        public decimal PhiVanChuyen { get; set; }
        public decimal TongThanhToan { get; set; }
        public string PhuongThucThanhToan { get; set; } = string.Empty;
        public string? MaGiaoDich { get; set; }
        public string? CongThanhToan { get; set; }
        public bool DaThanhToan { get; set; }
        public OrderStatus TrangThai { get; set; }
        public DateTime NgayTao { get; set; }
        public DateTime NgayCapNhat { get; set; }
        public List<OrderItemResponseDto> Items { get; set; } = new List<OrderItemResponseDto>();
    }

    public class OrderItemResponseDto
    {
        public int Id { get; set; }
        public string MaSanPham { get; set; } = string.Empty;
        public string TenSanPham { get; set; } = string.Empty;
        public string? HinhAnh { get; set; }
        public int SoLuong { get; set; }
        public string? MauSac { get; set; }
        public string? KichThuoc { get; set; }
        public decimal DonGia { get; set; }
        public decimal ThanhTien { get; set; }
    }
}