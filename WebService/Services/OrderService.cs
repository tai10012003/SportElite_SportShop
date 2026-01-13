using AutoMapper;
using WebService.DTOs.Orders;
using WebService.Enums;
using WebService.Interfaces.Orders;
using WebService.Interfaces.Users;
using WebService.Models;

namespace WebService.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, IUserRepository userRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<OrderResponseDto?> GetOrderByIdAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            return order == null ? null : _mapper.Map<OrderResponseDto>(order);
        }

        public async Task<OrderResponseDto?> GetOrderByCodeAsync(string maDonHang)
        {
            var order = await _orderRepository.GetByOrderCodeAsync(maDonHang);
            return order == null ? null : _mapper.Map<OrderResponseDto>(order);
        }

        public async Task<List<OrderListResponseDto>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetAllAsync();
            return orders.Select(o => new OrderListResponseDto
            {
                Id = o.Id,
                MaDonHang = o.MaDonHang,
                TenNguoiNhan = o.TenNguoiNhan,
                TongThanhToan = o.TongThanhToan,
                TrangThai = o.TrangThai,
                DaThanhToan = o.DaThanhToan,
                NgayTao = o.NgayTao,
                SoLuongSanPham = o.ChiTietDonHang.Sum(d => d.SoLuong)
            }).ToList();
        }

        public async Task<List<OrderListResponseDto>> GetOrdersByUserAsync(string maNguoiDung)
        {
            var orders = await _orderRepository.GetByUserIdAsync(maNguoiDung);
            return orders.Select(o => new OrderListResponseDto
            {
                Id = o.Id,
                MaDonHang = o.MaDonHang,
                TenNguoiNhan = o.TenNguoiNhan,
                TongThanhToan = o.TongThanhToan,
                PhuongThucThanhToan = o.PhuongThucThanhToan,
                TrangThai = o.TrangThai,
                DaThanhToan = o.DaThanhToan,
                NgayTao = o.NgayTao,
                SoLuongSanPham = o.ChiTietDonHang.Sum(d => d.SoLuong)
            }).ToList();
        }

        public async Task<OrderResponseDto> CreateOrderAsync(CreateOrderDto createOrderDto, string maNguoiDung)
        {
            await UpdateUserInfoFromCheckout(maNguoiDung, createOrderDto);
            var tamTinh = createOrderDto.Items.Sum(i => i.DonGia * i.SoLuong);
            var phiVanChuyen = tamTinh >= 500000 ? 0 : 30000;
            var soTienGiam = 0m;
            var tongThanhToan = tamTinh + phiVanChuyen - soTienGiam;
            var maDonHang = await _orderRepository.GenerateOrderCodeAsync();
            var order = new Order
            {
                MaDonHang = maDonHang,
                MaNguoiDung = maNguoiDung,
                TenNguoiNhan = createOrderDto.TenNguoiNhan,
                SoDienThoai = createOrderDto.SoDienThoai,
                DiaChi = createOrderDto.DiaChi,
                GhiChu = createOrderDto.GhiChu ?? string.Empty,
                TamTinh = tamTinh,
                MaGiamGia = createOrderDto.MaGiamGia,
                SoTienGiam = soTienGiam,
                PhiVanChuyen = phiVanChuyen,
                TongThanhToan = tongThanhToan,
                PhuongThucThanhToan = createOrderDto.PhuongThucThanhToan,
                TrangThai = OrderStatus.Pending,
                NgayTao = DateTime.Now,
                NgayCapNhat = DateTime.Now
            };
            foreach (var item in createOrderDto.Items)
            {
                var orderDetail = new OrderDetail
                {
                    MaDonHang = maDonHang,
                    MaSanPham = item.MaSanPham,
                    TenSanPham = item.TenSanPham,
                    HinhAnh = item.HinhAnh,
                    SoLuong = item.SoLuong,
                    MauSac = item.MauSac,
                    KichThuoc = item.KichThuoc,
                    DonGia = item.DonGia,
                    ThanhTien = item.DonGia * item.SoLuong,
                    NgayTao = DateTime.Now
                };
                order.ChiTietDonHang.Add(orderDetail);
            }
            order.LichSuTrangThai.Add(new OrderStatusHistory
            {
                MaDonHang = maDonHang,
                TrangThai = OrderStatus.Pending,
                GhiChu = "Đơn hàng đã được tạo",
                ThoiGian = DateTime.Now
            });
            var createdOrder = await _orderRepository.CreateAsync(order);
            return _mapper.Map<OrderResponseDto>(createdOrder);
        }

        public async Task<OrderResponseDto?> UpdateOrderStatusAsync(int id, UpdateOrderStatusDto updateStatusDto)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null) return null;
            order.TrangThai = updateStatusDto.TrangThai;
            order.NgayCapNhat = DateTime.Now;
            order.LichSuTrangThai.Add(new OrderStatusHistory
            {
                MaDonHang = order.MaDonHang,
                TrangThai = updateStatusDto.TrangThai,
                GhiChu = updateStatusDto.GhiChu,
                ThoiGian = DateTime.Now
            });
            var updatedOrder = await _orderRepository.UpdateAsync(order);
            return _mapper.Map<OrderResponseDto>(updatedOrder);
        }

        public async Task<bool> CancelOrderAsync(int id, string maNguoiDung)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null || order.MaNguoiDung != maNguoiDung) return false;
            if (order.TrangThai != OrderStatus.Pending && order.TrangThai != OrderStatus.Confirmed)
                return false;
            order.TrangThai = OrderStatus.Cancelled;
            order.NgayCapNhat = DateTime.Now;
            order.LichSuTrangThai.Add(new OrderStatusHistory
            {
                MaDonHang = order.MaDonHang,
                TrangThai = OrderStatus.Cancelled,
                GhiChu = "Đơn hàng bị hủy bởi khách hàng",
                ThoiGian = DateTime.Now
            });
            await _orderRepository.UpdateAsync(order);
            return true;
        }

        public async Task<bool> DeleteOrderAsync(int id)
        {
            return await _orderRepository.DeleteAsync(id);
        }

        private async Task UpdateUserInfoFromCheckout(string maNguoiDung, CreateOrderDto orderDto)
        {
            var user = await _userRepository.GetByMaNguoiDungAsync(maNguoiDung);
            if (user != null)
            {
                user.HoTen = orderDto.TenNguoiNhan;
                user.SoDienThoai = orderDto.SoDienThoai;
                user.DiaChi = orderDto.DiaChi;
                user.NgayCapNhat = DateTime.Now;
                await _userRepository.UpdateAsync(user);
            }
        }
    }
}