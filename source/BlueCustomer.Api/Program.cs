using BlueCustomer.Core.Customers.Commands.Create;
using BlueCustomer.Core.Customers.Commands.Delete;
using BlueCustomer.Core.Customers.Commands.Update;
using BlueCustomer.Core.Customers.Queries.GetAll;
using BlueCustomer.Core.Customers.Queries.GetById;
using BlueCustomer.Core.Customers.Repositories;
using BlueCustomer.Infrastructure;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDataProtection();
builder.Services.AddTransient(sp => sp.GetRequiredService<IDataProtectionProvider>().CreateProtector("BlueProtector"));
builder.Services.AddTransient<ICustomerRepository, CustomerRepository>();
builder.Services.AddTransient<IGetAllCustomersHandler, GetAllCustomersHandler>();
builder.Services.AddTransient<IGetCustomerByIdHandler, GetCustomerByIdHandler>();
builder.Services.AddTransient<ICreateCustomerHandler, CreateCustomerHandler>();
builder.Services.AddTransient<IUpdateCustomerHandler, UpdateCustomerHandler>();
builder.Services.AddTransient<IDeleteCustomerHandler, DeleteCustomerHandler>();
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

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
