using PickleballStore.BLL.ViewModels.Wishlist;
using PickleballStore.DAL.DataContext.Entities;

namespace PickleballStore.BLL.Services.Contracts
{
    public interface IWishlistService : ICrudService<WishlistItem, WishlistViewModel, WishlistCreateViewModel, WishlistUpdateViewModel>
    {
        Task<IEnumerable<WishlistViewModel>> GetUserWishlistAsync(string? userId);
        Task<bool> IsProductInWishlistAsync(int productId, string? userId);
        Task<bool> ToggleWishlistAsync(int productId, string? userId);
        Task<bool> RemoveFromWishlistAsync(int productId, string? userId);
        Task<int> GetWishlistCountAsync(string? userId);
    }
}