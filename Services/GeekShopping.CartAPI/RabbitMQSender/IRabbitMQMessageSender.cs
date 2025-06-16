using GeekShopping.MessageBus;

namespace GeekShopping.CartAPI.RabbitMQSender
{
    public interface IRabbitMQMessageSender
    {
        Task SendMessage(BaseMessage baseMessage, string queueName);
    }
}