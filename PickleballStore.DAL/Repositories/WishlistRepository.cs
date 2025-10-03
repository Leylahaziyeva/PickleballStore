using Microsoft.EntityFrameworkCore;
using PickleballStore.DAL.DataContext;
using PickleballStore.DAL.DataContext.Entities;
using PickleballStore.DAL.Repositories.Contracts;

namespace PickleballStore.DAL.Repositories
{
    public class WishlistRepository : EfCoreRepository<WishlistItem>, IWishlistRepository
    {

        private readonly AppDbContext _dbContext; 

        public WishlistRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext; 
        }

        public async Task<List<WishlistItem>> GetUserWishlistAsync(string userId)
        {
            return await GetAllAsync(
                predicate: w => w.UserId == userId && !w.IsDeleted
                //orderBy: q => q.OrderByDescending(w => w.AddedAt)
            ) as List<WishlistItem> ?? new List<WishlistItem>();
        }

        public async Task<WishlistItem?> GetWishlistItemByIdAsync(int itemId, string userId)
        {
            return await GetAsync(
                predicate: w => w.Id == itemId && w.UserId == userId && !w.IsDeleted
            );
        }

        public async Task<WishlistItem?> GetByProductIdAsync(string userId, int productId)
        {
            return await GetAsync(
                predicate: w => w.UserId == userId && w.ProductId == productId && !w.IsDeleted
            );
        }

        public async Task<bool> IsInWishlistAsync(string userId, int productId)
        {
            var item = await GetByProductIdAsync(userId, productId);
            return item != null;
        }

        public async Task<int> GetWishlistCountAsync(string userId)
        {
            var items = await GetUserWishlistAsync(userId);
            return items.Count;
        }
        public async Task AddAsync(WishlistItem item)
        {
            await _dbContext.WishlistItems.AddAsync(item);
            await _dbContext.SaveChangesAsync();
        }
    }
}
