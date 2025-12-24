using System.ComponentModel.DataAnnotations;

namespace WebService.DTOs.Auth
{
    public class RegisterDTO
    {
        [Required, StringLength(30, MinimumLength = 3)]
        public string TenDangNhap { get; set; } = string.Empty;

        [Required, StringLength(40)]
        public string HoTen { get; set; } = string.Empty;

        [Required, EmailAddress, StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required, StringLength(100, MinimumLength = 6)]
        public string MatKhau { get; set; } = string.Empty;

        [Phone, StringLength(20)]
        public string? SoDienThoai { get; set; }

        [StringLength(200)]
        public string? DiaChi { get; set; }
    }
}