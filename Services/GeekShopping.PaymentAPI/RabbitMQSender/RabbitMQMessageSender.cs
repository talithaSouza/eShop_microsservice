using System.Text;
using System.Text.Json;
using GeekShopping.MessageBus;
using GeekShopping.PaymentAPI.Messages;
using RabbitMQ.Client;

namespace GeekShopping.PaymentAPI.RabbitMQSender
{
    public class RabbitMQMessageSender : IRabbitMQMessageSender
    {
        private readonly string _hostName;
        private readonly string _password;
        private readonly string _userName;
        private IConnection _connection;
        // private const string _exchangeName = "FanoutPaymentUpdateExchange";
        private const string _exchangeName = "DirectPaymentUpdateExchange";
        private const string _paymenEmailtUpdateQueueName = "PaymentEmailUpdateQueueName";
        private const string _paymentOrderUpdateQueueName = "PaymentOrderUpdateQueueName";

        public RabbitMQMessageSender()
        {
            _hostName = "localhost";
            _password = "guest";
            _userName = "guest";
        }
        public async Task SendMessageAsync(BaseMessage baseMessage)
        {
            if (await ConnectionExists())
            {
                using var channel = await _connection.CreateChannelAsync();

                //   await channel.ExchangeDeclareAsync(_exchangeName, ExchangeType.Fanout, durable: false);

                await channel.ExchangeDeclareAsync(_exchangeName, ExchangeType.Direct, durable: false);

                await channel.QueueDeclareAsync(_paymenEmailtUpdateQueueName, durable: false, exclusive: false, autoDelete: false);

                await channel.QueueDeclareAsync(_paymentOrderUpdateQueueName, durable: false, exclusive: false, autoDelete: false);

                await channel.QueueBindAsync(_paymenEmailtUpdateQueueName, _exchangeName, "Payment");
                await channel.QueueBindAsync(_paymentOrderUpdateQueueName, _exchangeName, "Payment");

                byte[] body = GetMessageAsByteArray(baseMessage);

                await channel.BasicPublishAsync(exchange: _exchangeName, routingKey: "Payment", body: body);

                // para o tipo fanout: await channel.BasicPublishAsync(exchange: _exchangeName, routingKey: "Payment", body: body);
            }
        }

        private byte[] GetMessageAsByteArray(BaseMessage message)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
            };

            var json = JsonSerializer.Serialize<UpdatePaymentResultMessage>((UpdatePaymentResultMessage)message, options);
            var body = Encoding.UTF8.GetBytes(json);
            return body;
        }

        private async Task CreateConnection()
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _hostName,
                    UserName = _userName,
                    Password = _password
                };
                _connection = await factory.CreateConnectionAsync();
            }
            catch (Exception)
            {
                //Log exception
                throw;
            }
        }

        private async Task<bool> ConnectionExists()
        {
            if (_connection != null)
                return true;

            await CreateConnection();

            return _connection != null;
        }

    }
}