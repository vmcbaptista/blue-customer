using BlueCustomer.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueCustomer.Core.Repositories
{
    public interface ICustomerRepository
    {
        Task<Customer?> GetCustomer(Guid id, CancellationToken cancellationToken);
        Task<IReadOnlyCollection<Customer>> GetCustomers(CancellationToken cancellationToken);
        Task CreateCustomer(Customer customer, CancellationToken cancellationToken);
        Task UpdateCustomer(Customer customer, CancellationToken cancellationToken);
        Task DeleteCustomer(Customer customer, CancellationToken cancellationToken);
        Task SaveChanges(CancellationToken cancellationToken);
    }
}
