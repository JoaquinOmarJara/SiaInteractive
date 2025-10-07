using SiaInteractive.Domain.Entities;

namespace SiaInteractive.Application.Interfaces.Data
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Product> Products { get; }
        IRepository<Category> Categories { get; }
        IRepository<ProductCategory> ProductCategories { get; }
        Task<int> CompleteAsync();
    }
}
