using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebService.Enums;

namespace WebService.Models
{
    [Table("Orders")]
    public class Order
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("ma_don_hang", TypeName = "varchar(20)")]
        public string MaDonHang { get; set; } = string.Empty;

        [Required]
        [Column("ma_nguoi_dung", TypeName = "varchar(8)")]
        public string MaNguoiDung { get; set; } = string.Empty;

        [Required]
        [Column("ten_nguoi_nhan", TypeName = "nvarchar(100)")]
        public string TenNguoiNhan { get; set; } = string.Empty;

        [Required]
        [Column("so_dien_thoai", TypeName = "varchar(20)")]
        public string SoDienThoai { get; set; } = string.Empty;

        [Required]
        [Column("dia_chi", TypeName = "nvarchar(255)")]
        public string DiaChi { get; set; } = string.Empty;

        [Column("ghi_chu", TypeName = "nvarchar(500)")]
        public string GhiChu { get; set; } = string.Empty;

        [Required]
        [Column("tam_tinh", TypeName = "decimal(15,2)")]
        public decimal TamTinh { get; set; }

        [Column("ma_giam_gia", TypeName = "varchar(50)")]
        public string? MaGiamGia { get; set; }

        [Column("so_tien_giam", TypeName = "decimal(15,2)")]
        public decimal SoTienGiam { get; set; } = 0;

        [Required]
        [Column("phi_van_chuyen", TypeName = "decimal(15,2)")]
        public decimal PhiVanChuyen { get; set; }

        [Required]
        [Column("tong_thanh_toan", TypeName = "decimal(15,2)")]
        public decimal TongThanhToan { get; set; }

        [Required]
        [Column("phuong_thuc_thanh_toan", TypeName = "varchar(50)")]
        public string PhuongThucThanhToan { get; set; } = "cod";

        [Column("ma_giao_dich", TypeName = "varchar(100)")]
        public string? MaGiaoDich { get; set; }

        [Column("cong_thanh_toan", TypeName = "varchar(50)")]
        public string? CongThanhToan { get; set; }

        [Column("da_thanh_toan")]
        public bool DaThanhToan { get; set; } = false;

        [Required]
        [Column("trang_thai")]
        public OrderStatus TrangThai { get; set; } = OrderStatus.Pending;

        [Column("ngay_tao")]
        public DateTime NgayTao { get; set; } = DateTime.Now;

        [Column("ngay_cap_nhat")]
        public DateTime NgayCapNhat { get; set; } = DateTime.Now;

        [ForeignKey("MaNguoiDung")]
        public virtual User? NguoiDung { get; set; }

        public virtual ICollection<OrderDetail> ChiTietDonHang { get; set; } = new List<OrderDetail>();
        public virtual ICollection<OrderStatusHistory> LichSuTrangThai { get; set; } = new List<OrderStatusHistory>();
    }
}