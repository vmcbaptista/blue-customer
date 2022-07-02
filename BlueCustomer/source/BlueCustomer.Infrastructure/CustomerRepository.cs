using BlueCustomer.Core.Entities;
using BlueCustomer.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BlueCustomer.Infrastructure
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly BlueContext _context;

        public CustomerRepository(BlueContext context)
        {
            _context = context;
        }

        public async Task CreateCustomer(Customer customer, CancellationToken cancellationToken)
        {
            await _context.Customers.AddAsync(customer, cancellationToken);
        }

        public Task DeleteCustomer(Customer customer, CancellationToken cancellationToken)
        {
            _context.Customers.Remove(customer);
            return Task.CompletedTask;
        }

        public async Task<Customer?> GetCustomer(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Customers.SingleOrDefaultAsync(c => c.Id == id, cancellationToken: cancellationToken);
        }

        public async Task<IReadOnlyCollection<Customer>> GetCustomers(CancellationToken cancellationToken)
        {
            return await _context.Customers.ToArrayAsync(cancellationToken);
        }

        public Task UpdateCustomer(Customer customer, CancellationToken cancellationToken)
        {
            _context.Customers.Update(customer);
            return Task.CompletedTask;
        }

        public async Task SaveChanges(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}