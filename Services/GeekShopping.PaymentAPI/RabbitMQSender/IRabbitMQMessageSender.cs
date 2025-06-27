using GeekShopping.MessageBus;

namespace GeekShopping.PaymentProcessor.RabbitMQSender
{
    public interface IRabbitMQMessageSender
    {
        Task SendMessageAsync(BaseMessage baseMessage, string queueName);
    }
}