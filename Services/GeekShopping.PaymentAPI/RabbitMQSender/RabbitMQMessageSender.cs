using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using GeekShopping.MessageBus;
using GeekShopping.PaymentAPI.Messages;
using RabbitMQ.Client;

namespace GeekShopping.PaymentProcessor.RabbitMQSender
{
    public class RabbitMQMessageSender : IRabbitMQMessageSender
    {
        private readonly string _hostName;
        private readonly string _password;
        private readonly string _userName;
        private IConnection _connection;

        private const string _exchangeName = "FanoutPaymentUpdateExchange";

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

                await channel.ExchangeDeclareAsync(_exchangeName, ExchangeType.Fanout, durable: false);

                byte[] body = GetMessageAsByteArray(baseMessage);

                await channel.BasicPublishAsync(exchange: _exchangeName, routingKey: "", body: body);
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