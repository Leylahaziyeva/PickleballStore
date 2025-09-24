using AutoMapper;
using PickleballStore.BLL.Services.Contracts;
using PickleballStore.BLL.ViewModels.ProductVariant;
using PickleballStore.DAL.DataContext.Entities;
using PickleballStore.DAL.Repositories.Contracts;

namespace PickleballStore.BLL.Services
{
    public class ProductVariantManager : CrudManager<ProductVariant, ProductVariantViewModel, CreateProductVariantViewModel, UpdateProductVariantViewModel>, IProductVariantService
    {
        private readonly IProductVariantRepository _repository;
        private readonly IMapper _mapper;

        public ProductVariantManager(IProductVariantRepository repository, IMapper mapper) : base(repository, mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<ProductVariantViewModel>> GetVariantsByProductIdAsync(int productId)
        {
            var variants = await _repository.GetVariantsByProductIdAsync(productId);
            return _mapper.Map<List<ProductVariantViewModel>>(variants);
        }
    }
}
