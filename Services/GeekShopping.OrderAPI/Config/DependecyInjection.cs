

using GeekShopping.OrderAPI.Model.Context;
using GeekShopping.OrderAPI.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.OrderAPI.Config
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterRepository(this IServiceCollection services, DbContextOptionsBuilder<MySqlContext> dbBuilder)
        {
            #region O
            services.AddSingleton(new OrderRepository(dbBuilder.Options));
            services.AddSingleton<IOrderRepository, OrderRepository>();
            #endregion
          
            // #region R
            // services.AddSingleton<IRabbitMQMessageSender, RabbitMQMessageSender>();
            // #endregion

            return services;
        }
    }
}