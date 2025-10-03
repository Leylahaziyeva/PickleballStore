using PickleballStore.BLL.ViewModels.Account;

namespace PickleballStore.BLL.Services.Contracts
{
    public interface IWishlistService
    {
        Task<List<WishlistItemViewModel>> GetUserWishlistAsync(string userId);
        Task<bool> RemoveFromWishlistAsync(int wishlistItemId, string userId);
        Task<bool> IsInWishlistAsync(string userId, int productId);
        Task<int> GetWishlistCountAsync(string userId);
        Task<bool> AddToWishlistAsync(int productId, string userId);
    }
}
