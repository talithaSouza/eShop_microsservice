using GeekShopping.MessageBus;

namespace GeekShopping.OrderAPI.RabbitMQSender
{
    public interface IRabbitMQMessageSender
    {
        Task SendMessageAsync(BaseMessage baseMessage, string queueName);
    }
}