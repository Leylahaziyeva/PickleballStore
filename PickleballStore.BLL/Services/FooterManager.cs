using Microsoft.EntityFrameworkCore;
using PickleballStore.BLL.Services.Contracts;
using PickleballStore.BLL.ViewModels.Footer;
using PickleballStore.DAL.DataContext;
using PickleballStore.DAL.DataContext.Entities;

namespace PickleballStore.BLL.Services
{
    public class FooterManager : IFooterService
    {
        private readonly AppDbContext _dbContext;

        public FooterManager(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<FooterViewModel> GetFooterAsync()
        {
            var footerViewModel = new FooterViewModel
            {
                Logo = await _dbContext.Logos.OrderByDescending(x => x.CreatedAt).FirstOrDefaultAsync() ?? new Logo { LogoPath = "/images/default-logo.png" },

                SocialLinks = await _dbContext.SocialLinks.ToListAsync(),

                SubscribeForm = await _dbContext.SubscribeForms.OrderByDescending(x => x.CreatedAt).FirstOrDefaultAsync()
                ?? new SubscribeForm { Title = "Join Us", Description = "Subscribe to", Icon = "fa-envelope" },

                Bottom = await _dbContext.FooterBottoms.OrderByDescending(x => x.CreatedAt).FirstOrDefaultAsync() ?? new FooterBottom { Copyright = "© 2025", PaymentIcons = "#" }
            };

            return footerViewModel;
        }
    }
}
