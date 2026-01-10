using WebService.DTOs.Orders;
using WebService.Enums;

namespace WebService.Interfaces.Orders
{
    public interface IOrderService
    {
        Task<OrderResponseDto?> GetOrderByIdAsync(int id);
        Task<OrderResponseDto?> GetOrderByCodeAsync(string maDonHang);
        Task<List<OrderListResponseDto>> GetAllOrdersAsync();
        Task<List<OrderListResponseDto>> GetOrdersByUserAsync(string maNguoiDung);
        Task<OrderResponseDto> CreateOrderAsync(CreateOrderDto createOrderDto, string maNguoiDung);
        Task<OrderResponseDto?> UpdateOrderStatusAsync(int id, UpdateOrderStatusDto updateStatusDto);
        Task<bool> CancelOrderAsync(int id, string maNguoiDung);
        Task<bool> DeleteOrderAsync(int id);
    }
}