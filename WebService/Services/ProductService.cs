using AutoMapper;
using WebService.DTOs.Products;
using WebService.Interfaces.Products;
using WebService.Models;
using WebService.Data;
using Microsoft.EntityFrameworkCore;

namespace WebService.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        public ProductService(IProductRepository repository, IMapper mapper, AppDbContext context)
        {
            _repository = repository;
            _mapper = mapper;
            _context = context;
        }

        public async Task<(IEnumerable<GetProductDTO> products, int totalCount)> GetAllAsync(
            string search = "",
            string category = "",
            string brand = "",
            string price = "",
            string sort = "",
            int page = 1,
            int pageSize = 9,
            bool promotion = false
        )
        {
            var products = await _repository.GetAllAsync();
            var query = products.AsQueryable();
            if (!string.IsNullOrWhiteSpace(search))
            {
                var lowerSearch = search.ToLower();
                query = query.Where(p =>
                    (!string.IsNullOrEmpty(p.TenSanPham) && p.TenSanPham.ToLower().Contains(lowerSearch)) ||
                    (!string.IsNullOrEmpty(p.MaSanPham) && p.MaSanPham.ToLower().Contains(lowerSearch))
                );
            }
            if (!string.IsNullOrEmpty(category))
                query = query.Where(p => p.MaDanhMuc == category);
            if (!string.IsNullOrEmpty(brand))
                query = query.Where(p => p.MaThuongHieu == brand);
            if (!string.IsNullOrEmpty(price))
            {
                var range = price.Split('-');
                if (range.Length == 2)
                {
                    if (decimal.TryParse(range[0], out decimal min) && decimal.TryParse(range[1], out decimal max))
                        query = query.Where(p => p.Gia >= min && p.Gia <= max);
                    else if (range[1] == "up" && decimal.TryParse(range[0], out decimal minUp))
                        query = query.Where(p => p.Gia >= minUp);
                }
            }
            if (promotion) query = query.Where(p => p.GiaKhuyenMai != null && p.GiaKhuyenMai < p.Gia);
            query = ApplySorting(query, sort);
            var pagedProducts = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            int totalCount = query.Count();
            if (pagedProducts.Any())
            {
                var maSanPhams = pagedProducts.Select(p => p.MaSanPham).ToList();
                var maDanhMucs = pagedProducts.Select(p => p.MaDanhMuc).Distinct().ToList();
                var maThuongHieus = pagedProducts.Select(p => p.MaThuongHieu).Distinct().ToList();
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
                var dtos = _mapper.Map<List<GetProductDTO>>(pagedProducts);
                foreach (var dto in dtos)
                {
                    var product = pagedProducts.First(p => p.MaSanPham == dto.MaSanPham);
                    dto.TenDanhMuc = categories.GetValueOrDefault(product.MaDanhMuc, "Danh mục");
                    dto.TenThuongHieu = brands.GetValueOrDefault(product.MaThuongHieu, "Thương hiệu");
                    if (reviewStats.TryGetValue(product.MaSanPham, out var stats))
                    {
                        dto.AverageRating = Math.Round(stats.AverageRating, 1);
                        dto.TotalReviews = stats.TotalReviews;
                    }
                    else
                    {
                        dto.AverageRating = 0;
                        dto.TotalReviews = 0;
                    }
                }
                return (dtos, totalCount);
            }
            return (new List<GetProductDTO>(), 0);
        }

        private IQueryable<Product> ApplySorting(IQueryable<Product> query, string sort)
        {
            switch (sort?.ToLower())
            {
                case "price_asc":
                    return query.OrderBy(p => p.GiaKhuyenMai ?? p.Gia);
                case "price_desc":
                    return query.OrderByDescending(p => p.GiaKhuyenMai ?? p.Gia);
                default:
                    return query.OrderBy(p => p.Id);
            }
        }

        public async Task<List<GetProductDTO>> GetSuggestionsAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query)) return new List<GetProductDTO>();
            var products = await _repository.GetAllAsync();
            var lowerQuery = query.ToLower();
            var filtered = products.Where(p => !string.IsNullOrEmpty(p.TenSanPham) && p.TenSanPham.ToLower().Contains(lowerQuery)).Take(10).ToList();
            if (filtered.Any())
            {
                var maDanhMucs = filtered.Select(p => p.MaDanhMuc).Distinct();
                var maThuongHieus = filtered.Select(p => p.MaThuongHieu).Distinct();
                var categories = await _context.Categories.Where(c => maDanhMucs.Contains(c.MaDanhMuc)).ToDictionaryAsync(c => c.MaDanhMuc, c => c.TenDanhMuc);
                var brands = await _context.Brands.Where(b => maThuongHieus.Contains(b.MaThuongHieu)).ToDictionaryAsync(b => b.MaThuongHieu, b => b.TenThuongHieu);
                var dtos = filtered.Select(p => new GetProductDTO
                {
                    Id = p.Id,
                    MaSanPham = p.MaSanPham,
                    TenSanPham = p.TenSanPham,
                    Gia = p.Gia,
                    GiaKhuyenMai = p.GiaKhuyenMai,
                    Slug = p.Slug,
                    NoiBat = p.NoiBat,
                    HinhAnh = p.HinhAnh?
                    .Select(img => new ProductImageInProductDTO
                    {
                        Id = img.Id,
                        DuongDan = img.DuongDan,
                        AnhChinh = img.AnhChinh
                    }).ToList() ?? new List<ProductImageInProductDTO>(),
                    TenDanhMuc = categories.GetValueOrDefault(p.MaDanhMuc, "Danh mục"),
                    TenThuongHieu = brands.GetValueOrDefault(p.MaThuongHieu, "Thương hiệu")
                }).ToList();
                return dtos;
            }
            return new List<GetProductDTO>();
        }

        public async Task<ProductResponseDTO?> GetByIdAsync(int id)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null) return null;
            var dto = _mapper.Map<ProductResponseDTO>(product);
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.MaDanhMuc == product.MaDanhMuc);
            var brand = await _context.Brands.FirstOrDefaultAsync(b => b.MaThuongHieu == product.MaThuongHieu);
            dto.TenDanhMuc = category?.TenDanhMuc ?? "Danh mục";
            dto.TenThuongHieu = brand?.TenThuongHieu ?? "Thương hiệu";
            return dto;
        }

        public async Task<ProductResponseDTO?> GetBySlugAsync(string slug)
        {
            var product = await _repository.GetBySlugAsync(slug);
            if (product == null) return null;
            var dto = _mapper.Map<ProductResponseDTO>(product);
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.MaDanhMuc == product.MaDanhMuc);
            var brand = await _context.Brands.FirstOrDefaultAsync(b => b.MaThuongHieu == product.MaThuongHieu);
            dto.TenDanhMuc = category?.TenDanhMuc ?? "Danh mục";
            dto.TenThuongHieu = brand?.TenThuongHieu ?? "Thương hiệu";
            return dto;
        }

        public async Task<ProductResponseDTO?> GetByCodeAsync(string maSanPham)
        {
            var product = await _repository.GetByCodeAsync(maSanPham);
            if (product == null) return null;
            var dto = _mapper.Map<ProductResponseDTO>(product);
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.MaDanhMuc == product.MaDanhMuc);
            var brand = await _context.Brands.FirstOrDefaultAsync(b => b.MaThuongHieu == product.MaThuongHieu);
            dto.TenDanhMuc = category?.TenDanhMuc ?? "Danh mục";
            dto.TenThuongHieu = brand?.TenThuongHieu ?? "Thương hiệu";
            return dto;
        }

        public async Task<IEnumerable<GetProductDTO>> GetFeaturedAsync()
        {
            var products = await _repository.GetFeaturedAsync();
            var productList = products.ToList();
            if (productList.Any())
            {
                var maSanPhams = productList.Select(p => p.MaSanPham).ToList();
                var maDanhMucs = productList.Select(p => p.MaDanhMuc).Distinct();
                var maThuongHieus = productList.Select(p => p.MaThuongHieu).Distinct();
                var categories = await _context.Categories.Where(c => maDanhMucs.Contains(c.MaDanhMuc)).ToDictionaryAsync(c => c.MaDanhMuc, c => c.TenDanhMuc);
                var brands = await _context.Brands.Where(b => maThuongHieus.Contains(b.MaThuongHieu)).ToDictionaryAsync(b => b.MaThuongHieu, b => b.TenThuongHieu);
                var reviewStats = await _context.ProductReviews
                    .Where(r => maSanPhams.Contains(r.MaSanPham))
                    .GroupBy(r => r.MaSanPham)
                    .Select(g => new { MaSanPham = g.Key, Avg = g.Average(r => r.DiemDanhGia), Count = g.Count() })
                    .ToDictionaryAsync(x => x.MaSanPham);
                var dtos = _mapper.Map<List<GetProductDTO>>(productList);
                foreach (var dto in dtos)
                {
                    var product = productList.First(p => p.MaSanPham == dto.MaSanPham);
                    dto.TenDanhMuc = categories.GetValueOrDefault(product.MaDanhMuc, "Danh mục");
                    dto.TenThuongHieu = brands.GetValueOrDefault(product.MaThuongHieu, "Thương hiệu");
                    if (reviewStats.TryGetValue(product.MaSanPham, out var stats))
                    {
                        dto.AverageRating = Math.Round(stats.Avg, 1);
                        dto.TotalReviews = stats.Count;
                    }
                }
                return dtos;
            }
            return new List<GetProductDTO>();
        }

        public async Task<IEnumerable<GetProductDTO>> GetRelatedAsync(string slug)
        {
            var relatedProducts = await _repository.GetRelatedAsync(slug);
            var productList = relatedProducts.ToList();
            if (productList.Any())
            {
                var maSanPhams = productList.Select(p => p.MaSanPham).ToList();
                var maDanhMucs = productList.Select(p => p.MaDanhMuc).Distinct();
                var maThuongHieus = productList.Select(p => p.MaThuongHieu).Distinct();
                var categories = await _context.Categories.Where(c => maDanhMucs.Contains(c.MaDanhMuc)).ToDictionaryAsync(c => c.MaDanhMuc, c => c.TenDanhMuc);
                var brands = await _context.Brands.Where(b => maThuongHieus.Contains(b.MaThuongHieu)).ToDictionaryAsync(b => b.MaThuongHieu, b => b.TenThuongHieu);
                var reviewStats = await _context.ProductReviews
                    .Where(r => maSanPhams.Contains(r.MaSanPham))
                    .GroupBy(r => r.MaSanPham)
                    .Select(g => new { MaSanPham = g.Key, Avg = g.Average(r => r.DiemDanhGia), Count = g.Count() })
                    .ToDictionaryAsync(x => x.MaSanPham);
                var dtos = _mapper.Map<List<GetProductDTO>>(productList);
                foreach (var dto in dtos)
                {
                    var product = productList.First(p => p.MaSanPham == dto.MaSanPham);
                    dto.TenDanhMuc = categories.GetValueOrDefault(product.MaDanhMuc, "Danh mục");
                    dto.TenThuongHieu = brands.GetValueOrDefault(product.MaThuongHieu, "Thương hiệu");
                    if (reviewStats.TryGetValue(product.MaSanPham, out var stats))
                    {
                        dto.AverageRating = Math.Round(stats.Avg, 1);
                        dto.TotalReviews = stats.Count;
                    }
                }
                return dtos;
            }
            return new List<GetProductDTO>();
        }

        public async Task<ProductResponseDTO> CreateAsync(CreateProductDTO productDto)
        {
            var product = _mapper.Map<Product>(productDto);
            product.NgayTao = DateTime.Now;
            product.NgayCapNhat = DateTime.Now;
            
            var createdProduct = await _repository.CreateAsync(product);
            return _mapper.Map<ProductResponseDTO>(createdProduct);
        }

        public async Task UpdateAsync(int id, UpdateProductDTO productDto)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null) throw new KeyNotFoundException($"Product with ID {id} not found");
            _mapper.Map(productDto, product);
            product.NgayCapNhat = DateTime.Now;
            
            await _repository.UpdateAsync(product);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}