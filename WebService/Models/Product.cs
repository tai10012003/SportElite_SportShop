using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebService.Models
{
    [Table("Products")]
    public class Product
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("ma_san_pham")]
        [StringLength(8)]
        public string MaSanPham { get; set; } = string.Empty;

        [Required]
        [Column("ten_san_pham", TypeName = "nvarchar(100)")]
        public string TenSanPham { get; set; } = string.Empty;

        [Column("mo_ta", TypeName = "nvarchar(150)")]
        public string? MoTa { get; set; }

        [Column("mo_ta_chi_tiet", TypeName = "nvarchar(max)")]
        public string? MoTaChiTiet { get; set; }

        [Column("kich_thuoc", TypeName = "nvarchar(100)")]
        public string? KichThuoc { get; set; }

        [Column("mau_sac", TypeName = "nvarchar(100)")]
        public string? MauSac { get; set; }

        [Column("chat_lieu", TypeName = "nvarchar(100)")]
        public string? ChatLieu { get; set; }

        [Column("xuat_xu", TypeName = "nvarchar(30)")]
        public string? XuatXu { get; set; }

        [Column("bao_hanh", TypeName = "nvarchar(20)")]
        public string? BaoHanh { get; set; }

        [Required]
        [Column("gia", TypeName = "decimal(15,2)")]
        public decimal Gia { get; set; }

        [Column("gia_khuyen_mai", TypeName = "decimal(15,2)")]
        public decimal? GiaKhuyenMai { get; set; }

        [Column("so_luong")]
        public int SoLuong { get; set; } = 0;

        [Column("ma_danh_muc", TypeName = "varchar(8)")]
        public string MaDanhMuc { get; set; } = string.Empty;

        [Column("ma_thuong_hieu", TypeName = "varchar(8)")]
        public string MaThuongHieu { get; set; } = string.Empty;

        [Column("tinh_trang")]
        public bool TinhTrang { get; set; } = true;

        [Column("noi_bat")]
        public bool NoiBat { get; set; } = true;

        [Column("luot_xem")]
        public int LuotXem { get; set; } = 0;

        [Column("slug", TypeName = "varchar(255)")]
        public string Slug { get; set; } = string.Empty;

        [Column("ngay_tao")]
        public DateTime NgayTao { get; set; } = DateTime.Now;

        [Column("ngay_cap_nhat")]
        public DateTime NgayCapNhat { get; set; } = DateTime.Now;

        public virtual ICollection<ProductImage> HinhAnh { get; set; } = new List<ProductImage>();
    }
}
