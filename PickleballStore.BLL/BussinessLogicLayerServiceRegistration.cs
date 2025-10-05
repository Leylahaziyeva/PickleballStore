using Microsoft.Extensions.DependencyInjection;
using PickleballStore.BLL.Mapping;
using PickleballStore.BLL.Services;
using PickleballStore.BLL.Services.Contracts;
using PickleballStore.Services;

namespace PickleballStore.BLL
{
    public static class BussinessLogicLayerServiceRegistration
    {
        public static IServiceCollection AddBussinessLogicLayerServices(this IServiceCollection services)
        {
            services.AddAutoMapper(confg => confg.AddProfile<MappingProfile>());
            services.AddScoped(typeof(ICrudService<,,,>), typeof(CrudManager<,,,>));
            services.AddScoped<ISearchService, SearchManager>();
            services.AddScoped<IFooterService, FooterManager>();
            services.AddScoped<ICategoryService, CategoryManager>();
            services.AddScoped<IProductService, ProductManager>();
            services.AddScoped<IProductVariantService, ProductVariantManager>();
            services.AddScoped<ISliderService, SliderManager>();
            services.AddScoped<IHomeService, HomeManager>();
            services.AddScoped<IShopService, ShopManager>();
            services.AddScoped<IWishlistService, WishlistManager>();
            services.AddScoped<IOrderService, OrderManager>();
            services.AddScoped<IAddressService, AddressManager>();
            services.AddScoped<FileService>();
            services.AddScoped<BasketManager>();

            return services;
        }
    }
}