using PickleballStore.BLL.ViewModels.Shop;

namespace PickleballStore.BLL.Services.Contracts
{
    public interface IShopService
    {
        Task<ShopViewModel> GetShopViewModelAsync();
    }
}
