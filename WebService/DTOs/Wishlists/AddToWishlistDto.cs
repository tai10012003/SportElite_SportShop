using System.ComponentModel.DataAnnotations;

namespace WebService.DTOs.Wishlists
{
    public class AddToWishlistDto
    {
        [Required(ErrorMessage = "Mã sản phẩm là bắt buộc")]
        public string MaSanPham { get; set; } = string.Empty;
    }
}