using Microsoft.EntityFrameworkCore;
using PickleballStore.BLL.Services.Contracts;
using PickleballStore.BLL.ViewModels.Header;
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
                Logo = await _dbContext.Logos.OrderByDescending(x => x.CreatedAt).FirstOrDefaultAsync() ?? new Logo { LogoPath = "/images/default-logo.png" },
                SearchInfo = await _dbContext.SearchInfos.Include(s => s.QuickLinks).OrderByDescending(x => x.CreatedAt).FirstOrDefaultAsync() ?? new SearchInfo()
            };

            return headerViewModel;
        }
    }
}
