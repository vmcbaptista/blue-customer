using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BlueCustomer.Infrastructure.Migrations;
public class BlueContextFactory : IDesignTimeDbContextFactory<BlueContext>
{
    public BlueContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<BlueContext>();
        optionsBuilder.UseSqlServer(opts => opts.MigrationsAssembly(typeof(BlueContextFactory).Assembly.FullName));

        return new BlueContext(optionsBuilder.Options);
    }
}