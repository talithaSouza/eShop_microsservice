

using GeekShopping.OrderAPI.MessagesConsumer;
using GeekShopping.OrderAPI.RabbitMQSender;
using GeekShopping.OrderAPI.Repository.Interface;

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
            services.AddSingleton<IRabbitMQMessageSender, RabbitMQMessageSender>();
            #endregion
             

            return services;
        }
    }
}