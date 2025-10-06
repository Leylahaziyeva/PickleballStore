using AutoMapper;
using PickleballStore.BLL.ViewModels.Account;
using PickleballStore.BLL.ViewModels.Address;
using PickleballStore.BLL.ViewModels.Category;
using PickleballStore.BLL.ViewModels.Checkout;
using PickleballStore.BLL.ViewModels.Order;
using PickleballStore.BLL.ViewModels.Product;
using PickleballStore.BLL.ViewModels.ProductImage;
using PickleballStore.BLL.ViewModels.ProductVariant;
using PickleballStore.BLL.ViewModels.Slider;
using PickleballStore.BLL.ViewModels.Wishlist;
using PickleballStore.DAL.DataContext.Entities;

namespace PickleballStore.BLL.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductViewModel>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : ""))
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images))
                .ForMember(dest => dest.Variants, opt => opt.MapFrom(src => src.Variants))
                .ForMember(dest => dest.CoverImageName, opt => opt.MapFrom(src => src.CoverImageName));



            CreateMap<ProductViewModel, Product>()
                .ForMember(dest => dest.Images, opt => opt.Ignore())      // Images IFormFile deyil, map etməyəcəyik
                .ForMember(dest => dest.Variants, opt => opt.Ignore())
                .ForMember(dest => dest.Category, opt => opt.Ignore());


            CreateMap<Product, CreateProductViewModel>()
                .ForMember(dest => dest.ImageFiles, opt => opt.Ignore())
                .ForMember(dest => dest.Variants, opt => opt.Ignore())
                .ForMember(dest => dest.CategorySelectListItems, opt => opt.Ignore());


            CreateMap<CreateProductViewModel, Product>()
                .ForMember(dest => dest.Images, opt => opt.Ignore())
                .ForMember(dest => dest.Variants, opt => opt.Ignore())
                .ForMember(dest => dest.Category, opt => opt.Ignore());


            CreateMap<Product, UpdateProductViewModel>()
                .ForMember(dest => dest.ImageFiles, opt => opt.Ignore())
                .ForMember(dest => dest.ExistingImages, opt => opt.MapFrom(src => src.Images.Select(i => i.ImageName)))
                .ForMember(dest => dest.Variants, opt => opt.MapFrom(src => src.Variants))
                .ForMember(dest => dest.CategorySelectListItems, opt => opt.Ignore());


            CreateMap<UpdateProductViewModel, Product>()
                .ForMember(dest => dest.Images, opt => opt.Ignore())
                .ForMember(dest => dest.Variants, opt => opt.Ignore())
                .ForMember(dest => dest.Category, opt => opt.Ignore());


            CreateMap<ProductImage, ProductImageViewModel>()
                .ForMember(dest => dest.ImageName, opt => opt.MapFrom(src => src.ImageName))
                .ForMember(dest => dest.ColorCode, opt => opt.MapFrom(src => src.ColorCode ?? "default"))
                .ForMember(dest => dest.IsMain, opt => opt.MapFrom(src => src.IsMain))
                .ForMember(dest => dest.IsHover, opt => opt.MapFrom(src => src.IsHover));


            CreateMap<ProductImageViewModel, ProductImage>()
                .ForMember(dest => dest.ImageName, opt => opt.MapFrom(src => src.ImageName))
                .ForMember(dest => dest.ColorCode, opt => opt.MapFrom(src => src.ColorCode))
                .ForMember(dest => dest.IsMain, opt => opt.MapFrom(src => src.IsMain))
                .ForMember(dest => dest.IsHover, opt => opt.MapFrom(src => src.IsHover));


            CreateMap<ProductVariant, ProductVariantViewModel>().ReverseMap();
            CreateMap<ProductVariant, CreateProductVariantViewModel>().ReverseMap();
            CreateMap<ProductVariant, UpdateProductVariantViewModel>().ReverseMap();


            CreateMap<Category, CategoryViewModel>().ReverseMap();
            CreateMap<Category, CreateCategoryViewModel>().ReverseMap();
            CreateMap<Category, UpdateCategoryViewModel>().ReverseMap();

            CreateMap<Slider, SliderViewModel>().ForMember(dest => dest.ProductName,
                opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : null)).ReverseMap();
            CreateMap<Slider, CreateSliderViewModel>().ReverseMap();
            CreateMap<Slider, UpdateSliderViewModel>().ReverseMap();

            CreateMap<Order, OrderListViewModel>();
            CreateMap<Order, OrderDetailsViewModel>();
            CreateMap<OrderItem, OrderItemViewModel>();

            CreateMap<CheckoutViewModel, Order>()
                .ForMember(dest => dest.ShippingAddress, opt => opt.Ignore())
                .ForMember(dest => dest.Items, opt => opt.Ignore())
                .ForMember(dest => dest.OrderNumber, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore());

            CreateMap<CartItemViewModel, OrderItem>()
                .ForMember(dest => dest.Order, opt => opt.Ignore())
                .ForMember(dest => dest.OrderId, opt => opt.Ignore())
                .ForMember(dest => dest.Product, opt => opt.Ignore());

            CreateMap<CreateAddressViewModel, Address>()
          .ForMember(dest => dest.Adress, opt => opt.MapFrom(src => src.Adress))
          .ForMember(dest => dest.Id, opt => opt.Ignore())
          .ForMember(dest => dest.UserId, opt => opt.Ignore());

            CreateMap<UpdateAddressViewModel, Address>()
                .ForMember(dest => dest.Adress, opt => opt.MapFrom(src => src.Adress))
                .ForMember(dest => dest.UserId, opt => opt.Ignore());

            CreateMap<Address, AddressViewModel>();

            CreateMap<WishlistItem, WishlistViewModel>();
            CreateMap<WishlistCreateViewModel, WishlistItem>();
            CreateMap<WishlistUpdateViewModel, WishlistItem>();


            CreateMap<AppUser, AccountViewModel>()
                .ReverseMap()
                .ForMember(dest => dest.UserName, opt => opt.Ignore())
                .ForMember(dest => dest.NormalizedUserName, opt => opt.Ignore())
                .ForMember(dest => dest.NormalizedEmail, opt => opt.Ignore())
                .ForMember(dest => dest.EmailConfirmed, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.SecurityStamp, opt => opt.Ignore())
                .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore())
                .ForMember(dest => dest.PhoneNumber, opt => opt.Ignore())
                .ForMember(dest => dest.PhoneNumberConfirmed, opt => opt.Ignore())
                .ForMember(dest => dest.TwoFactorEnabled, opt => opt.Ignore())
                .ForMember(dest => dest.LockoutEnd, opt => opt.Ignore())
                .ForMember(dest => dest.LockoutEnabled, opt => opt.Ignore())
                .ForMember(dest => dest.AccessFailedCount, opt => opt.Ignore());
        }
    }
}