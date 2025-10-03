using AutoMapper;
using PickleballStore.BLL.Services.Contracts;
using PickleballStore.BLL.ViewModels.Address;
using PickleballStore.DAL.DataContext.Entities;
using PickleballStore.DAL.Repositories.Contracts;

namespace PickleballStore.BLL.Services
{
    public class AddressManager : IAddressService
    {
        private readonly IRepository<Address> _repository;
        private readonly IMapper _mapper;

        public AddressManager(IRepository<Address> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<AddressViewModel>> GetUserAddressesAsync(string userId)
        {
            var addresses = await _repository.GetAllAsync(
                predicate: a => a.UserId == userId,
                orderBy: q => q.OrderByDescending(a => a.IsDefault).ThenByDescending(a => a.CreatedAt)
            );

            return _mapper.Map<List<AddressViewModel>>(addresses);
        }

        public async Task<AddressViewModel?> GetAddressByIdAsync(int id, string userId)
        {
            var address = await _repository.GetAsync(
                predicate: a => a.Id == id && a.UserId == userId
            );

            return _mapper.Map<AddressViewModel>(address);
        }

        public async Task<int> CreateAddressAsync(string userId, CreateAddressViewModel model)
        {
            var address = _mapper.Map<Address>(model);
            address.UserId = userId;

            var hasAddresses = await _repository.GetAllAsync(a => a.UserId == userId);
            if (!hasAddresses.Any())
            {
                address.IsDefault = true;
            }

            if (address.IsDefault)
            {
                var defaults = await _repository.GetAllAsync(a => a.UserId == userId && a.IsDefault);
                foreach (var existing in defaults)
                {
                    existing.IsDefault = false;
                    await _repository.UpdateAsync(existing);
                }
            }

            await _repository.CreateAsync(address);
            return address.Id;
        }

        public async Task<bool> UpdateAddressAsync(int id, string userId, UpdateAddressViewModel model)
        {
            var address = await _repository.GetAsync(a => a.Id == id && a.UserId == userId);

            if (address == null)
                return false;

            _mapper.Map(model, address);

            if (model.IsDefault && !address.IsDefault)
            {
                var otherAddresses = await _repository.GetAllAsync(
                    predicate: a => a.UserId == userId && a.Id != id && a.IsDefault
                );

                foreach (var other in otherAddresses)
                {
                    other.IsDefault = false;
                    await _repository.UpdateAsync(other);
                }
            }

            await _repository.UpdateAsync(address);
            return true;
        }

        public async Task<bool> DeleteAddressAsync(int id, string userId)
        {
            var address = await _repository.GetAsync(a => a.Id == id && a.UserId == userId);

            if (address == null)
                return false;

            var wasDefault = address.IsDefault;

            await _repository.DeleteAsync(address);

            if (wasDefault)
            {
                var remainingAddresses = await _repository.GetAllAsync(
                    predicate: a => a.UserId == userId,
                    orderBy: q => q.OrderByDescending(a => a.CreatedAt)
                );

                var nextDefault = remainingAddresses.FirstOrDefault();
                if (nextDefault != null)
                {
                    nextDefault.IsDefault = true;
                    await _repository.UpdateAsync(nextDefault);
                }
            }

            return true;
        }

        public async Task<AddressViewModel?> GetDefaultAddressAsync(string userId)
        {
            var address = await _repository.GetAsync(
                predicate: a => a.UserId == userId && a.IsDefault
            );

            if (address == null)
                return null;

            return _mapper.Map<AddressViewModel>(address);
        }

        public async Task<bool> SetDefaultAddressAsync(int id, string userId)
        {
            var address = await _repository.GetAsync(a => a.Id == id && a.UserId == userId);
            if (address == null) return false;

            var allAddresses = await _repository.GetAllAsync(a => a.UserId == userId);
            foreach (var addr in allAddresses)
            {
                addr.IsDefault = (addr.Id == id);
                await _repository.UpdateAsync(addr);
            }

            return true;
        }
    }
}