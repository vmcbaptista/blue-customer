using BlueCustomer.Core.Customers.Repositories;
using BlueCustomer.Core.Customers.ValueObjects;
using FluentResults;
using Microsoft.AspNetCore.DataProtection;

namespace BlueCustomer.Core.Customers.Commands.Create
{
    public class CreateCustomerHandler : ICreateCustomerHandler
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IDataProtector _dataProtector;

        public CreateCustomerHandler(ICustomerRepository customerRepository, IDataProtector dataProtector)
        {
            _customerRepository = customerRepository;
            _dataProtector = dataProtector;
        }

        public async Task<Result> Handle(CreateCustomer command, CancellationToken cancellationToken)
        {
            var customer = new Customer(command.Id, new Name(command.FirstName, command.Surname), new Email(command.Email), new Password(_dataProtector.Protect(command.Password)));

            await _customerRepository.CreateCustomer(customer, cancellationToken).ConfigureAwait(false);
            await _customerRepository.SaveChanges(cancellationToken).ConfigureAwait(false);

            return Result.Ok();

        }
    }
}
