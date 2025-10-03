using PickleballStore.BLL.ViewModels.Address;

namespace PickleballStore.BLL.Services.Contracts
{
    public interface IAddressService
    {
        Task<List<AddressViewModel>> GetUserAddressesAsync(string userId);
        Task<AddressViewModel?> GetAddressByIdAsync(int id, string userId);
        Task<int> CreateAddressAsync(string userId, CreateAddressViewModel model);
        Task<bool> UpdateAddressAsync(int id, string userId, UpdateAddressViewModel model);
        Task<bool> DeleteAddressAsync(int id, string userId);
        Task<AddressViewModel?> GetDefaultAddressAsync(string userId);
        Task<bool> SetDefaultAddressAsync(int id, string userId);
    }
}
