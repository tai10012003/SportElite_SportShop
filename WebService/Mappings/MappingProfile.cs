using AutoMapper;
using WebService.DTOs.Products;
using WebService.DTOs.Categories;
using WebService.DTOs.Brands;
using WebService.DTOs.ProductImages;
using WebService.DTOs.Users;
using WebService.DTOs.ProductReviews;
using WebService.DTOs.Orders;
using WebService.DTOs.Wishlists;
using WebService.Models;

namespace WebService.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, GetProductDTO>()
            .ForMember(dest => dest.HinhAnh, opt => opt.MapFrom(src => src.HinhAnh));
            CreateMap<Product, ProductResponseDTO>();
            CreateMap<CreateProductDTO, Product>();
            CreateMap<UpdateProductDTO, Product>();

            CreateMap<ProductImage, ProductImageInProductDTO>();
            CreateMap<ProductImage, WebService.DTOs.ProductImages.ProductImageDTO>();
            CreateMap<CreateProductImageDTO, ProductImage>();
            CreateMap<UpdateProductImageDTO, ProductImage>()
            .ForMember(dest => dest.MaSanPham, opt => opt.Ignore())
            .ForMember(dest => dest.DuongDan, opt => opt.Ignore());
            CreateMap<ProductImage, WebService.DTOs.Products.ProductImageDTO>();

            CreateMap<Brand, GetBrandDTO>();
            CreateMap<Brand, BrandResponseDTO>();
            CreateMap<CreateBrandDTO, Brand>();
            CreateMap<UpdateBrandDTO, Brand>()
            .ForMember(dest => dest.MaThuongHieu, opt => opt.Ignore());

            CreateMap<Category, GetCategoryDTO>();
            CreateMap<Category, CategoryResponseDTO>();
            CreateMap<CreateCategoryDTO, Category>();
            CreateMap<UpdateCategoryDTO, Category>()
            .ForMember(dest => dest.MaDanhMuc, opt => opt.Ignore());

            CreateMap<User, GetUserDTO>();
            CreateMap<User, UserResponseDTO>()
                .ForMember(dest => dest.Success, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.Message, opt => opt.Ignore());
            CreateMap<CreateUserDTO, User>()
                .ForMember(dest => dest.NgayTao, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.NgayCapNhat, opt => opt.MapFrom(src => DateTime.Now));
            CreateMap<UpdateUserDTO, User>()
                .ForMember(dest => dest.NgayCapNhat, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.TenDangNhap, opt => opt.Ignore())
                .ForMember(dest => dest.MatKhau, opt => opt.Ignore())
                .ForMember(dest => dest.NgayTao, opt => opt.Ignore());

            CreateMap<ProductReview, GetProductReviewDTO>();
            CreateMap<ProductReview, ProductReviewResponseDTO>();
            CreateMap<CreateProductReviewDTO, ProductReview>()
                .ForMember(dest => dest.NgayTao, opt => opt.MapFrom(_ => DateTime.Now))
                .ForMember(dest => dest.NgayCapNhat, opt => opt.MapFrom(_ => DateTime.Now));
            CreateMap<UpdateProductReviewDTO, ProductReview>()
                .ForMember(dest => dest.MaSanPham, opt => opt.Ignore())
                .ForMember(dest => dest.MaNguoiDung, opt => opt.Ignore())
                .ForMember(dest => dest.NgayTao, opt => opt.Ignore())
                .ForMember(dest => dest.NgayCapNhat, opt => opt.MapFrom(_ => DateTime.Now));

            CreateMap<Order, OrderResponseDto>()
                .ForMember(dest => dest.MaDonHang, opt => opt.MapFrom(src => src.MaDonHang))
                .ForMember(dest => dest.TamTinh, opt => opt.MapFrom(src => src.TamTinh))
                .ForMember(dest => dest.SoTienGiam, opt => opt.MapFrom(src => src.SoTienGiam))
                .ForMember(dest => dest.PhiVanChuyen, opt => opt.MapFrom(src => src.PhiVanChuyen))
                .ForMember(dest => dest.TongThanhToan, opt => opt.MapFrom(src => src.TongThanhToan))
                .ForMember(dest => dest.TrangThai, opt => opt.MapFrom(src => src.TrangThai))
                .ForMember(dest => dest.DaThanhToan, opt => opt.MapFrom(src => src.DaThanhToan))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.ChiTietDonHang));
            CreateMap<OrderDetail, OrderItemResponseDto>()
                .ForMember(dest => dest.MaSanPham, opt => opt.MapFrom(src => src.MaSanPham))
                .ForMember(dest => dest.TenSanPham, opt => opt.MapFrom(src => src.TenSanPham))
                .ForMember(dest => dest.HinhAnh, opt => opt.MapFrom(src => src.HinhAnh))
                .ForMember(dest => dest.DonGia, opt => opt.MapFrom(src => src.DonGia))
                .ForMember(dest => dest.SoLuong, opt => opt.MapFrom(src => src.SoLuong))
                .ForMember(dest => dest.ThanhTien, opt => opt.MapFrom(src => src.ThanhTien));   

            CreateMap<Wishlist, WishlistItemDto>()
                .ForMember(dest => dest.MaSanPham, opt => opt.MapFrom(src => src.MaSanPham))
                .ForMember(dest => dest.TenSanPham, opt => opt.MapFrom(src => src.SanPham!.TenSanPham))
                .ForMember(dest => dest.HinhAnh, opt => opt.MapFrom(src => src.SanPham!.HinhAnh.FirstOrDefault()!.DuongDan))
                .ForMember(dest => dest.Gia, opt => opt.MapFrom(src => src.SanPham!.Gia))
                .ForMember(dest => dest.GiaKhuyenMai, opt => opt.MapFrom(src => src.SanPham!.GiaKhuyenMai))
                .ForMember(dest => dest.SoLuong, opt => opt.MapFrom(src => src.SanPham!.SoLuong))
                .ForMember(dest => dest.MaDanhMuc, opt => opt.MapFrom(src => src.SanPham!.MaDanhMuc))
                .ForMember(dest => dest.MaThuongHieu, opt => opt.MapFrom(src => src.SanPham!.MaThuongHieu))
                .ForMember(dest => dest.NgayThem, opt => opt.MapFrom(src => src.NgayThem))
                .ForMember(dest => dest.ConHang, opt => opt.MapFrom(src => src.SanPham!.SoLuong > 0))
                .ForMember(dest => dest.TenDanhMuc, opt => opt.Ignore())
                .ForMember(dest => dest.TenThuongHieu, opt => opt.Ignore())
                .ForMember(dest => dest.AverageRating, opt => opt.Ignore())
                .ForMember(dest => dest.TotalReviews, opt => opt.Ignore())
                .ForMember(dest => dest.KichThuoc, opt => opt.Ignore())
                .ForMember(dest => dest.MauSac, opt => opt.Ignore())
                .ForMember(dest => dest.HinhAnhList, opt => opt.Ignore());
        }
    }
}