using GeekShopping.OrderAPI.Model;
using GeekShopping.OrderAPI.Model.Context;
using Microsoft.EntityFrameworkCore;

namespace GeekShopping.OrderAPI.Repository.Interface
{
    public class OrderRepository : IOrderRepository
    {
        private readonly MySqlContext _context;

        public OrderRepository(MySqlContext context)
        {
            _context = context;
        }


        public async Task<bool> AddOrder(OrderHeader header)
        {
            if (header == null)
                return false;

            _context.Headers.Add(header);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task UpdateOrderPaymentStatus(long orderHeaderId, bool status)
        {
            var header = await _context.Headers.FirstOrDefaultAsync(o => o.Id == orderHeaderId);

            if (header != null)
            {
                header.PaymentStatus = status;
                await _context.SaveChangesAsync();
            }
        }

    }
}