
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using GeekShopping.OrderAPI.Messages;
using GeekShopping.OrderAPI.Model;
using GeekShopping.OrderAPI.Repository.Interface;
using Microsoft.EntityFrameworkCore.Metadata;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace GeekShopping.OrderAPI.MessagesConsumer
{
    public class RabbitMQCheckoutConsumer : BackgroundService
    {
        private readonly IServiceProvider serviceProvider;

        public RabbitMQCheckoutConsumer(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;

            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                Password = "guest",
                UserName = "guest"
            };

            Task.Run(async () =>
            {
                _connection = await factory.CreateConnectionAsync();

                _channel = await _connection.CreateChannelAsync();

                await _channel.QueueDeclareAsync(queue: "checkoutqueue", durable: false, exclusive: false, autoDelete: false, arguments: null);
            }).GetAwaiter().GetResult();
        }


        private IConnection _connection;
        private IChannel _channel;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
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

            return;
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

            using (var scope = serviceProvider.CreateScope())
            {
                var orderRepository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();

                await orderRepository.AddOrder(order);
            }

        }

    }
}