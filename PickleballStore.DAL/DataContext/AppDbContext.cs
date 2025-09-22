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
        //public DbSet<ProductImage> ProductImages { get; set; } = null!;
        //public DbSet<ProductTag> ProductTags { get; set; } = null!;
        //public DbSet<Review> Reviews { get; set; } = null!;
        //public DbSet<Tag> Tags { get; set; } = null!;
        public DbSet<ContactInfo> ContactInfos { get; set; } = null!;
        public DbSet<FooterBottom> FooterBottoms { get; set; } = null!;
        public DbSet<Logo> Logos { get; set; } = null!;
        public DbSet<SubscribeForm> SubscribeForms { get; set; } = null!;
        public DbSet<SocialLink> SocialLinks { get; set; } = null!;
        public DbSet<SwiperSlide> SwiperSlides { get; set; } = null!;
        public DbSet<SearchInfo> SearchInfos { get; set; } = null!;

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
