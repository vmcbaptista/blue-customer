using BlueCustomer.Api.Models;
using BlueCustomer.Core.Entities;
using BlueCustomer.Core.Repositories;
using BlueCustomer.Core.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace BlueCustomer.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
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
           var customer = await _customerRepository.GetCustomer(id, cancellationToken).ConfigureAwait(false);

            if (customer == null)
            {
                return NotFound();
            }

            return Ok(MapCustomerToReadDto(customer));
        }

        [HttpPost]
        public async Task<ActionResult<CustomerDto>> Post([FromBody] UpsertCustomerDto customerDto, CancellationToken cancellationToken)
        {
            await _customerRepository.CreateCustomer(MapUpsertDtoToCustomer(customerDto), cancellationToken);
            await _customerRepository.SaveChanges(cancellationToken).ConfigureAwait(false);
            return CreatedAtAction(nameof(GetById), new { id = customerDto.Id }, MapUpsertDtoToReadDto(customerDto));
        }

        [HttpPut("{id:Guid}")]
        public async Task<ActionResult> PutAsync(Guid id, [FromBody] UpsertCustomerDto customerDto, CancellationToken cancellationToken)
        {
            if (id != customerDto.Id)
            {
                return BadRequest();
            }

            var customer = await _customerRepository.GetCustomer(id, cancellationToken).ConfigureAwait(false);

            if (customer == null)
            {
                return NotFound();
            }

            customer.Update(new Name(customerDto.FirstName, customerDto.Surname), new Email(customerDto.Email), new Password(customerDto.Password));

            await _customerRepository.UpdateCustomer(customer, cancellationToken).ConfigureAwait(false);
            await _customerRepository.SaveChanges(cancellationToken).ConfigureAwait(false);

            return NoContent();
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
            var password = new Password(customer.Password);

            return new Customer(id, name, email, password);
        }
    }
}
