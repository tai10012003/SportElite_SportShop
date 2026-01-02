using AutoMapper;
using WebService.DTOs.ProductReviews;
using Microsoft.EntityFrameworkCore;
using WebService.Data;
using WebService.Interfaces.ProductReviews;
using WebService.Models;

namespace WebService.Services
{
    public class ProductReviewService : IProductReviewService
    {
        private readonly IProductReviewRepository _repo;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        public ProductReviewService(IProductReviewRepository repo, IMapper mapper, AppDbContext context)
        {
            _repo = repo;
            _mapper = mapper;
            _context = context;
        }

        public async Task<IEnumerable<GetProductReviewDTO>> GetByProductAsync(string maSanPham)
        {
            var reviews = await _repo.GetByProductAsync(maSanPham);
            var reviewList = reviews.ToList();
            if (reviewList.Any())
            {
                var maNguoiDungs = reviewList.Select(r => r.MaNguoiDung).Distinct();
                var users = await _context.Users.Where(u => maNguoiDungs.Contains(u.MaNguoiDung)).ToDictionaryAsync(u => u.MaNguoiDung, u => u.HoTen);
                var dtos = _mapper.Map<List<GetProductReviewDTO>>(reviewList);
                foreach (var dto in dtos)
                {
                    var review = reviewList.First(r => r.Id == dto.Id);
                    dto.HoTen = users.GetValueOrDefault(review.MaNguoiDung, "Người dùng ẩn danh");
                }
                return dtos;
            }
            return new List<GetProductReviewDTO>();
        }

        public async Task<GetProductReviewDTO> GetByIdAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException("Không tìm thấy đánh giá");
            return _mapper.Map<GetProductReviewDTO>(entity);
        }

        public async Task<ProductReviewResponseDTO> CreateAsync(CreateProductReviewDTO dto)
        {
            var entity = _mapper.Map<ProductReview>(dto);
            entity.NgayTao = entity.NgayCapNhat = DateTime.Now;
            await _repo.AddAsync(entity);
            await _repo.SaveChangesAsync();
            var result = _mapper.Map<ProductReviewResponseDTO>(entity);
            result.Message = "Tạo đánh giá thành công";
            return result;
        }

        public async Task<ProductReviewResponseDTO> UpdateAsync(int id, UpdateProductReviewDTO dto)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException("Không tìm thấy đánh giá");
            entity.DiemDanhGia = dto.DiemDanhGia;
            entity.NoiDung = dto.NoiDung;
            entity.NgayCapNhat = DateTime.Now;
            await _repo.UpdateAsync(entity);
            await _repo.SaveChangesAsync();
            var result = _mapper.Map<ProductReviewResponseDTO>(entity);
            result.Message = "Cập nhật đánh giá thành công";
            return result;
        }

        public async Task<ProductReviewResponseDTO> DeleteAsync(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException("Không tìm thấy đánh giá");
            await _repo.DeleteAsync(entity);
            await _repo.SaveChangesAsync();
            var result = _mapper.Map<ProductReviewResponseDTO>(entity);
            result.Message = "Xóa đánh giá thành công";
            return result;
        }
    }
}
