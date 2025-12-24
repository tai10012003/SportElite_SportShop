using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebService.Models
{
    [Table("Users")]
    public class User
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("ma_nguoi_dung", TypeName = "varchar(8)")]
        public string MaNguoiDung { get; set; } = string.Empty;

        [Required]
        [Column("ten_dang_nhap", TypeName = "varchar(30)")]
        public string TenDangNhap { get; set; } = string.Empty;

        [Column("mat_khau", TypeName = "varchar(255)")]
        public string MatKhau { get; set; } = string.Empty;

        [Column("email", TypeName = "varchar(100)")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Column("ho_ten", TypeName = "nvarchar(40)")]
        public string HoTen { get; set; } = string.Empty;

        [Column("so_dien_thoai", TypeName = "varchar(20)")]
        public string? SoDienThoai { get; set; }

        [Column("dia_chi", TypeName = "nvarchar(200)")]
        public string? DiaChi { get; set; }

        [Required]
        [Column("quyen", TypeName = "varchar(10)")]
        public string Quyen { get; set; } = "user";

        [Column("trang_thai")]
        public bool TrangThai { get; set; } = true;

        [Column("ngay_tao")]
        public DateTime NgayTao { get; set; } = DateTime.Now;

        [Column("ngay_cap_nhat")]
        public DateTime NgayCapNhat { get; set; } = DateTime.Now;
    }
}