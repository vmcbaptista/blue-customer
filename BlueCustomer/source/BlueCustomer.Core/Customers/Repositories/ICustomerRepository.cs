namespace BlueCustomer.Core.Customers.Repositories
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
