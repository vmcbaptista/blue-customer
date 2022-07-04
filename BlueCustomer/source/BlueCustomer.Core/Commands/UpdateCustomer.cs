using BlueCustomer.Core.Errors;
using BlueCustomer.Core.Handlers;
using BlueCustomer.Core.Repositories;
using BlueCustomer.Core.ValueObjects;
using FluentResults;
using Microsoft.AspNetCore.DataProtection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueCustomer.Core.Commands
{

    public record UpdateCustomer(Guid Id, string FirstName, string Surname, string Email, string Password);

    public interface IUpdateCustomerHandler : ICommandHandler<UpdateCustomer> { }
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
                return Result.Fail(new DomainErrors.Customer.NotFound());
            }

            customer.Update(new Name(command.FirstName, command.Surname), new Email(command.Email), new Password(_dataProtector.Protect(command.Password)));

            await _customerRepository.UpdateCustomer(customer, cancellationToken).ConfigureAwait(false);
            await _customerRepository.SaveChanges(cancellationToken).ConfigureAwait(false);

            return Result.Ok();

        }
    }
}
