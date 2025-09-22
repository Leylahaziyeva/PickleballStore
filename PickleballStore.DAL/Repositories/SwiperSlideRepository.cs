using Microsoft.EntityFrameworkCore;
using PickleballStore.DAL.DataContext;
using PickleballStore.DAL.DataContext.Entities;
using PickleballStore.DAL.Repositories.Contracts;

namespace PickleballStore.DAL.Repositories
{
    public class SwiperSlideRepository : EfCoreRepository<SwiperSlide>, ISwiperSlideRepository
    {
        private readonly AppDbContext _dbContext;

        public SwiperSlideRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<SwiperSlide>> GetSliderByIdAsync(int id)
        {
            return await _dbContext.SwiperSlides
                .Where(s => s.Id == id)
                .ToListAsync();
        }
    }
}