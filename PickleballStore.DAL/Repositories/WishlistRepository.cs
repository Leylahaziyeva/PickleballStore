using PickleballStore.DAL.DataContext;
using PickleballStore.DAL.DataContext.Entities;
using PickleballStore.DAL.Repositories.Contracts;

namespace PickleballStore.DAL.Repositories
{
    public class WishlistRepository : EfCoreRepository<WishlistItem>, IWishlistRepository
    {
        public WishlistRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}