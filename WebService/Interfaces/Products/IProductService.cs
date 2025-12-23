using WebService.DTOs.Products;

namespace WebService.Interfaces.Products
{
    public interface IProductService
    {
        Task<(IEnumerable<GetProductDTO> products, int totalCount)> GetAllAsync(
        string search = "",
        string category = "",
        string brand = "",
        string price = "",
        string sort = "",
        int page = 1,
        int pageSize = 9,
        bool promotion = false
    );
        Task<List<GetProductDTO>> GetSuggestionsAsync(string query);
        Task<ProductResponseDTO?> GetByIdAsync(int id);
        Task<ProductResponseDTO?> GetBySlugAsync(string slug);
        Task<ProductResponseDTO?> GetByCodeAsync(string maSanPham);
        Task<IEnumerable<GetProductDTO>> GetFeaturedAsync();
        Task<IEnumerable<GetProductDTO>> GetRelatedAsync(string slug);
        Task<ProductResponseDTO> CreateAsync(CreateProductDTO productDto);
        Task UpdateAsync(int id, UpdateProductDTO productDto);
        Task DeleteAsync(int id);
    }
}