using Microsoft.EntityFrameworkCore;

namespace GeekShopping.Email.Model.Context
{
    public class MySqlContext : DbContext
    {
        public MySqlContext(DbContextOptions<MySqlContext> options) : base(options) { }

        public DbSet<EmailLog> EmailsLogs { get; set; }
        
    }

}