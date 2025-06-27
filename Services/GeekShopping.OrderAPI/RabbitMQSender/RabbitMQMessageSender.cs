using System.Text;
using System.Text.Json;
using GeekShopping.MessageBus;
using GeekShopping.OrderAPI.Messages;
using RabbitMQ.Client;

namespace GeekShopping.OrderAPI.RabbitMQSender
{
    public class RabbitMQMessageSender : IRabbitMQMessageSender
    {
        private readonly string _hostName;
        private readonly string _password;
        private readonly string _userName;
        private IConnection _connection;

        public RabbitMQMessageSender()
        {
            _hostName = "localhost";
            _password = "guest";
            _userName = "guest";
        }
        public async Task SendMessageAsync(BaseMessage baseMessage, string queueName)
        {
            var factory = new ConnectionFactory()
            {
                HostName = _hostName,
                Password = _password,
                UserName = _userName
            };

            _connection = await factory.CreateConnectionAsync();

            using var channel = await _connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            byte[] body = GetMessageAsByteArray(baseMessage);

            await channel.BasicPublishAsync(exchange: "", routingKey: queueName, body: body);
        }

        private byte[] GetMessageAsByteArray(BaseMessage message)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
            };

            var json = JsonSerializer.Serialize<PaymentDTO>((PaymentDTO)message, options);
            var body = Encoding.UTF8.GetBytes(json);
            return body;
        }

    }
}