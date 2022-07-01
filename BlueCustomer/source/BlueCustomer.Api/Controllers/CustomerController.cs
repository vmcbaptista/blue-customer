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
            return Ok(customers.Select(MapCustomerToDto));
        }

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<CustomerDto>> GetById(Guid id, CancellationToken cancellationToken)
        {
           var customer = await _customerRepository.GetCustomer(id, cancellationToken).ConfigureAwait(false);

            if (customer == null)
            {
                return NotFound();
            }

            return Ok(MapCustomerToDto(customer));
        }

        [HttpPost]
        public async Task<ActionResult<CustomerDto>> Post([FromBody] InsertCustomerDto customerDto, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.CreateCustomer(MapDtoToCustomer(customerDto), cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = customerDto.Id }, MapCustomerToDto(customer));
        }

        [HttpPut("{id:Guid}")]
        public ActionResult Put(Guid id, [FromBody] InsertCustomerDto customerDto, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id:Guid}")]
        public ActionResult Delete(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }


        private CustomerDto MapCustomerToDto(Customer customer)
        {
            return new CustomerDto(customer.Id, customer.Name.FirstName, customer.Name.Surname, customer.Email.Value);
        }

        private Customer MapDtoToCustomer(InsertCustomerDto customer)
        {
            var id = customer.Id;
            var name = new Name(customer.FirstName, customer.Surname);
            var email = new Email(customer.Email);
            var password = new Password(customer.Password);

            return new Customer(id, name, email, password);
        }
    }
}
