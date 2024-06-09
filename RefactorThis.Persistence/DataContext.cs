using Microsoft.EntityFrameworkCore;
using RefactorThis.Domain;

namespace RefactorThis.Persistence
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions options) : base(options) { }

        public DbSet<Invoice> Invoices { get; set; }
    }
}