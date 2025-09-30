using Microsoft.EntityFrameworkCore;
using PickleballStore.BLL.Services.Contracts;
using PickleballStore.BLL.ViewModels.Product;
using PickleballStore.BLL.ViewModels.Search;
using PickleballStore.DAL.DataContext;

namespace PickleballStore.Services
{
    public class SearchManager : ISearchService
    {
        private readonly AppDbContext _dbContext;

        public SearchManager(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<SearchViewModel> GetSearchDataAsync()
        {
            return new SearchViewModel
            {
                SearchSection = await _dbContext.SearchSections
                    .Include(s => s.QuickLinks)
                    .OrderByDescending(x => x.CreatedAt)
                    .FirstOrDefaultAsync(),

                InspirationProducts = await _dbContext.Products
                    .OrderByDescending(x => x.CreatedAt)
                    .Take(3)
                    .Select(p => new ProductViewModel
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Price = p.Price,
                        PriceBeforeDiscount = p.PriceBeforeDiscount,
                        CoverImageName = p.CoverImageName
                    })
                    .ToListAsync()
            };
        }
    }
}