using PickleballStore.DAL.DataContext.Entities;

namespace PickleballStore.DAL.Repositories.Contracts
{
    public interface ISliderRepository : IRepository<Slider>
    {
        Task<List<Slider>> GetSliderByIdAsync(int id);
    }
}
