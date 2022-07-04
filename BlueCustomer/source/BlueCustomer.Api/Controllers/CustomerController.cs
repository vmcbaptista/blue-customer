using BlueCustomer.Api.Models;
using BlueCustomer.Core.Commands;
using BlueCustomer.Core.Entities;
using BlueCustomer.Core.Errors;
using BlueCustomer.Core.Handlers;
using BlueCustomer.Core.Queries;
using BlueCustomer.Core.Repositories;
using BlueCustomer.Core.ValueObjects;
using FluentResults;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace BlueCustomer.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IDataProtector _dataProtector;
        private readonly IUpdateCustomerHandler _updateCommandHandler;
        private readonly IGetCustomerByIdHandler _getCustomerByIdHandler;

        public CustomerController(ICustomerRepository customerRepository, IDataProtector dataProtector, IUpdateCustomerHandler commandHandler, IGetCustomerByIdHandler getCustomerByIdHandler)
        {
            _customerRepository = customerRepository;
            _dataProtector = dataProtector;
            _updateCommandHandler = commandHandler;
            _getCustomerByIdHandler = getCustomerByIdHandler;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> Get(CancellationToken cancellationToken)
        {
            var customers = await _customerRepository.GetCustomers(cancellationToken).ConfigureAwait(false);
            return Ok(customers.Select(MapCustomerToReadDto));
        }

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<CustomerDto>> GetById(Guid id, CancellationToken cancellationToken)
        {
           var getCustomerByIdResult = await _getCustomerByIdHandler.Handle(new GetCustomerById(id), cancellationToken).ConfigureAwait(false);

            if (getCustomerByIdResult.IsSuccess)
            {
                return Ok(MapCustomerToReadDto(getCustomerByIdResult.Value));
            }

            if (getCustomerByIdResult.HasError<DomainErrors.Customer.NotFound>())
            {
                return NotFound();
            }

            return Problem(getCustomerByIdResult);
        }

        [HttpPost]
        public async Task<ActionResult<CustomerDto>> Post([FromBody] UpsertCustomerDto customerDto, CancellationToken cancellationToken)
        {
            await _customerRepository.CreateCustomer(MapUpsertDtoToCustomer(customerDto), cancellationToken);
            await _customerRepository.SaveChanges(cancellationToken).ConfigureAwait(false);
            return CreatedAtAction(nameof(GetById), new { id = customerDto.Id }, MapUpsertDtoToReadDto(customerDto));
        }

        [HttpPut("{id:Guid}")]
        public async Task<ActionResult> PutAsync(Guid id, [FromBody] UpdateCustomer updateCustomer, CancellationToken cancellationToken)
        {
            if (id != updateCustomer.Id)
            {
                return BadRequest();
            }

            var updateResult = await _updateCommandHandler.Handle(updateCustomer, cancellationToken).ConfigureAwait(false);

            if (updateResult.IsSuccess)
            {
                return NoContent();
            }

            if (updateResult.HasError<DomainErrors.Customer.NotFound>()) 
            { 
                return NotFound();
            }

            return Problem(updateResult);
        }

        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetCustomer(id, cancellationToken).ConfigureAwait(false);
            
            if (customer == null)
            {
                return NotFound();
            }
            
            await _customerRepository.DeleteCustomer(customer, cancellationToken);
            await _customerRepository.SaveChanges(cancellationToken).ConfigureAwait(false);

            return NoContent();
        }

        private ObjectResult Problem(ResultBase result)
        {
            return Problem(string.Join(';', result.Errors));
        }

        private CustomerDto MapUpsertDtoToReadDto(UpsertCustomerDto customer)
        {
            return new CustomerDto(customer.Id, customer.FirstName, customer.Surname, customer.Email);
        }

        private CustomerDto MapCustomerToReadDto(Customer customer)
        {
            return new CustomerDto(customer.Id, customer.Name.FirstName, customer.Name.Surname, customer.Email.Value);
        }

        private Customer MapUpsertDtoToCustomer(UpsertCustomerDto customer)
        {
            var id = customer.Id;
            var name = new Name(customer.FirstName, customer.Surname);
            var email = new Email(customer.Email);
            var password = new Password(_dataProtector.Protect(customer.Password));

            return new Customer(id, name, email, password);
        }
    }
}
