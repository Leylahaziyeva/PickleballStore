using PickleballStore.BLL.ViewModels.Slider;
using PickleballStore.DAL.DataContext.Entities;

namespace PickleballStore.BLL.Services.Contracts
{
    public interface ISliderService : ICrudService<Slider, SliderViewModel, CreateSliderViewModel, UpdateSliderViewModel>
    {
        Task<List<SliderViewModel>> GetSlidersByIdAsync(int id);
    }
}
