using BlueCustomer.Core.Customers;
using Microsoft.EntityFrameworkCore;

namespace BlueCustomer.Infrastructure
{
    public class BlueContext : DbContext
    {
        public BlueContext(DbContextOptions<BlueContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CustomerEntityTypeConfiguration).Assembly);
        }
    }
}