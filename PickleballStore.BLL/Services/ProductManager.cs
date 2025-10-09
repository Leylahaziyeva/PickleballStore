using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PickleballStore.BLL.Services.Contracts;
using PickleballStore.BLL.ViewModels.Product;
using PickleballStore.BLL.ViewModels.ProductVariant;
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

        public async Task<CreateProductViewModel> GetCreateProductViewModelAsync()
        {
            var createProductViewModel = new CreateProductViewModel();

            createProductViewModel.CategorySelectListItems = await _categoryService.GetCategorySelectListItemsAsync();

            return createProductViewModel;
        }

        public async Task<UpdateProductViewModel> GetUpdateViewModelAsync(int id)
        {
            var product = await Repository.GetAsync(
                predicate: p => p.Id == id,
                include: source => source
                    .Include(p => p.Images!)
                    .Include(p => p.Variants!)
                    .Include(p => p.Category!)
            );

            if (product == null) return null!;

            var updateProductViewModel = Mapper.Map<UpdateProductViewModel>(product);
            updateProductViewModel.CategorySelectListItems = await _categoryService.GetCategorySelectListItemsAsync();

            if (product.Images != null && product.Images.Any())
            {
                updateProductViewModel.ExistingImages = product.Images
                    .Select(i => i.ImageName)
                    .ToList();
            }

            if (product.Variants != null && product.Variants.Any())
            {
                updateProductViewModel.Variants = Mapper.Map<List<UpdateProductVariantViewModel>>(product.Variants);
            }

            return updateProductViewModel;
        }

        public async Task<List<ProductViewModel>> GetRelatedProductsAsync(int categoryId, int id)
        {
            var products = await _repository.GetProductsByCategoryAsync(categoryId, id);

            var relatedProductsViewModel = _mapper.Map<List<ProductViewModel>>(products);

            return relatedProductsViewModel;
        }

        public async Task<List<ProductViewModel>> GetAllWithDetailsAsync()
        {
            var products = await Repository.GetAllAsync(
                include: source => source.Include(p => p.Category!)
            );

            return Mapper.Map<List<ProductViewModel>>(products);
        }

        public override async Task CreateAsync(CreateProductViewModel model)
        {
            var coverImageName = await _fileService.SaveFileAsync(model.CoverImageFile);

            var imageNames = new List<string>();
            if (model.ImageFiles != null && model.ImageFiles.Any())
            {
                foreach (var file in model.ImageFiles)
                {
                    var imageName = await _fileService.SaveFileAsync(file);
                    imageNames.Add(imageName);
                }
            }

            var variants = new List<ProductVariant>();
            if (model.Variants != null && model.Variants.Any())
            {
                foreach (var variant in model.Variants)
                {
                    string? variantImageName = null;

                    if (variant.ImageFile != null && variant.ImageFile.Length > 0)
                    {
                        variantImageName = await _fileService.SaveFileAsync(variant.ImageFile);
                    }

                    variants.Add(new ProductVariant
                    {
                        OptionName = variant.OptionName,
                        OptionValue = variant.OptionValue,
                        ColorCode = variant.ColorCode,
                        OptionImageName = variantImageName 
                    });
                }
            }

            var product = new Product
            {
                Name = model.Name,
                Description = model.Description,
                AdditionalInformation = model.AdditionalInformation,
                Price = model.Price,
                QuantityAvailable = model.Stock,
                CategoryId = model.CategoryId,
                CoverImageName = coverImageName,
                IsBestSeller = false,
                LiveViewCount = 0,
                LiveInCarts = 0,
                BadgeLabel = null,
                BadgeCssClass = null,
                CountdownEndDate = null,

                Images = new List<ProductImage>
        {
            new ProductImage
            {
                ImageName = coverImageName,
                IsMain = true,
                IsHover = false
            }
        },

                Variants = variants 
            };

            if (imageNames.Any())
            {
                foreach (var imageName in imageNames)
                {
                    product.Images.Add(new ProductImage
                    {
                        ImageName = imageName,
                        IsMain = false,
                        IsHover = false
                    });
                }
            }

            await _repository.CreateAsync(product);
        }

        public override async Task<bool> UpdateAsync(int id, UpdateProductViewModel model)
        {
            var product = await Repository.GetAsync(
                predicate: p => p.Id == id,
                include: source => source
                    .Include(p => p.Images!)
                    .Include(p => p.Variants!)
            );

            if (product == null)
                return false;

            product.Name = model.Name;
            product.Description = model.Description;
            product.AdditionalInformation = model.AdditionalInformation;
            product.Price = model.Price;
            product.QuantityAvailable = model.Stock;
            product.CategoryId = model.CategoryId;

            if (model.CoverImageFile != null && model.CoverImageFile.Length > 0)
            {
                if (!string.IsNullOrEmpty(product.CoverImageName))
                {
                    _fileService.DeleteFile(product.CoverImageName);
                }

                var oldCoverImage = product.Images!.FirstOrDefault(i => i.IsMain);
                if (oldCoverImage != null)
                {
                    product.Images!.Remove(oldCoverImage);
                }

                var newCoverImageName = await _fileService.SaveFileAsync(model.CoverImageFile);
                product.CoverImageName = newCoverImageName;

                product.Images!.Add(new ProductImage
                {
                    ImageName = newCoverImageName,
                    IsMain = true,
                    IsHover = false,
                    ProductId = product.Id
                });
            }

            if (model.ImageFiles != null && model.ImageFiles.Any())
            {
                foreach (var file in model.ImageFiles)
                {
                    if (file != null && file.Length > 0)
                    {
                        var imageName = await _fileService.SaveFileAsync(file);
                        product.Images!.Add(new ProductImage
                        {
                            ImageName = imageName,
                            IsMain = false,
                            IsHover = false,
                            ProductId = product.Id
                        });
                    }
                }
            }

            if (model.Variants != null)
            {
                var existingVariantIds = model.Variants
                    .Where(v => v.Id > 0)
                    .Select(v => v.Id)
                    .ToList();

                var variantsToRemove = product.Variants!
                    .Where(v => !existingVariantIds.Contains(v.Id))
                    .ToList();

                foreach (var variant in variantsToRemove)
                {
                    if (!string.IsNullOrEmpty(variant.OptionImageName))
                        _fileService.DeleteFile(variant.OptionImageName);

                    product.Variants!.Remove(variant);
                }

                foreach (var variantModel in model.Variants)
                {
                    if (variantModel.Id > 0)
                    {
                        var existingVariant = product.Variants!.FirstOrDefault(v => v.Id == variantModel.Id);
                        if (existingVariant != null)
                        {
                            existingVariant.OptionName = variantModel.OptionName;
                            existingVariant.OptionValue = variantModel.OptionValue;
                            existingVariant.ColorCode = variantModel.ColorCode;

                            if (variantModel.ImageFile != null && variantModel.ImageFile.Length > 0)
                            {
                                if (!string.IsNullOrEmpty(existingVariant.OptionImageName))
                                    _fileService.DeleteFile(existingVariant.OptionImageName);

                                var variantImageName = await _fileService.SaveFileAsync(variantModel.ImageFile);
                                existingVariant.OptionImageName = variantImageName;
                            }
                        }
                    }
                    else
                    {
                        string? newVariantImageName = null;
                        if (variantModel.ImageFile != null && variantModel.ImageFile.Length > 0)
                        {
                            newVariantImageName = await _fileService.SaveFileAsync(variantModel.ImageFile);
                        }

                        product.Variants!.Add(new ProductVariant
                        {
                            OptionName = variantModel.OptionName,
                            OptionValue = variantModel.OptionValue,
                            ColorCode = variantModel.ColorCode,
                            OptionImageName = newVariantImageName,
                            ProductId = product.Id
                        });
                    }
                }
            }

            await Repository.UpdateAsync(product);
            return true;
        }
    }
}