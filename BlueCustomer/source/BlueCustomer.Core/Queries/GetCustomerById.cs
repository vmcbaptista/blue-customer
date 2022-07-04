using BlueCustomer.Core.Entities;
using BlueCustomer.Core.Errors;
using BlueCustomer.Core.Handlers;
using BlueCustomer.Core.Repositories;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueCustomer.Core.Queries
{
    public record GetCustomerById(Guid Id);

    public interface IGetCustomerByIdHandler : IQueryHandler<GetCustomerById, Customer> { }

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
                return Result.Fail(new DomainErrors.Customer.NotFound());
            }

            return Result.Ok(customer);
        }
    }
}
