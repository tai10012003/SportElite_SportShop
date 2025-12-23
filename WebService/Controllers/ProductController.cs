using Microsoft.AspNetCore.Mvc;
using WebService.DTOs.Products;
using WebService.Interfaces.Products;
using WebService.Models;
using WebService.Interfaces.Brands;
using WebService.Interfaces.Categories;

namespace WebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IBrandService _brandService;

        public ProductController(IProductService productService, ICategoryService categoryService, IBrandService brandService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _brandService = brandService;
        }

        [HttpGet]
        public async Task<ActionResult<object>> GetProducts(
            [FromQuery] string search = "",
            [FromQuery] string category = "",
            [FromQuery] string brand = "",
            [FromQuery] string price = "",
            [FromQuery] string sort = "",
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 9,
            [FromQuery] bool promotion = false
        )
        {
            var (products, totalCount) = await _productService.GetAllAsync(search, category, brand, price, sort, page, pageSize, promotion);
            var categories = await _categoryService.GetAllAsync();
            var brands = await _brandService.GetAllAsync();
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            return Ok(new
            {
                products = products,
                categories = categories,
                brands = brands,
                totalProducts = totalCount,
                totalPages = totalPages
            });
        }

        [HttpGet("suggest")]
        public async Task<IActionResult> Suggest([FromQuery] string query)
        {
            var suggestions = await _productService.GetSuggestionsAsync(query);
            return Ok(suggestions);
        }

        [HttpGet("featured")]
        public async Task<ActionResult<IEnumerable<GetProductDTO>>> GetFeaturedProducts()
        {
            var products = await _productService.GetFeaturedAsync();
            return Ok(products);
        }

        [HttpGet("related/{slug}")]
        public async Task<ActionResult<IEnumerable<GetProductDTO>>> GetRelatedProducts(string slug)
        {
            var related = await _productService.GetRelatedAsync(slug);
            return Ok(related);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductResponseDTO>> GetProduct(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null)
                return NotFound();
            return Ok(product);
        }

        [HttpGet("slug/{slug}")]
        public async Task<ActionResult<ProductResponseDTO>> GetProductDetail(string slug)
        {
            var product = await _productService.GetBySlugAsync(slug);
            if (product == null)
                return NotFound();
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<ProductResponseDTO>> CreateProduct(CreateProductDTO product)
        {
            var createdProduct = await _productService.CreateAsync(product);
            return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.Id }, createdProduct);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, UpdateProductDTO product)
        {
            await _productService.UpdateAsync(id, product);
            return NoContent();
        }
    }
}