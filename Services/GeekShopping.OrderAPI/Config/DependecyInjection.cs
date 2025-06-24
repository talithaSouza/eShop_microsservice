

using GeekShopping.OrderAPI.MessagesConsumer;
using GeekShopping.OrderAPI.Model.Context;
using GeekShopping.OrderAPI.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.OrderAPI.Config
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterRepository(this IServiceCollection services)
        {
            #region O
            services.AddScoped<IOrderRepository, OrderRepository>();
            #endregion
          
            #region R
            services.AddHostedService<RabbitMQCheckoutConsumer>();
            #endregion

            return services;
        }
    }
}