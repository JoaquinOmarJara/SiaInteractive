using AutoMapper;
using SiaInteractive.Application.DTOs;
using SiaInteractive.Application.Interfaces.Data;
using SiaInteractive.Application.Interfaces.Service;
using SiaInteractive.Domain.Entities;

namespace SiaInteractive.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDTO>> GetAllProductsAsync()
        {
            var products = await _unitOfWork.Products.GetAllAsync();
            return _mapper.Map<IEnumerable<ProductDTO>>(products);
        }

        public async Task<ProductDTO> GetProductByIdAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            return _mapper.Map<ProductDTO>(product);
        }

        public async Task<ProductDTO> CreateProductAsync(CreateProductDto createProductDto)
        {
            var productEntity = _mapper.Map<Product>(createProductDto);

            await _unitOfWork.Products.AddAsync(productEntity);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<ProductDTO>(productEntity);
        }

        public async Task UpdateProductAsync(int id, ProductDTO productDto)
        {
            var existingProduct = await _unitOfWork.Products.GetByIdAsync(id);

            if (existingProduct == null)
            {
                throw new KeyNotFoundException($"Producto con ID {id} no encontrado.");
            }

            _mapper.Map(productDto, existingProduct);

            _unitOfWork.Products.Update(existingProduct);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            var productToDelete = await _unitOfWork.Products.GetByIdAsync(id);

            if (productToDelete == null)
            {
                throw new KeyNotFoundException($"Producto con ID {id} no encontrado.");
            }

            _unitOfWork.Products.Delete(productToDelete);
            await _unitOfWork.CompleteAsync();
        }

        public async Task AddCategoryToProductAsync(int productId, int categoryId)
        {
            var productCategory = new ProductCategory
            {
                ProductID = productId,
                CategoryID = categoryId
            };

            await _unitOfWork.ProductCategories.AddAsync(productCategory);
            await _unitOfWork.CompleteAsync();
        }

        public async Task RemoveCategoryFromProductAsync(int productId, int categoryId)
        {
            var productCategoryToRemove = await _unitOfWork.ProductCategories
                .FindAsync(pc => pc.ProductID == productId && pc.CategoryID == categoryId);

            if (productCategoryToRemove == null)
            {
                throw new KeyNotFoundException("La relación entre el producto y la categoría no existe.");
            }

            _unitOfWork.ProductCategories.Delete(productCategoryToRemove);
            await _unitOfWork.CompleteAsync();
        }
    }
}
