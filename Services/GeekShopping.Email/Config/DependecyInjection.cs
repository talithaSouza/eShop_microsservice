
using GeekShopping.Email.MessagesConsumer;
using GeekShopping.Email.Repository.Interface;

namespace GeekShopping.Email.Config
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterRepository(this IServiceCollection services)
        {
            #region O
            services.AddScoped<IEmailRepository, EmailRepository>();
            #endregion
          
            #region R
            // services.AddHostedService<RabbitMQCheckoutConsumer>();
            services.AddHostedService<RabbitMQPaymentConsumer>();
            // services.AddSingleton<IRabbitMQMessageSender, RabbitMQMessageSender>();
            #endregion
             

            return services;
        }
    }
}