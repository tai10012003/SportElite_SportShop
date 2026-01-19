using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebService.Models
{
    [Table("Wishlists")]
    public class Wishlist
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("ma_nguoi_dung", TypeName = "varchar(8)")]
        public string MaNguoiDung { get; set; } = string.Empty;

        [Required]
        [Column("ma_san_pham")]
        [StringLength(8)]
        public string MaSanPham { get; set; } = string.Empty;

        [Column("ngay_them")]
        public DateTime NgayThem { get; set; } = DateTime.Now;

        [ForeignKey("MaNguoiDung")]
        public virtual User? NguoiDung { get; set; }

        [ForeignKey("MaSanPham")]
        public virtual Product? SanPham { get; set; }
    }
}