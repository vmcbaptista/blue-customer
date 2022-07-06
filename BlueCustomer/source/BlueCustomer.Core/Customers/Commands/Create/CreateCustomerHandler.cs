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
            var nameCreateResult = Name.Create(command.FirstName, command.Surname);
            if (nameCreateResult.IsFailed)
            {
                return nameCreateResult.ToResult();
            }

            var emailCreateResult = Email.Create(command.Email);
            if (emailCreateResult.IsFailed)
            {
                return emailCreateResult.ToResult();
            }

            var passwordCreateResult = Password.Create(_dataProtector.Protect(command.Password));
            if (passwordCreateResult.IsFailed)
            {
                return passwordCreateResult.ToResult();
            }

            var customerCreateResult = Customer.Create(command.Id, nameCreateResult.Value, emailCreateResult.Value, passwordCreateResult.Value);
            if (customerCreateResult.IsFailed)
            {
                return customerCreateResult.ToResult();
            }

            await _customerRepository.CreateCustomer(customerCreateResult.Value, cancellationToken).ConfigureAwait(false);
            await _customerRepository.SaveChanges(cancellationToken).ConfigureAwait(false);

            return Result.Ok();

        }
    }
}
