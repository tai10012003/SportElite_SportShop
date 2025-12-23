namespace WebService.DTOs.Products
{
    public class GetProductDTO
    {
        public int Id { get; set; }
        public string MaSanPham { get; set; } = string.Empty;
        public string TenSanPham { get; set; } = string.Empty;
        public string TenDanhMuc { get; set; } = string.Empty;
        public string TenThuongHieu { get; set; } = string.Empty;
        public decimal Gia { get; set; }
        public decimal? GiaKhuyenMai { get; set; }
        public string Slug { get; set; } = string.Empty;
        public bool NoiBat { get; set; }
        public ICollection<ProductImageInProductDTO> HinhAnh { get; set; } = new List<ProductImageInProductDTO>();
    }
}