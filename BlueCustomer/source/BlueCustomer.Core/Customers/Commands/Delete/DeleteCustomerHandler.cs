using BlueCustomer.Core.Customers.Errors;
using BlueCustomer.Core.Customers.Repositories;
using FluentResults;

namespace BlueCustomer.Core.Customers.Commands.Delete
{
    public class DeleteCustomerHandler : IDeleteCustomerHandler
    {
        private readonly ICustomerRepository _customerRepository;

        public DeleteCustomerHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<Result> Handle(DeleteCustomer command, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetCustomer(command.Id, cancellationToken).ConfigureAwait(false);

            if (customer == null)
            {
                return Result.Fail(new CustomerNotFound());
            }

            await _customerRepository.DeleteCustomer(customer, cancellationToken).ConfigureAwait(false);
            await _customerRepository.SaveChanges(cancellationToken).ConfigureAwait(false);

            return Result.Ok();

        }
    }
}
