using System.Text;
using System.Text.Json;
using GeekShopping.Email.Messages;
using GeekShopping.Email.Repository.Interface;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace GeekShopping.Email.MessagesConsumer
{
    public class RabbitMQPaymentConsumer : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private IConnection _connection;
        private IChannel _channel;
        private const string _exchangeName = "FanoutPaymentUpdateExchange";
        //Essa propriedade pode ter um nome declarado pelo proprio dev, foi usado um nome randomico apenas para exemplo
        private string _queueName = "";

        public RabbitMQPaymentConsumer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (await ConnectionExists())
            {
                stoppingToken.ThrowIfCancellationRequested();

                var consumer = new AsyncEventingBasicConsumer(_channel);

                consumer.ReceivedAsync += async (chanel, evt) =>
                {
                    try
                    {
                        var content = Encoding.UTF8.GetString(evt.Body.ToArray());

                        var message = JsonSerializer.Deserialize<UpdatePaymentResultMessage>(content);

                        await ProcessLogs(message);

                        await _channel.BasicAckAsync(evt.DeliveryTag, false);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erro ao processar a mensagem: {ex.Message}");
                        await _channel.BasicNackAsync(evt.DeliveryTag, false, requeue: true);
                    }

                };

                await _channel.BasicConsumeAsync(_queueName, false, consumer);

                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_channel != null)
            {
                await _channel.CloseAsync();
                _channel.Dispose();
            }

            if (_connection != null)
            {
                await _connection.CloseAsync();
                _connection.Dispose();
            }

            await base.StopAsync(cancellationToken);
        }

        private async Task ProcessLogs(UpdatePaymentResultMessage message)
        {
            using var scope = _serviceProvider.CreateScope();

            var emailRepository = scope.ServiceProvider.GetRequiredService<IEmailRepository>();
            try
            {
                await emailRepository.LogEmail(message);
            }
            catch (Exception)
            {
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

        private async Task CreateConnection()
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                Password = "guest",
                UserName = "guest"
            };
            _connection = await factory.CreateConnectionAsync();

            _channel = await _connection.CreateChannelAsync();

            await _channel.ExchangeDeclareAsync(_exchangeName, ExchangeType.Fanout);

            _queueName = (await _channel.QueueDeclareAsync()).QueueName;
            //a fila poderia ter sido declarada da maneira abaixo, inclusive é melhor indicado
            // await _channel.QueueDeclareAsync(queue: "orderpaymentresultqueue", durable: false, exclusive: false, autoDelete: false, arguments: null);

            //Para o tipo fanout não é necessário o routingKey pois ele é ignorado
            await _channel.QueueBindAsync(_queueName, _exchangeName, routingKey: "");
        }

    }
}