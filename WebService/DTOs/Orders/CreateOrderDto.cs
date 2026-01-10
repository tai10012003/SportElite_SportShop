using System.ComponentModel.DataAnnotations;

namespace WebService.DTOs.Orders
{
    public class CreateOrderDto
    {
        [Required(ErrorMessage = "Tên người nhận là bắt buộc")]
        [StringLength(100)]
        public string TenNguoiNhan { get; set; } = string.Empty;

        [Required(ErrorMessage = "Số điện thoại là bắt buộc")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [StringLength(20)]
        public string SoDienThoai { get; set; } = string.Empty;

        [Required(ErrorMessage = "Địa chỉ là bắt buộc")]
        [StringLength(255)]
        public string DiaChi { get; set; } = string.Empty;

        [StringLength(500)]
        public string? GhiChu { get; set; }

        [Required(ErrorMessage = "Phương thức thanh toán là bắt buộc")]
        [StringLength(50)]
        public string PhuongThucThanhToan { get; set; } = "cod";

        public string? MaGiamGia { get; set; }

        [Required]
        public List<CreateOrderItemDto> Items { get; set; } = new List<CreateOrderItemDto>();
    }

    public class CreateOrderItemDto
    {
        [Required]
        public string MaSanPham { get; set; } = string.Empty;

        [Required]
        public string TenSanPham { get; set; } = string.Empty;

        public string? HinhAnh { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
        public int SoLuong { get; set; }

        public string? MauSac { get; set; }

        public string? KichThuoc { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Đơn giá phải lớn hơn hoặc bằng 0")]
        public decimal DonGia { get; set; }
    }
}