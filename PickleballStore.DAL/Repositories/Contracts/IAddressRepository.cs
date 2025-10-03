using PickleballStore.DAL.DataContext.Entities;

namespace PickleballStore.DAL.Repositories.Contracts
{
    public interface IAddressRepository : IRepository<Address>
    {
        Task<List<Address>> GetUserAddressesAsync(string userId);
        Task<Address?> GetDefaultAddressAsync(string userId);
        Task<Address?> GetUserAddressByIdAsync(int addressId, string userId);
        Task UnsetAllDefaultAddressesAsync(string userId);
    }
}
