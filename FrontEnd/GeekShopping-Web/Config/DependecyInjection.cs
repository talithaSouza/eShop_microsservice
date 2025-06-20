using GeekShopping.Web.Services.IServices;
using GeekShopping_Web.Services;
using GeekShopping_Web.Services.IServices;

namespace GeekShopping_Web.Config
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
           
            #region C
            services.AddHttpClient<ICartService, CartService>(c =>
                c.BaseAddress = new Uri(configuration["ServicesUrls:CartAPI"])
            );
           
            services.AddHttpClient<ICouponService, CouponService>(c =>
                c.BaseAddress = new Uri(configuration["ServicesUrls:CouponAPI"])
            );
            #endregion

            #region P
            services.AddHttpClient<IProductService, ProductService>(c =>
                c.BaseAddress = new Uri(configuration["ServicesUrls:ProductsAPI"])
            );
            #endregion

            return services;
        }

    }
}