using Data.Models;
using System.Data.Entity;

namespace Data.Contexts
{
    public class LogiContext : DbContext
    {
        public LogiContext() : base("Name=Logi_DB")
        { }

        public LogiContext(string connectionString) : base(connectionString) { }

        public DbSet<Ship> Ships { get; set; }

        public DbSet<TodoItem> TodoItems { get; set; }

        public DbSet<BankAccountNumber> BankAccounts { get; set; }

    }
}
