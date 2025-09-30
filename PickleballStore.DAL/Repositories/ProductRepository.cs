using Microsoft.EntityFrameworkCore;
using PickleballStore.DAL.DataContext;
using PickleballStore.DAL.DataContext.Entities;
using PickleballStore.DAL.Repositories.Contracts;

namespace PickleballStore.DAL.Repositories
{
    public class ProductRepository : EfCoreRepository<Product>, IProductRepository
    {
        private readonly AppDbContext _dbContext;

        public ProductRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Product?> GetByIdWithDetailsAsync(int id)
        {
            return await _dbContext.Products
           .Include(p => p.Variants) 
           .Include(p => p.Images)    
           .Include(p => p.Category)
           .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
