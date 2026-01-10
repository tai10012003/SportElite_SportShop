using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebService.Models
{
    [Table("OrderDetails")]
    public class OrderDetail
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("ma_don_hang", TypeName = "varchar(20)")]
        public string MaDonHang { get; set; } = string.Empty;

        [Column("ma_san_pham")]
        [StringLength(8)]
        public string MaSanPham { get; set; } = string.Empty;

        [Required]
        [Column("ten_san_pham", TypeName = "nvarchar(150)")]
        public string TenSanPham { get; set; } = string.Empty;

        [Column("hinh_anh", TypeName = "varchar(255)")]
        public string? HinhAnh { get; set; }

        [Required]
        [Column("so_luong")]
        public int SoLuong { get; set; }

        [Column("mau_sac", TypeName = "nvarchar(50)")]
        public string? MauSac { get; set; }

        [Column("kich_thuoc", TypeName = "nvarchar(50)")]
        public string? KichThuoc { get; set; }

        [Required]
        [Column("don_gia", TypeName = "decimal(15,2)")]
        public decimal DonGia { get; set; }

        [Required]
        [Column("thanh_tien", TypeName = "decimal(15,2)")]
        public decimal ThanhTien { get; set; }

        [Column("ngay_tao")]
        public DateTime NgayTao { get; set; } = DateTime.Now;

        [ForeignKey("MaDonHang")]
        public virtual Order? DonHang { get; set; }

        [ForeignKey("MaSanPham")]
        public virtual Product? SanPham { get; set; }
    }
}