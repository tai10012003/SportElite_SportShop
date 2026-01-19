using AutoMapper;
using WebService.DTOs.Wishlists;
using WebService.Interfaces.Products;
using WebService.Interfaces.Wishlists;
using WebService.Models;

namespace WebService.Services
{
    public class WishlistService : IWishlistService
    {
        private readonly IWishlistRepository _wishlistRepo;
        private readonly IProductRepository _productRepo;
        private readonly IMapper _mapper;

        public WishlistService(IWishlistRepository wishlistRepo, IProductRepository productRepo, IMapper mapper)
        {
            _wishlistRepo = wishlistRepo;
            _productRepo = productRepo;
            _mapper = mapper;
        }

        public async Task<WishlistActionResultDto> AddToWishlistAsync(string maNguoiDung, AddToWishlistDto dto)
        {
            var product = await _productRepo.GetByCodeAsync(dto.MaSanPham);
            if (product == null)
            {
                return new WishlistActionResultDto
                {
                    Success = false,
                    Message = "Sản phẩm không tồn tại",
                    IsInWishlist = false
                };
            }
            var exists = await _wishlistRepo.ExistsAsync(maNguoiDung, dto.MaSanPham);
            if (exists)
            {
                return new WishlistActionResultDto
                {
                    Success = false,
                    Message = "Sản phẩm đã có trong danh sách yêu thích",
                    IsInWishlist = true
                };
            }
            var wishlist = new Wishlist
            {
                MaNguoiDung = maNguoiDung,
                MaSanPham = dto.MaSanPham,
                NgayThem = DateTime.Now
            };
            await _wishlistRepo.AddAsync(wishlist);
            await _wishlistRepo.SaveChangesAsync();
            return new WishlistActionResultDto
            {
                Success = true,
                Message = "Đã thêm vào danh sách yêu thích",
                IsInWishlist = true
            };
        }

        public async Task<WishlistActionResultDto> RemoveFromWishlistAsync(string maNguoiDung, string maSanPham)
        {
            var wishlist = await _wishlistRepo.GetByUserAndProductAsync(maNguoiDung, maSanPham);
            if (wishlist == null)
            {
                return new WishlistActionResultDto
                {
                    Success = false,
                    Message = "Sản phẩm không có trong danh sách yêu thích",
                    IsInWishlist = false
                };
            }
            await _wishlistRepo.RemoveAsync(wishlist);
            await _wishlistRepo.SaveChangesAsync();
            return new WishlistActionResultDto
            {
                Success = true,
                Message = "Đã xóa khỏi danh sách yêu thích",
                IsInWishlist = false
            };
        }

        public async Task<WishlistActionResultDto> ToggleWishlistAsync(string maNguoiDung, string maSanPham)
        {
            var exists = await _wishlistRepo.ExistsAsync(maNguoiDung, maSanPham);
            if (exists)
            {
                return await RemoveFromWishlistAsync(maNguoiDung, maSanPham);
            }
            else
            {
                return await AddToWishlistAsync(maNguoiDung, new AddToWishlistDto { MaSanPham = maSanPham });
            }
        }

        public async Task<WishlistResponseDto> GetWishlistAsync(string maNguoiDung)
        {
            var wishlists = await _wishlistRepo.GetByMaNguoiDungAsync(maNguoiDung);
            var items = _mapper.Map<List<WishlistItemDto>>(wishlists);
            return new WishlistResponseDto
            {
                TongSanPham = items.Count,
                SanPham = items
            };
        }

        public async Task<CheckWishlistDto> CheckProductInWishlistAsync(string maNguoiDung, string maSanPham)
        {
            var exists = await _wishlistRepo.ExistsAsync(maNguoiDung, maSanPham);
            return new CheckWishlistDto
            {
                MaSanPham = maSanPham,
                IsInWishlist = exists
            };
        }

        public async Task<List<CheckWishlistDto>> CheckMultipleProductsAsync(string maNguoiDung, List<string> maSanPhams)
        {
            var result = new List<CheckWishlistDto>();
            foreach (var maSanPham in maSanPhams)
            {
                var exists = await _wishlistRepo.ExistsAsync(maNguoiDung, maSanPham);
                result.Add(new CheckWishlistDto
                {
                    MaSanPham = maSanPham,
                    IsInWishlist = exists
                });
            }
            return result;
        }
    }
}