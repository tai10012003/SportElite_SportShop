using Microsoft.EntityFrameworkCore;
using WebService.Data;
using WebService.Interfaces.Orders;
using WebService.Models;

namespace WebService.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            return await _context.Orders.Include(o => o.ChiTietDonHang).Include(o => o.LichSuTrangThai).Include(o => o.NguoiDung).FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<Order?> GetByOrderCodeAsync(string maDonHang)
        {
            return await _context.Orders.Include(o => o.ChiTietDonHang).Include(o => o.LichSuTrangThai).Include(o => o.NguoiDung).FirstOrDefaultAsync(o => o.MaDonHang == maDonHang);
        }

        public async Task<List<Order>> GetAllAsync()
        {
            return await _context.Orders.Include(o => o.ChiTietDonHang).Include(o => o.NguoiDung).OrderByDescending(o => o.NgayTao).ToListAsync();
        }

        public async Task<List<Order>> GetByUserIdAsync(string maNguoiDung)
        {
            return await _context.Orders.Include(o => o.ChiTietDonHang).Where(o => o.MaNguoiDung == maNguoiDung).OrderByDescending(o => o.NgayTao).ToListAsync();
        }

        public async Task<Order> CreateAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<Order> UpdateAsync(Order order)
        {
            order.NgayCapNhat = DateTime.Now;
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return false;
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Orders.AnyAsync(o => o.Id == id);
        }

        public async Task<string> GenerateOrderCodeAsync()
        {
            var today = DateTime.Now;
            var prefix = $"ORD{today:yyyyMMdd}";
            var lastOrder = await _context.Orders.Where(o => o.MaDonHang.StartsWith(prefix)).OrderByDescending(o => o.MaDonHang).FirstOrDefaultAsync();
            if (lastOrder == null)
            {
                return $"{prefix}0001";
            }
            var lastNumber = int.Parse(lastOrder.MaDonHang.Substring(prefix.Length));
            var newNumber = lastNumber + 1;
            return $"{prefix}{newNumber:D4}";
        }
    }
}