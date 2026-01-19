using Microsoft.EntityFrameworkCore;
using WebService.Models;

namespace WebService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductReview> ProductReviews { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<OrderStatusHistory> OrderStatusHistories { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
                .Property(p => p.NgayTao)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Product>()
                .Property(p => p.NgayTao)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Category>()
                .HasAlternateKey(c => c.MaDanhMuc);

            modelBuilder.Entity<Product>()
                .HasOne<Category>()
                .WithMany()
                .HasForeignKey(p => p.MaDanhMuc)
                .HasPrincipalKey(c => c.MaDanhMuc);

            modelBuilder.Entity<Brand>()
                .HasAlternateKey(b => b.MaThuongHieu);
                
            modelBuilder.Entity<Product>()
                .HasOne<Brand>()
                .WithMany()
                .HasForeignKey(p => p.MaThuongHieu)
                .HasPrincipalKey(b => b.MaThuongHieu);

            modelBuilder.Entity<Category>()
                .Property(c => c.NgayTao)
                .HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<Category>()
                .Property(c => c.NgayCapNhat)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Brand>()
                .Property(b => b.NgayTao)
                .HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<Brand>()
                .Property(b => b.NgayCapNhat)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Product>()
                .HasAlternateKey(p => p.MaSanPham);

            modelBuilder.Entity<ProductImage>()
                .Property(b => b.NgayTao)
                .HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<ProductImage>()
                .Property(b => b.NgayCapNhat)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<ProductImage>()
                .HasOne(pi => pi.SanPham)
                .WithMany(p => p.HinhAnh)
                .HasForeignKey(pi => pi.MaSanPham)
                .HasPrincipalKey(p => p.MaSanPham);

            modelBuilder.Entity<ProductReview>()
                .Property(pr => pr.NgayTao)
                .HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<ProductReview>()
                .Property(pr => pr.NgayCapNhat)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<ProductReview>()
                .HasOne<Product>()
                .WithMany()
                .HasForeignKey(pr => pr.MaSanPham)
                .HasPrincipalKey(p => p.MaSanPham);

            modelBuilder.Entity<User>()
                .Property(u => u.NgayTao)
                .HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<User>()
                .Property(u => u.NgayCapNhat)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Order>()
                .HasAlternateKey(o => o.MaDonHang);

            modelBuilder.Entity<Order>()
                .Property(o => o.NgayTao)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Order>()
                .Property(o => o.NgayCapNhat)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Order>()
                .HasOne(o => o.NguoiDung)
                .WithMany()
                .HasForeignKey(o => o.MaNguoiDung)
                .HasPrincipalKey(u => u.MaNguoiDung);

            modelBuilder.Entity<OrderDetail>()
                .Property(od => od.NgayTao)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.DonHang)
                .WithMany(o => o.ChiTietDonHang)
                .HasForeignKey(od => od.MaDonHang)
                .HasPrincipalKey(o => o.MaDonHang);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.SanPham)
                .WithMany()
                .HasForeignKey(od => od.MaSanPham)
                .HasPrincipalKey(p => p.MaSanPham);

            modelBuilder.Entity<OrderStatusHistory>()
                .Property(osh => osh.ThoiGian)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<OrderStatusHistory>()
                .HasOne(osh => osh.DonHang)
                .WithMany(o => o.LichSuTrangThai)
                .HasForeignKey(osh => osh.MaDonHang)
                .HasPrincipalKey(o => o.MaDonHang);

            modelBuilder.Entity<Wishlist>()
                .Property(w => w.NgayThem)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Wishlist>()
                .HasIndex(w => new { w.MaNguoiDung, w.MaSanPham })
                .IsUnique();

            modelBuilder.Entity<Wishlist>()
                .HasOne(w => w.NguoiDung)
                .WithMany()
                .HasForeignKey(w => w.MaNguoiDung)
                .HasPrincipalKey(u => u.MaNguoiDung)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Wishlist>()
                .HasOne(w => w.SanPham)
                .WithMany()
                .HasForeignKey(w => w.MaSanPham)
                .HasPrincipalKey(p => p.MaSanPham)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
