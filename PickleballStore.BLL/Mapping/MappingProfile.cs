using AutoMapper;
using PickleballStore.BLL.ViewModels.Category;
using PickleballStore.BLL.ViewModels.Product;
using PickleballStore.BLL.ViewModels.ProductVariant;
using PickleballStore.BLL.ViewModels.Slider;
using PickleballStore.DAL.DataContext.Entities;

namespace PickleballStore.BLL.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Category, CategoryViewModel>().ReverseMap();
            CreateMap<Category, CreateCategoryViewModel>().ReverseMap();
            CreateMap<Category, UpdateCategoryViewModel>().ReverseMap();

            CreateMap<Product, ProductViewModel>()
                .ForMember(x => x.CategoryName, opt => opt.MapFrom(src => src.Category == null ? "" : src.Category.Name))
                .ForMember(x => x.ImageNames, opt => opt.MapFrom(src => src.Images.Select(i => i.ImageName).ToList()))
                .ForMember(x => x.Variants, opt => opt.MapFrom(src => src.Variants))
                .ReverseMap();

            CreateMap<Product, CreateProductViewModel>().ReverseMap();
            CreateMap<Product, UpdateProductViewModel>().ReverseMap();

            CreateMap<ProductVariant, ProductVariantViewModel>().ReverseMap();
            CreateMap<ProductVariant, CreateProductVariantViewModel>().ReverseMap();
            CreateMap<ProductVariant, UpdateProductVariantViewModel>().ReverseMap();

            CreateMap<Slider, SliderViewModel>().ReverseMap();
            CreateMap<Slider, CreateSliderViewModel>().ReverseMap();
            CreateMap<Slider, UpdateSliderViewModel>().ReverseMap();

            //CreateMap<Review, CreateReviewViewModel>().ReverseMap();
            //CreateMap<Review, UpdateReviewViewModel>().ReverseMap();
            //CreateMap<Review, ReviewViewModel>()
            //    .ForMember(x => x.ProductName, opt => opt.MapFrom(src => src.Product == null ? "" : src.Product.Name))
            //    .ForMember(x => x.AppUserName, opt => opt.MapFrom(src => src.AppUser == null ? "" : src.AppUser.UserName))
            //    .ForMember(x => x.AppUserProfileImageName, opt => opt.MapFrom(src => src.AppUser == null ? "" : src.AppUser.ProfileImageName))
            //    .ReverseMap();
        }
    }
}
