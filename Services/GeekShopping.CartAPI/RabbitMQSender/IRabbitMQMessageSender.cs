using GeekShopping.MessageBus;

namespace GeekShopping.CartAPI.RabbitMQSender
{
    public interface IRabbitMQMessageSender
    {
        Task SendMessageAsync(BaseMessage baseMessage, string queueName);
    }
}