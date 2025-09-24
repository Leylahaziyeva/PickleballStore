using Microsoft.EntityFrameworkCore;
using PickleballStore.DAL.DataContext;
using PickleballStore.DAL.DataContext.Entities;
using PickleballStore.DAL.Repositories.Contracts;

namespace PickleballStore.DAL.Repositories
{
    public class ProductVariantRepository : EfCoreRepository<ProductVariant>, IProductVariantRepository
    {
        private readonly AppDbContext _dbContext;
        public ProductVariantRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<ProductVariant>> GetVariantsByProductIdAsync(int productId)
        {
            return await _dbContext.ProductVariants
                .Where(v => v.ProductId == productId)
                .ToListAsync();
        }
    }
}
