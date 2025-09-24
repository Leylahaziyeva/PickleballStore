using Microsoft.Extensions.DependencyInjection;
using PickleballStore.BLL.Mapping;
using PickleballStore.BLL.Services;
using PickleballStore.BLL.Services.Contracts;

namespace PickleballStore.BLL
{
    public static class BussinessLogicLayerServiceRegistration
    {
        public static IServiceCollection AddBussinessLogicLayerServices(this IServiceCollection services)
        {
            services.AddAutoMapper(confg => confg.AddProfile<MappingProfile>());
            services.AddScoped(typeof(ICrudService<,,,>), typeof(CrudManager<,,,>));
            services.AddScoped<IHeaderService, HeaderManager>();
            services.AddScoped<IFooterService, FooterManager>();
            services.AddScoped<ICategoryService, CategoryManager>();
            services.AddScoped<IProductService, ProductManager>();
            services.AddScoped<ISliderService, SliderManager>();
            services.AddScoped<IHomeService, HomeManager>();
            services.AddScoped<IShopService, ShopManager>();
            services.AddScoped<FileService>();
            services.AddScoped<WishlistManager>();
            services.AddScoped<BasketManager>();
            //services.AddScoped<IReviewService, ReviewManager>();

            return services;
        }
    }
}
