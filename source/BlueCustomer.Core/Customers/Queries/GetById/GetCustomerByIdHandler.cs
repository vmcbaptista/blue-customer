using BlueCustomer.Core.Customers.Errors;
using BlueCustomer.Core.Customers.Repositories;
using FluentResults;

namespace BlueCustomer.Core.Customers.Queries.GetById
{
    public class GetCustomerByIdHandler : IGetCustomerByIdHandler
    {
        private readonly ICustomerRepository _customerRepository;

        public GetCustomerByIdHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<Result<Customer>> Handle(GetCustomerById query, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetCustomer(query.Id, cancellationToken).ConfigureAwait(false);

            if (customer == null)
            {
                return Result.Fail(new CustomerNotFound());
            }

            return Result.Ok(customer);
        }
    }
}
