
using GeekShopping.CartAPI.RabbitMQSender;
using GeekShopping.CartAPI.Repository;
using GeekShopping.CartAPI.Repository.Interfaces;

namespace GeekShopping.ProductAPI.Config
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterRepository(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient<ICouponRepository, CouponRepository>(c =>
                c.BaseAddress = new Uri(configuration["ServicesUrls:CouponAPI"])
            );

            #region C
            services.AddScoped<ICartRepository, CartRepository>();

            services.AddScoped<ICouponRepository, CouponRepository>();
            #endregion
          
            #region R
            services.AddSingleton<IRabbitMQMessageSender, RabbitMQMessageSender>();
            #endregion

            return services;
        }
    }
}