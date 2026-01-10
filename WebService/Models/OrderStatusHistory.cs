using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebService.Enums;

namespace WebService.Models
{
    [Table("OrderStatusHistories")]
    public class OrderStatusHistory
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("ma_don_hang", TypeName = "varchar(20)")]
        public string MaDonHang { get; set; } = string.Empty;

        [Required]
        [Column("trang_thai")]
        public OrderStatus TrangThai { get; set; }

        [Column("ghi_chu", TypeName = "nvarchar(255)")]
        public string? GhiChu { get; set; }

        [Required]
        [Column("thoi_gian")]
        public DateTime ThoiGian { get; set; } = DateTime.Now;

        [ForeignKey("MaDonHang")]
        public virtual Order? DonHang { get; set; }
    }
}