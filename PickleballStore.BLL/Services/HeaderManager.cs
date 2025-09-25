using Microsoft.EntityFrameworkCore;
using PickleballStore.BLL.Services.Contracts;
using PickleballStore.BLL.ViewModels.Header;
using PickleballStore.BLL.ViewModels.Product;
using PickleballStore.DAL.DataContext;
using PickleballStore.DAL.DataContext.Entities;

namespace PickleballStore.BLL.Services
{
    public class HeaderManager : IHeaderService
    {
        private readonly AppDbContext _dbContext;

        public HeaderManager(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<HeaderViewModel> GetHeaderAsync()
        {
            var headerViewModel = new HeaderViewModel
            {
                Logo = await _dbContext.Logos
                    .OrderByDescending(x => x.CreatedAt)
                    .FirstOrDefaultAsync()
                    ?? new Logo { LogoPath = "/images/default-logo.png" },

                SearchSection = await _dbContext.SearchSections
                    .Include(s => s.QuickLinks)
                    .OrderByDescending(x => x.CreatedAt)
                    .FirstOrDefaultAsync()
                    ?? new SearchSection(),

                InspirationProducts = await _dbContext.Products
                    .OrderByDescending(x => x.CreatedAt).Take(3)
                    .Select(p => new ProductViewModel
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Description = p.Description,
                        Price = p.Price,
                        PriceBeforeDiscount = p.PriceBeforeDiscount,
                        CoverImageName = p.CoverImageName,
                        CategoryId = p.CategoryId,
                        CategoryName = p.Category!.Name
                    })
                    .ToListAsync()
            };

            return headerViewModel;
        }
    }

}
