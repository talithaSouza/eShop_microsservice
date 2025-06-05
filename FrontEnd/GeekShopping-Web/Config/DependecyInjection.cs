using GeekShopping_Web.Services;
using GeekShopping_Web.Services.IServices;

namespace GeekShopping_Web.Config
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            #region H
            services.AddHttpClient<IProductService, ProductService>(c =>
                c.BaseAddress = new Uri(configuration["ServicesUrls:ProductsAPI"])
            );
            #endregion

            return services;
        }

    }
}