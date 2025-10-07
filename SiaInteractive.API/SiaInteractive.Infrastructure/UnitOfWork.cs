using SiaInteractive.Application.Interfaces.Data;
using SiaInteractive.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiaInteractive.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreDbContext _context;
        public IRepository<Product> Products { get; }
        public IRepository<Category> Categories { get; }
        public IRepository<ProductCategory> ProductCategories { get; }

        public UnitOfWork(StoreDbContext context)
        {
            _context = context;
            Products = new Repository<Product>(_context);
            Categories = new Repository<Category>(_context);
            ProductCategories = new Repository<ProductCategory>(_context);
        }

        public Task<int> CompleteAsync() => _context.SaveChangesAsync();
        public void Dispose() => _context.Dispose();
    }
}
