using System.ComponentModel.DataAnnotations;
using WebService.Enums;

namespace WebService.DTOs.Orders
{
    public class UpdateOrderStatusDto
    {
        [Required]
        public OrderStatus TrangThai { get; set; }

        [StringLength(255)]
        public string? GhiChu { get; set; }
    }
}