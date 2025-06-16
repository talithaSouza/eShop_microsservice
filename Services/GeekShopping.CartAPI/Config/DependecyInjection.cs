
using GeekShopping.CartAPI.RabbitMQSender;
using GeekShopping.CartAPI.Repository;
using GeekShopping.CartAPI.Repository.Interfaces;

namespace GeekShopping.ProductAPI.Config
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterRepository(this IServiceCollection services)
        {
            #region C
            services.AddScoped<ICartRepository, CartRepository>();
            #endregion

            #region R
            services.AddSingleton<IRabbitMQMessageSender, RabbitMQMessageSender>();
            #endregion

            return services;
        }
    }
}