using System.Text;
using System.Text.Json;
using GeekShopping.PaymentAPI.Messages;
using GeekShopping.PaymentProcessor;
using GeekShopping.PaymentProcessor.RabbitMQSender;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace GeekShopping.PaymentAPI.MessagesConsumer
{
    public class RabbitMQPaymentConsumer : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private IConnection _connection;
        private IChannel _channel;
        private readonly IProcessPayment _processPayment;
        private readonly IRabbitMQMessageSender _rabbitMQMessageSender;

        public RabbitMQPaymentConsumer(IServiceProvider serviceProvider, IProcessPayment processPayment, IRabbitMQMessageSender rabbitMQMessageSender)
        {
            _serviceProvider = serviceProvider;
            _processPayment = processPayment;
            _rabbitMQMessageSender = rabbitMQMessageSender;

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

                        var paymentMessage = JsonSerializer.Deserialize<PaymentMessage>(content);

                        await ProcessPayment(paymentMessage);

                        await _channel.BasicAckAsync(evt.DeliveryTag, false);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erro ao processar a mensagem: {ex.Message}");
                        await _channel.BasicNackAsync(evt.DeliveryTag, false, requeue: true);
                    }

                };

                await _channel.BasicConsumeAsync("orderpaymentprocessqueue", false, consumer);

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

//Erro ao processar a mensagem: Unable to cast object of type 'GeekShopping.PaymentAPI.Messages.UpdatePaymentResultMessage' to type 'GeekShopping.PaymentAPI.Messages.PaymentMessage'.
        private async Task ProcessPayment(PaymentMessage paymentMessage)
        {
            var result = _processPayment.PaymentProcessor();

            UpdatePaymentResultMessage paymentResult = new()
            {
                Status = result,
                OrderId = paymentMessage.OrderId,
                Email = paymentMessage.Email
            };
            // using (var scope = _serviceProvider.CreateScope())
            // {
            //     //  var rabbitMQMessageSender = scope.ServiceProvider.GetRequiredService<IRabbitMQMessageSender>();
            try
            {
                await _rabbitMQMessageSender.SendMessageAsync(paymentResult, "orderpaymentresultqueue");
            }
            catch (Exception)
            {
                throw;
            }
            // }

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

            await _channel.QueueDeclareAsync(queue: "orderpaymentprocessqueue", durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

    }
}