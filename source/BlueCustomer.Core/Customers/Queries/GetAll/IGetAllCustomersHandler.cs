using BlueCustomer.Core.Customers.Handlers;

namespace BlueCustomer.Core.Customers.Queries.GetAll
{
    public interface IGetAllCustomersHandler : IQueryHandler<GetAllCustomers, IReadOnlyCollection<Customer>> { }
}
