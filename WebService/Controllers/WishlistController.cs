using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using WebService.DTOs.Wishlists;
using WebService.Interfaces.Wishlists;

namespace WebService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistService _wishlistService;

        public WishlistController(IWishlistService wishlistService)
        {
            _wishlistService = wishlistService;
        }

        [HttpPost]
        public async Task<IActionResult> AddToWishlist([FromBody] AddToWishlistDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var maNguoiDung = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(maNguoiDung))
                return Unauthorized(new { message = "Không tìm thấy thông tin người dùng" });
            var result = await _wishlistService.AddToWishlistAsync(maNguoiDung, dto);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [HttpDelete("{maSanPham}")]
        public async Task<IActionResult> RemoveFromWishlist(string maSanPham)
        {
            var maNguoiDung = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(maNguoiDung))
                return Unauthorized(new { message = "Không tìm thấy thông tin người dùng" });
            var result = await _wishlistService.RemoveFromWishlistAsync(maNguoiDung, maSanPham);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        [HttpPost("toggle")]
        public async Task<IActionResult> ToggleWishlist([FromBody] AddToWishlistDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var maNguoiDung = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(maNguoiDung))
                return Unauthorized(new { message = "Không tìm thấy thông tin người dùng" });
            var result = await _wishlistService.ToggleWishlistAsync(maNguoiDung, dto.MaSanPham);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetWishlist()
        {
            var maNguoiDung = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(maNguoiDung))
                return Unauthorized(new { message = "Không tìm thấy thông tin người dùng" });
            var result = await _wishlistService.GetWishlistAsync(maNguoiDung);
            return Ok(result);
        }

        [HttpGet("check/{maSanPham}")]
        public async Task<IActionResult> CheckProductInWishlist(string maSanPham)
        {
            var maNguoiDung = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(maNguoiDung))
                return Unauthorized(new { message = "Không tìm thấy thông tin người dùng" });
            var result = await _wishlistService.CheckProductInWishlistAsync(maNguoiDung, maSanPham);
            return Ok(result);
        }

        [HttpPost("check-multiple")]
        public async Task<IActionResult> CheckMultipleProducts([FromBody] List<string> maSanPhams)
        {
            var maNguoiDung = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(maNguoiDung))
                return Unauthorized(new { message = "Không tìm thấy thông tin người dùng" });
            var result = await _wishlistService.CheckMultipleProductsAsync(maNguoiDung, maSanPhams);
            return Ok(result);
        }
    }
}