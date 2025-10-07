using Microsoft.AspNetCore.Mvc;
using SiaInteractive.Application.DTOs;
using SiaInteractive.Application.Interfaces.Service;

namespace SiaInteractive.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductDto createProductDto)
        {
            var newProduct = await _productService.CreateProductAsync(createProductDto);
            return CreatedAtAction(nameof(GetProduct), new { id = newProduct.ProductID }, newProduct);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, ProductDTO productDto)
        {
            if (id != productDto.ProductID) return BadRequest();
            await _productService.UpdateProductAsync(id, productDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _productService.DeleteProductAsync(id);
            return NoContent();
        }

        // Endpoint para gestionar la relación N-N
        [HttpPost("{productId}/categories/{categoryId}")]
        public async Task<IActionResult> AddCategoryToProduct(int productId, int categoryId)
        {
            await _productService.AddCategoryToProductAsync(productId, categoryId);
            return Ok();
        }
    }
}
