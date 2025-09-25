using Microsoft.EntityFrameworkCore;
using PickleballStore.DAL.DataContext;
using PickleballStore.DAL.DataContext.Entities;
using PickleballStore.DAL.Repositories.Contracts;

namespace PickleballStore.DAL.Repositories
{
    public class SliderRepository : EfCoreRepository<Slider>, ISliderRepository
    {
        private readonly AppDbContext _dbContext;

        public SliderRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Slider>> GetSliderByIdAsync(int id)
        {
            return await _dbContext.Sliders.Include(s => s.Product) .Where(s => s.Id == id).ToListAsync();
        }
    }
}