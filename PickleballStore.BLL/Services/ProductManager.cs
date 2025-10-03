using AutoMapper;
using PickleballStore.BLL.Services.Contracts;
using PickleballStore.BLL.ViewModels.Product;
using PickleballStore.DAL.DataContext.Entities;
using PickleballStore.DAL.Repositories.Contracts;

namespace PickleballStore.BLL.Services
{
    public class ProductManager : CrudManager<Product, ProductViewModel, CreateProductViewModel, UpdateProductViewModel>, IProductService
    {
        private readonly IProductRepository _repository;
        private readonly ICategoryService _categoryService;
        private readonly FileService _fileService;
        private readonly IMapper _mapper;

        public ProductManager(IProductRepository repository, ICategoryService categoryService, IMapper mapper, FileService fileService)
            : base(repository, mapper)
        {
            _repository = repository;
            _categoryService = categoryService;
            _mapper = mapper;
            _fileService = fileService;
        }

        public async Task<ProductViewModel?> GetByIdWithDetailsAsync(int id)
        {
            var product = await _repository.GetByIdWithDetailsAsync(id);

            if (product == null) return null;

            var productViewModel = _mapper.Map<ProductViewModel>(product);

            productViewModel.CategoryName = product.Category?.Name;

            return productViewModel;
        }

        public async Task<List<ProductViewModel>> GetRelatedProductsAsync(int categoryId, int id)
        {
            var products = await _repository.GetProductsByCategoryAsync(categoryId, id);

            var relatedProductsViewModel = _mapper.Map<List<ProductViewModel>>(products);

            return relatedProductsViewModel;
        }


        //public async Task<CreateProductViewModel> GetCreateProductViewModelAsync()
        //{
        //    var createProductViewModel = new CreateProductViewModel();

        //    createProductViewModel.CategorySelectListItems = await _categoryService.GetCategorySelectListItemsAsync();
        //    createProductViewModel.TagSelectListItems = await _tagService.GetTagSelectListItemsAsync();

        //    return createProductViewModel;
        //}

        //public async Task<UpdateProductViewModel> GetUpdateViewModelAsync(int id)
        //{
        //    var product = await Repository.GetAsync(
        //        predicate: p => p.Id == id,
        //        include: source => source
        //            .Include(p => p.Images!)
        //            .Include(p => p.Category!)
        //            .Include(p => p.ProductTags!)
        //                .ThenInclude(pt => pt.Tag!));

        //    if (product == null) return null!;

        //    var updateProductViewModel = Mapper.Map<UpdateProductViewModel>(product);
        //    updateProductViewModel.CategorySelectListItems = await _categoryService.GetCategorySelectListItemsAsync();

        //    var tagSelectListItems = await _tagService.GetTagSelectListItemsAsync();
        //    var newTagSelectListItems = new List<SelectListItem>();
        //    if (tagSelectListItems != null && tagSelectListItems.Any())
        //    {
        //        foreach (var tag in tagSelectListItems)
        //        {
        //            if (!product.ProductTags.Any(pt => pt.TagId == int.Parse(tag.Value)))
        //            {
        //                newTagSelectListItems.Add(tag);
        //            }
        //        }
        //    }

        //    updateProductViewModel.TagSelectListItems = newTagSelectListItems;

        //    return updateProductViewModel;
        //}

        //public override async Task CreateAsync(CreateProductViewModel model)
        //{
        //    var product = Mapper.Map<Product>(model);

        //    if (model.TagIds != null && model.TagIds.Any())
        //    {
        //        product.ProductTags = model.TagIds.Select(tagId => new ProductTag
        //        {
        //            TagId = tagId
        //        }).ToList();
        //    }

        //    if (model.CoverImageFile != null)
        //    {
        //        if (!_fileService.IsImageFile(model.CoverImageFile))
        //            throw new ArgumentException("The file is not a valid image.", nameof(model.CoverImageFile));

        //        product.CoverImageName = await _fileService.GenerateFile(model.CoverImageFile, FilePathConstants.ProductImagePath);
        //    }

        //    if (model.ImageFiles != null && model.ImageFiles.Any())
        //    {
        //        product.Images = new List<ProductImage>();
        //        foreach (var imageFile in model.ImageFiles)
        //        {
        //            if (!_fileService.IsImageFile(imageFile))
        //                throw new ArgumentException("One of the files is not a valid image.", nameof(model.ImageFiles));
        //        }

        //        foreach (var imageFile in model.ImageFiles)
        //        {
        //            var imageName = await _fileService.GenerateFile(imageFile, FilePathConstants.ProductImagePath);
        //            product.Images.Add(new ProductImage
        //            {
        //                ImageName = imageName
        //            });
        //        }
        //    }

        //    await Repository.CreateAsync(product);
        //}

        //public override async Task<bool> UpdateAsync(int id, UpdateProductViewModel model)
        //{
        //    var existingProduct = await Repository.GetByIdAsync(id);

        //    if (existingProduct == null) return false;

        //    var existingProducts = Mapper.Map<Product>(model);

        //    if (model.TagIds != null && model.TagIds.Any())
        //    {
        //        existingProducts.ProductTags = model.TagIds.Select(tagId => new ProductTag
        //        {
        //            TagId = tagId
        //        }).ToList();
        //    }

        //    if (model.CoverImageFile != null)
        //    {
        //        if (!_fileService.IsImageFile(model.CoverImageFile))
        //            throw new ArgumentException("The file is not a valid image.", nameof(model.CoverImageFile));

        //        var oldCoverImageName = existingProducts.CoverImageName;
        //        existingProducts.CoverImageName = await _fileService.GenerateFile(model.CoverImageFile, FilePathConstants.ProductImagePath);

        //        if (!string.IsNullOrEmpty(oldCoverImageName))
        //        {
        //            var oldFilePath = Path.Combine(FilePathConstants.ProductImagePath, oldCoverImageName);
        //            if (File.Exists(oldFilePath))
        //                File.Delete(oldFilePath);
        //        }

        //        if (model.ImageFiles != null && model.ImageFiles.Any())
        //        {
        //            existingProducts.Images = new List<ProductImage>();
        //            foreach (var imageFile in model.ImageFiles)
        //            {
        //                if (!_fileService.IsImageFile(imageFile))
        //                    throw new ArgumentException("One of the files is not a valid image.", nameof(model.ImageFiles));
        //            }

        //            foreach (var imageFile in model.ImageFiles)
        //            {
        //                var imageName = await _fileService.GenerateFile(imageFile, FilePathConstants.ProductImagePath);
        //                existingProducts.Images.Add(new ProductImage
        //                {
        //                    ImageName = imageName
        //                });
        //            }
        //        }
        //    }

        //    await Repository.UpdateAsync(existingProducts);

        //    return true;
        //}
    }
}
