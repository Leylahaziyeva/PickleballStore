using PickleballStore.DAL.DataContext.Entities;

namespace PickleballStore.DAL.Repositories.Contracts
{
    public interface IWishlistRepository : IRepository<WishlistItem>
    {
        Task<List<WishlistItem>> GetUserWishlistAsync(string userId);
        Task<WishlistItem?> GetWishlistItemByIdAsync(int itemId, string userId);
        Task<WishlistItem?> GetByProductIdAsync(string userId, int productId);
        Task<bool> IsInWishlistAsync(string userId, int productId);
        Task<int> GetWishlistCountAsync(string userId);
        Task AddAsync(WishlistItem item);
    }
}
