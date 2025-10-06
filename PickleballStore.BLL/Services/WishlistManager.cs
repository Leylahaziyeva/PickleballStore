using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PickleballStore.BLL.Services.Contracts;
using PickleballStore.BLL.ViewModels.Wishlist;
using PickleballStore.DAL.DataContext.Entities;
using PickleballStore.DAL.Repositories.Contracts;

namespace PickleballStore.BLL.Services
{
    public class WishlistManager : CrudManager<WishlistItem, WishlistViewModel, WishlistCreateViewModel, WishlistUpdateViewModel>, IWishlistService
    {
        public WishlistManager(IWishlistRepository repository, IMapper mapper)
      : base(repository, mapper)
        {
        }

        public async Task<IEnumerable<WishlistViewModel>> GetUserWishlistAsync(string? userId)
        {
            var items = await Repository.GetAllAsync(
                predicate: w => w.UserId == userId && !w.IsDeleted,
                include: q => q.Include(w => w.Product)
                              .ThenInclude(p => p!.Images)
                              .Include(w => w.Product)
                              .ThenInclude(p => p!.Variants)
                              .Include(w => w.Product)
                              .ThenInclude(p => p!.Category!),
                orderBy: q => q.OrderByDescending(w => w.CreatedAt),
                AsNoTracking: true
            );

            return Mapper.Map<IEnumerable<WishlistViewModel>>(items);
        }

        public async Task<bool> IsProductInWishlistAsync(int productId, string? userId)
        {
            var item = await Repository.GetAsync(
                predicate: w => w.ProductId == productId && w.UserId == userId && !w.IsDeleted,
                AsNoTracking: true
            );

            return item != null;
        }

        public async Task<bool> ToggleWishlistAsync(int productId, string? userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new InvalidOperationException("User must be logged in to use wishlist");
            }

            var existing = await Repository.GetAsync(
                predicate: w => w.ProductId == productId && w.UserId == userId && !w.IsDeleted
            );

            if (existing != null)
            {
                existing.IsDeleted = true;
                existing.UpdatedAt = DateTime.UtcNow;
                await Repository.UpdateAsync(existing);
                return true;
            }
            else
            {
                var newItem = new WishlistItem
                {
                    ProductId = productId,
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };
                await Repository.CreateAsync(newItem);
                return true;
            }
        }

        public async Task<bool> RemoveFromWishlistAsync(int productId, string? userId)
        {
            var item = await Repository.GetAsync(
                predicate: w => w.ProductId == productId && w.UserId == userId && !w.IsDeleted
            );

            if (item == null)
                return false;

            item.IsDeleted = true;
            item.UpdatedAt = DateTime.Now;
            await Repository.UpdateAsync(item);
            return true;
        }

        public async Task<int> GetWishlistCountAsync(string? userId)
        {
            var items = await Repository.GetAllAsync(
                predicate: w => w.UserId == userId && !w.IsDeleted,
                AsNoTracking: true
            );

            return items.Count;
        }
    }
}