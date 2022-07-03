using BlueCustomer.Core.Repositories;
using BlueCustomer.Infrastructure;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDataProtection();
builder.Services.AddTransient(sp => sp.GetRequiredService<IDataProtectionProvider>().CreateProtector("BlueProtector"));
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddDbContext<BlueContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Blue")));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<BlueContext>>();
    var context = services.GetRequiredService<BlueContext>();

    logger.LogInformation("Migrating database");
    context.Database.Migrate();
    logger.LogInformation("Database migrated");
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
