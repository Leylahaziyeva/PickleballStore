using AutoMapper;
using PickleballStore.BLL.Services.Contracts;
using PickleballStore.BLL.ViewModels.Account;
using PickleballStore.DAL.Repositories.Contracts;

namespace PickleballStore.BLL.Services
{
    public class WishlistManager : IWishlistService
    {
        private readonly IWishlistRepository _wishlistRepository;
        private readonly IProductRepository _productRepository; 
        private readonly IMapper _mapper;

        public WishlistManager(IWishlistRepository wishlistRepository, IProductRepository productRepository, IMapper mapper)
        {
            _wishlistRepository = wishlistRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public Task<bool> AddToWishlistAsync(int productId, string userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<WishlistItemViewModel>> GetUserWishlistAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetWishlistCountAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsInWishlistAsync(string userId, int productId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveFromWishlistAsync(int wishlistItemId, string userId)
        {
            throw new NotImplementedException();
        }
    }
}
