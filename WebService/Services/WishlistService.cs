using AutoMapper;
using WebService.DTOs.Wishlists;
using WebService.Interfaces.Products;
using WebService.Interfaces.Wishlists;
using WebService.Models;
using WebService.Data;
using Microsoft.EntityFrameworkCore;

namespace WebService.Services
{
    public class WishlistService : IWishlistService
    {
        private readonly IWishlistRepository _wishlistRepo;
        private readonly IProductRepository _productRepo;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        public WishlistService(IWishlistRepository wishlistRepo, IProductRepository productRepo, IMapper mapper, AppDbContext context)
        {
            _wishlistRepo = wishlistRepo;
            _productRepo = productRepo;
            _mapper = mapper;
            _context = context;
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
            var maDanhMucs = wishlists.Select(w => w.SanPham!.MaDanhMuc).Distinct().ToList();
            var maThuongHieus = wishlists.Select(w => w.SanPham!.MaThuongHieu).Distinct().ToList();
            var maSanPhams = wishlists.Select(w => w.SanPham!.MaSanPham).ToList();
            var categories = await _context.Categories.Where(c => maDanhMucs.Contains(c.MaDanhMuc)).ToDictionaryAsync(c => c.MaDanhMuc, c => c.TenDanhMuc);
            var brands = await _context.Brands.Where(b => maThuongHieus.Contains(b.MaThuongHieu)).ToDictionaryAsync(b => b.MaThuongHieu, b => b.TenThuongHieu);
            var reviewStats = await _context.ProductReviews
                .Where(r => maSanPhams.Contains(r.MaSanPham))
                .GroupBy(r => r.MaSanPham)
                .Select(g => new
                {
                    MaSanPham = g.Key,
                    AverageRating = g.Average(r => r.DiemDanhGia),
                    TotalReviews = g.Count()
                })
                .ToDictionaryAsync(x => x.MaSanPham);
            foreach (var item in items)
            {
                var product = wishlists.First(w => w.SanPham!.MaSanPham == item.MaSanPham).SanPham!;
                item.TenDanhMuc = categories.GetValueOrDefault(product.MaDanhMuc, "Danh mục");
                item.TenThuongHieu = brands.GetValueOrDefault(product.MaThuongHieu, "Thương hiệu");
                item.KichThuoc = product.KichThuoc;
                item.MauSac = product.MauSac;
                item.Slug = product.Slug;
                item.HinhAnhList = product.HinhAnh.Select(h => h.DuongDan).ToList();
                if (reviewStats.TryGetValue(product.MaSanPham, out var stats))
                {
                    item.AverageRating = Math.Round(stats.AverageRating, 1);
                    item.TotalReviews = stats.TotalReviews;
                }
                else
                {
                    item.AverageRating = 0;
                    item.TotalReviews = 0;
                }
            }
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