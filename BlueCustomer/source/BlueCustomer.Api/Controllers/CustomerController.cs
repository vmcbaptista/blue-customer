using BlueCustomer.Api.Models;
using BlueCustomer.Core.Repositories;
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
            return Ok(customers.Select(c => new CustomerDto(c.Id, c.Name.FirstName, c.Name.Surname, c.Email.Value)));
        }

        [HttpGet("{id:Guid}")]
        public ActionResult<CustomerDto> Get(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public ActionResult Post([FromBody] UpsertCustomerDto customerDto, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        [HttpPut("{id:Guid}")]
        public ActionResult Put(Guid id, [FromBody] UpsertCustomerDto customerDto, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{id:Guid}")]
        public ActionResult Delete(Guid id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
