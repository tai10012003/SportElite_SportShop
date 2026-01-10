using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebService.DTOs.Orders;
using WebService.Interfaces.Orders;

namespace WebService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<OrderListResponseDto>>> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        [HttpGet("my-orders")]
        [Authorize]
        public async Task<ActionResult<List<OrderListResponseDto>>> GetMyOrders()
        {
            var maNguoiDung = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(maNguoiDung))
                return Unauthorized(new { message = "Không tìm thấy thông tin người dùng" });
            var orders = await _orderService.GetOrdersByUserAsync(maNguoiDung);
            return Ok(orders);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<OrderResponseDto>> GetOrderById(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound(new { message = "Không tìm thấy đơn hàng" });
            var maNguoiDung = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var isAdmin = User.IsInRole("Admin");
            if (!isAdmin && order.Items.FirstOrDefault()?.MaSanPham != null)
            {
            }
            return Ok(order);
        }

        [HttpGet("code/{maDonHang}")]
        [Authorize]
        public async Task<ActionResult<OrderResponseDto>> GetOrderByCode(string maDonHang)
        {
            var order = await _orderService.GetOrderByCodeAsync(maDonHang);
            if (order == null)
                return NotFound(new { message = "Không tìm thấy đơn hàng" });
            return Ok(order);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<OrderResponseDto>> CreateOrder([FromBody] CreateOrderDto createOrderDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var maNguoiDung = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(maNguoiDung))
                return Unauthorized(new { message = "Không tìm thấy thông tin người dùng" });
            try
            {
                var order = await _orderService.CreateOrderAsync(createOrderDto, maNguoiDung);
                return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Tạo đơn hàng thất bại", error = ex.Message });
            }
        }

        [HttpPut("{id}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<OrderResponseDto>> UpdateOrderStatus(int id, [FromBody] UpdateOrderStatusDto updateStatusDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var order = await _orderService.UpdateOrderStatusAsync(id, updateStatusDto);
            if (order == null)
                return NotFound(new { message = "Không tìm thấy đơn hàng" });
            return Ok(order);
        }

        [HttpPut("{id}/cancel")]
        [Authorize]
        public async Task<ActionResult> CancelOrder(int id)
        {
            var maNguoiDung = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(maNguoiDung))
                return Unauthorized(new { message = "Không tìm thấy thông tin người dùng" });
            var result = await _orderService.CancelOrderAsync(id, maNguoiDung);
            if (!result)
                return BadRequest(new { message = "Không thể hủy đơn hàng" });
            return Ok(new { message = "Đã hủy đơn hàng thành công" });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteOrder(int id)
        {
            var result = await _orderService.DeleteOrderAsync(id);
            if (!result)
                return NotFound(new { message = "Không tìm thấy đơn hàng" });
            return Ok(new { message = "Đã xóa đơn hàng thành công" });
        }
    }
}