using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PickleballStore.DAL.DataContext.Entities;

namespace PickleballStore.DAL.DataContext
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<ProductImage> ProductImages { get; set; } = null!;
        public DbSet<ProductVariant> ProductVariants { get; set; } = null!;
        public DbSet<Logo> Logos { get; set; } = null!;
        public DbSet<FooterBottom> FooterBottoms { get; set; } = null!;
        public DbSet<PaymentMethod> PaymentMethods { get; set; } = null!;
        public DbSet<SocialLink> SocialLinks { get; set; } = null!;
        public DbSet<FooterInfo> FooterInfos { get; set; } = null!;
        public DbSet<NewsletterSubscription> NewsletterSubscriptions { get; set; } = null!;
        public DbSet<Currency> Currencies { get; set; } = null!;
        public DbSet<Language> Languages { get; set; } = null!;
        public DbSet<Slider> Sliders { get; set; } = null!;
        public DbSet<SearchSection> SearchSections { get; set; } = null!;
        public DbSet<SearchQuickLink> SearchQuickLinks { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Product>()
               .Property(p => p.Price)
               .HasPrecision(18, 2);

            base.OnModelCreating(builder);
        }

        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimestamps()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is TimeStample &&
                           (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                var timestamp = (TimeStample)entry.Entity;

                if (entry.State == EntityState.Added)
                {
                    timestamp.CreatedAt = DateTime.UtcNow;
                }

                timestamp.UpdatedAt = DateTime.UtcNow;
            }
        }
    }
}
