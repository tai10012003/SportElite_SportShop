using WebService.Models;

namespace WebService.Interfaces.Orders
{
    public interface IOrderRepository
    {
        Task<Order?> GetByIdAsync(int id);
        Task<Order?> GetByOrderCodeAsync(string maDonHang);
        Task<List<Order>> GetAllAsync();
        Task<List<Order>> GetByUserIdAsync(string maNguoiDung);
        Task<Order> CreateAsync(Order order);
        Task<Order> UpdateAsync(Order order);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<string> GenerateOrderCodeAsync();
    }
}