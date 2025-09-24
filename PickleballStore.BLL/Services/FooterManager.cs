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
            var footerInfo = await _dbContext.FooterInfos
                .Include(f => f.Logo)
                .Include(f => f.SocialLinks)
                .OrderByDescending(f => f.CreatedAt)
                .FirstOrDefaultAsync();

            var footerBottom = await _dbContext.FooterBottoms
                .Include(f => f.PaymentMethods)
                .OrderByDescending(f => f.CreatedAt)
                .FirstOrDefaultAsync();

            var currencies = await _dbContext.Currencies.ToListAsync();
            var languages = await _dbContext.Languages.ToListAsync();

            // Get newsletter subscription (not used for text, just for form binding)
            var newsletter = new NewsletterSubscription();

            var viewModel = new FooterViewModel
            {
                Logo = footerInfo?.Logo ?? new Logo { LogoPath = "/images/default-logo.png" },
                FooterInfo = footerInfo ?? new FooterInfo
                {
                    Address = "1234 Fashion Street, Suite 567, New York, NY 10001",
                    Email = "info@example.com",
                    Phone = "(000) 000-0000"
                },
                SocialLinks = footerInfo?.SocialLinks?.ToList() ?? new List<SocialLink>(),

                NewsletterSubscription = newsletter,

                Currencies = currencies,
                Languages = languages,

                FooterBottom = footerBottom ?? new FooterBottom
                {
                    CopyrightText = $"© {DateTime.UtcNow.Year} Ecomus Store. All Rights Reserved"
                },
                PaymentMethods = footerBottom?.PaymentMethods?.ToList() ?? new List<PaymentMethod>()
            };

            return viewModel;
        }
    }
}
