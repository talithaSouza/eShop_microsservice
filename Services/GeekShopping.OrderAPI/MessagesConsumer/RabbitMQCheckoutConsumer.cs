using System.Text;
using System.Text.Json;
using GeekShopping.OrderAPI.Messages;
using GeekShopping.OrderAPI.Model;
using GeekShopping.OrderAPI.Repository.Interface;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace GeekShopping.OrderAPI.MessagesConsumer
{
    public class RabbitMQCheckoutConsumer : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private IConnection _connection;
        private IChannel _channel;

        public RabbitMQCheckoutConsumer(IServiceProvider serviceProvider)
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

                        var checkoutHeaderDTO = JsonSerializer.Deserialize<CheckoutHeaderDTO>(content);

                        await ProcessOrder(checkoutHeaderDTO);

                        await _channel.BasicAckAsync(evt.DeliveryTag, false);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erro ao processar a mensagem: {ex.Message}");
                        await _channel.BasicNackAsync(evt.DeliveryTag, false, requeue: true);
                    }

                };

                await _channel.BasicConsumeAsync("checkoutqueue", false, consumer);

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

        private async Task ProcessOrder(CheckoutHeaderDTO checkoutHeaderDTO)
        {
            OrderHeader order = new()
            {
                UserId = checkoutHeaderDTO.UserId,
                FirstName = checkoutHeaderDTO.FirstName,
                LastName = checkoutHeaderDTO.LastName,
                OrderDetails = new List<OrderDetail>(),
                CardNumber = checkoutHeaderDTO.CardNumber,
                CouponCode = checkoutHeaderDTO.CouponCode,
                CVV = checkoutHeaderDTO.CVV,
                DiscountAmount = checkoutHeaderDTO.DiscountAmount,
                Email = checkoutHeaderDTO.Email,
                ExpiryMonthYear = checkoutHeaderDTO.ExpiryMothYear,
                OrderTime = DateTime.Now,
                PurchaseAmount = checkoutHeaderDTO.PurchaseAmount,
                PaymentStatus = false,
                Phone = checkoutHeaderDTO.Phone,
                DateTime = checkoutHeaderDTO.DateTime
            };

            foreach (var details in checkoutHeaderDTO.CartDetails)
            {
                OrderDetail detail = new()
                {
                    ProductId = details.ProductId,
                    ProductName = details.Product.Name,
                    Price = details.Product.Price,
                    Count = details.Count,
                };

                order.CartTotalItens += details.Count;
                order.OrderDetails.Add(detail);
            }

            using (var scope = _serviceProvider.CreateScope())
            {
                var orderRepository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();

                await orderRepository.AddOrder(order);
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

            await _channel.QueueDeclareAsync(queue: "checkoutqueue", durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

    }
}