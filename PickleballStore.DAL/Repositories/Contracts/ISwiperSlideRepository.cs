using PickleballStore.DAL.DataContext.Entities;

namespace PickleballStore.DAL.Repositories.Contracts
{
    public interface ISwiperSlideRepository : IRepository<SwiperSlide>
    {
        Task<List<SwiperSlide>> GetSliderByIdAsync(int id);
    }
}
