using System.Text;
using System.Text.Json;
using GeekShopping.OrderAPI.Messages;
using GeekShopping.OrderAPI.Model;
using GeekShopping.OrderAPI.RabbitMQSender;
using GeekShopping.OrderAPI.Repository.Interface;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace GeekShopping.OrderAPI.MessagesConsumer
{
    public class RabbitMQPaymentConsumer : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private IConnection _connection;
        private IChannel _channel;
        private const string _exchangeName = "DirectPaymentUpdateExchange";

        private const string _paymentOrderUpdateQueueName = "PaymentOrderUpdateQueueName";


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

                        var paymentResult = JsonSerializer.Deserialize<UpdatePaymentResultDTO>(content);

                        await UpdatePaymentStatus(paymentResult);

                        await _channel.BasicAckAsync(evt.DeliveryTag, false);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erro ao processar a mensagem: {ex.Message}");
                        await _channel.BasicNackAsync(evt.DeliveryTag, false, requeue: true);
                    }

                };

                await _channel.BasicConsumeAsync(_paymentOrderUpdateQueueName, false, consumer);

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

        private async Task UpdatePaymentStatus(UpdatePaymentResultDTO paymentResult)
        {

            using (var scope = _serviceProvider.CreateScope())
            {
                var orderRepository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();
                try
                {
                    await orderRepository.UpdateOrderPaymentStatus(paymentResult.OrderId, paymentResult.Status);
                }
                catch (Exception)
                {
                    throw;
                }
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

             await _channel.ExchangeDeclareAsync(_exchangeName, ExchangeType.Direct);

            //a fila poderia ter sido declarada da maneira abaixo, inclusive é melhor indicado
            // await _channel.QueueDeclareAsync(queue: "orderpaymentresultqueue", durable: false, exclusive: false, autoDelete: false, arguments: null);
            await _channel.QueueDeclareAsync(queue: _paymentOrderUpdateQueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            //Para o tipo fanout não é necessário o routingKey pois ele é ignorado
            // await _channel.QueueBindAsync(_queueName, _exchangeName, routingKey: "");

            await _channel.QueueBindAsync(_paymentOrderUpdateQueueName, _exchangeName, routingKey: "Payment");
        }

    }
}