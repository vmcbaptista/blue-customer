using BlueCustomer.Core.Customers.Errors;
using BlueCustomer.Core.Customers.Repositories;
using FluentResults;

namespace BlueCustomer.Core.Customers.Queries.GetAll
{
    public class GetAllCustomersHandler : IGetAllCustomersHandler
    {
        private readonly ICustomerRepository _customerRepository;

        public GetAllCustomersHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<Result<IReadOnlyCollection<Customer>>> Handle(GetAllCustomers query, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetCustomers(cancellationToken).ConfigureAwait(false);

            if (customer == null)
            {
                return Result.Fail(new CustomerNotFound());
            }

            return Result.Ok(customer);
        }
    }
}
