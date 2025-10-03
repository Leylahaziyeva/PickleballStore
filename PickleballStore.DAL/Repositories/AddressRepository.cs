using PickleballStore.DAL.DataContext;
using PickleballStore.DAL.DataContext.Entities;
using PickleballStore.DAL.Repositories.Contracts;

namespace PickleballStore.DAL.Repositories
{
    public class AddressRepository : EfCoreRepository<Address>, IAddressRepository
    {
        public AddressRepository(AppDbContext dbContext) : base(dbContext) { }

        public async Task<List<Address>> GetUserAddressesAsync(string userId)
        {
            return await GetAllAsync(
                predicate: a => a.UserId == userId && !a.IsDeleted
                //orderBy: q => q.OrderByDescending(a => a.IsDefault)
            ) as List<Address> ?? new List<Address>();
        }

        public async Task<Address?> GetDefaultAddressAsync(string userId)
        {
            return await GetAsync(
                predicate: a => a.UserId == userId && !a.IsDeleted
            );
        }

        public async Task<Address?> GetUserAddressByIdAsync(int addressId, string userId)
        {
            return await GetAsync(
                predicate: a => a.Id == addressId && a.UserId == userId && !a.IsDeleted
            );
        }

        public async Task UnsetAllDefaultAddressesAsync(string userId)
        {
            var addresses = await GetAllAsync(
                predicate: a => a.UserId == userId && !a.IsDeleted
            );

            await DbContext.SaveChangesAsync();
        }
    }

}
