using GeekShopping.Email.Messages;
using GeekShopping.Email.Model;
using GeekShopping.Email.Model.Context;

namespace GeekShopping.Email.Repository.Interface
{
    public class EmailRepository : IEmailRepository
    {
        private readonly MySqlContext _context;

        public EmailRepository(MySqlContext context)
        {
            _context = context;
        }


        public async Task LogEmail(UpdatePaymentResultMessage message)
        {
            EmailLog email = new EmailLog()
            {
                Email = message.Email,
                SentDate = DateTime.Now,
                Log = $"Order - {message.OrderId} has been created successfully!"
            };
            
            _context.EmailsLogs.Add(email);
            await _context.SaveChangesAsync();
        }

    }
}