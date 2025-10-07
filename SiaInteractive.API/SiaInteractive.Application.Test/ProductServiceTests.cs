using static System.Formats.Asn1.AsnWriter;

namespace SiaInteractive.Application.Test
{
    using AutoMapper;
    using FluentAssertions;
    using Moq;
    using SiaInteractive.Application.DTOs;
    using SiaInteractive.Application.Interfaces.Data;
    using SiaInteractive.Application.Services;
    using SiaInteractive.Domain.Entities;
    using Xunit;

    public class ProductServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IRepository<Product>> _mockProductRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly ProductService _sut; // System Under Test (Sistema Bajo Prueba)

        public ProductServiceTests()
        {
            // ARRANGE Global: Preparamos los mocks que usaremos en todas las pruebas.
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockProductRepo = new Mock<IRepository<Product>>();
            _mockMapper = new Mock<IMapper>();

            // Configuramos el mock de UnitOfWork para que devuelva el mock del repositorio de productos.
            _mockUnitOfWork.Setup(uow => uow.Products).Returns(_mockProductRepo.Object);

            // Creamos la instancia del servicio que vamos a probar, inyectando los mocks.
            _sut = new ProductService(_mockUnitOfWork.Object, _mockMapper.Object);
        }

        // --- Pruebas para GetProductByIdAsync ---

        [Fact]
        public async Task GetProductByIdAsync_WhenProductExists_ShouldReturnProductDto()
        {
            // Arrange: Preparamos el escenario específico para esta prueba.
            var product = new Product { ProductID = 1, Name = "Laptop" };
            var productDto = new ProductDTO { ProductID = 1, Name = "Laptop" };

            _mockProductRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(product);
            _mockMapper.Setup(m => m.Map<ProductDTO>(product)).Returns(productDto);

            // Act: Ejecutamos el método que queremos probar.
            var result = await _sut.GetProductByIdAsync(1);

            // Assert: Verificamos que el resultado sea el esperado.
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(productDto); // Compara todas las propiedades.
        }

        [Fact]
        public async Task GetProductByIdAsync_WhenProductDoesNotExist_ShouldReturnNull()
        {
            // Arrange
            _mockProductRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Product)null);

            // Act
            var result = await _sut.GetProductByIdAsync(99); // ID que no existe

            // Assert
            result.Should().BeNull();
        }

        // --- Pruebas para CreateProductAsync ---

        [Fact]
        public async Task CreateProductAsync_WithValidData_ShouldReturnCreatedProductDto()
        {
            // Arrange
            var createDto = new CreateProductDto { Name = "Nuevo Producto", Description = "Desc" };
            var product = new Product { ProductID = 0, Name = "Nuevo Producto", Description = "Desc" };
            var resultDto = new ProductDTO { ProductID = 1, Name = "Nuevo Producto", Description = "Desc" };

            _mockMapper.Setup(m => m.Map<Product>(createDto)).Returns(product);
            _mockMapper.Setup(m => m.Map<ProductDTO>(product)).Returns(resultDto);
            _mockUnitOfWork.Setup(uow => uow.CompleteAsync()).ReturnsAsync(1);

            // Act
            var result = await _sut.CreateProductAsync(createDto);

            // Assert
            result.Should().NotBeNull();
            result.ProductID.Should().Be(1);
            _mockProductRepo.Verify(repo => repo.AddAsync(It.IsAny<Product>()), Times.Once); // Verificamos que se llamó al método AddAsync
            _mockUnitOfWork.Verify(uow => uow.CompleteAsync(), Times.Once); // Verificamos que se guardaron los cambios
        }

        // --- Pruebas para DeleteProductAsync ---

        [Fact]
        public async Task DeleteProductAsync_WhenProductExists_ShouldCompleteSuccessfully()
        {
            // Arrange
            var product = new Product { ProductID = 1, Name = "Producto a Borrar" };
            _mockProductRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(product);

            // Act
            // Usamos una función anónima para poder capturar excepciones si las hubiera
            Func<Task> act = async () => await _sut.DeleteProductAsync(1);

            // Assert
            await act.Should().NotThrowAsync(); // Verificamos que no lanza ninguna excepción.
            _mockProductRepo.Verify(repo => repo.Delete(product), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteProductAsync_WhenProductDoesNotExist_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            _mockProductRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Product)null);

            // Act
            Func<Task> act = async () => await _sut.DeleteProductAsync(99);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>(); // Verificamos que lanza la excepción esperada.
        }
    }
}