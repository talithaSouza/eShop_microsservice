

using GeekShopping.PaymentAPI.MessagesConsumer;
using GeekShopping.PaymentAPI.RabbitMQSender;

namespace GeekShopping.PaymentProcessor.Config
{
    public static class DependencyInjection
    {
        public static IServiceCollection RegisterRepository(this IServiceCollection services)
        {

            #region 
            services.AddSingleton<IProcessPayment, ProcessPayment>();
            #endregion
            #region R
            services.AddHostedService<RabbitMQPaymentConsumer>();
            services.AddSingleton<IRabbitMQMessageSender, RabbitMQMessageSender>();
            #endregion


            return services;
        }
    }
}