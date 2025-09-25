using AutoMapper;
using PickleballStore.BLL.Services.Contracts;
using PickleballStore.BLL.ViewModels.Slider;
using PickleballStore.DAL.DataContext.Entities;
using PickleballStore.DAL.Repositories.Contracts;

namespace PickleballStore.BLL.Services
{
    public class SliderManager : CrudManager<Slider, SliderViewModel, CreateSliderViewModel, UpdateSliderViewModel>, ISliderService
    {
        private readonly ISliderRepository _repository;
        private readonly IMapper _mapper;

        public SliderManager(ISliderRepository repository, IMapper mapper) : base(repository, mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<SliderViewModel>> GetSlidersByIdAsync(int id)
        {
            var Sliders = await _repository.GetSliderByIdAsync(id);
            return _mapper.Map<List<SliderViewModel>>(Sliders);
        }
    }
}
