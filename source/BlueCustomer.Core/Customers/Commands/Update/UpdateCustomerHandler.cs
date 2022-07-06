using BlueCustomer.Core.Customers.Errors;
using BlueCustomer.Core.Customers.Repositories;
using BlueCustomer.Core.Customers.ValueObjects;
using FluentResults;
using Microsoft.AspNetCore.DataProtection;

namespace BlueCustomer.Core.Customers.Commands.Update
{
    public class UpdateCustomerHandler : IUpdateCustomerHandler
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IDataProtector _dataProtector;

        public UpdateCustomerHandler(ICustomerRepository customerRepository, IDataProtector dataProtector)
        {
            _customerRepository = customerRepository;
            _dataProtector = dataProtector;
        }

        public async Task<Result> Handle(UpdateCustomer command, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetCustomer(command.Id, cancellationToken).ConfigureAwait(false);

            if (customer == null)
            {
                return Result.Fail(new CustomerNotFound());
            }

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

            var customerUpdateResult = customer.Update(nameCreateResult.Value, emailCreateResult.Value, passwordCreateResult.Value);
            if (customerUpdateResult.IsFailed)
            {
                return customerUpdateResult;
            }

            await _customerRepository.UpdateCustomer(customer, cancellationToken).ConfigureAwait(false);
            await _customerRepository.SaveChanges(cancellationToken).ConfigureAwait(false);

            return Result.Ok();

        }
    }
}
