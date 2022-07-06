using BlueCustomer.Core.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlueCustomer.Infrastructure
{
    public class CustomerEntityTypeConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.Property(c => c.Id);
            builder.OwnsOne(c => c.Name).Property(e => e.FirstName).HasColumnName("FirstName");
            builder.OwnsOne(c => c.Name).Property(e => e.Surname).HasColumnName("Surname");
            builder.OwnsOne(c => c.Password).Property(e => e.Value).HasColumnName("Password");
            builder.OwnsOne(c => c.Email).Property(e => e.Value).HasColumnName("Email");
        }
    }
}