using SiaInteractive.Application.DTOs;

namespace SiaInteractive.Application.Interfaces.Service
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDTO>> GetAllProductsAsync();
        Task<ProductDTO> GetProductByIdAsync(int id);
        Task<ProductDTO> CreateProductAsync(CreateProductDto productDto);
        Task UpdateProductAsync(int id, ProductDTO productDto);
        Task DeleteProductAsync(int id);
        Task AddCategoryToProductAsync(int productId, int categoryId);
        Task RemoveCategoryFromProductAsync(int productId, int categoryId);
    }
}
