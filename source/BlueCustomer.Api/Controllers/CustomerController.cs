using BlueCustomer.Api.Models;
using BlueCustomer.Core.Customers;
using BlueCustomer.Core.Customers.Commands.Create;
using BlueCustomer.Core.Customers.Commands.Delete;
using BlueCustomer.Core.Customers.Commands.Update;
using BlueCustomer.Core.Customers.Errors;
using BlueCustomer.Core.Customers.Queries.GetAll;
using BlueCustomer.Core.Customers.Queries.GetById;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace BlueCustomer.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IUpdateCustomerHandler _updateCustomerHandler;
        private readonly IGetCustomerByIdHandler _getCustomerByIdHandler;
        private readonly IGetAllCustomersHandler _getAllCustomersHandler;
        private readonly ICreateCustomerHandler _createCustomerHandler;
        private readonly IDeleteCustomerHandler _deleteCustomerHandler;

        public CustomerController(IUpdateCustomerHandler updateCustomerHandler, IGetCustomerByIdHandler getCustomerByIdHandler, ICreateCustomerHandler createCustomerHandler, IDeleteCustomerHandler deleteCustomerHandler, IGetAllCustomersHandler getAllCustomersHandler)
        {
            _updateCustomerHandler = updateCustomerHandler;
            _getCustomerByIdHandler = getCustomerByIdHandler;
            _createCustomerHandler = createCustomerHandler;
            _deleteCustomerHandler = deleteCustomerHandler;
            _getAllCustomersHandler = getAllCustomersHandler;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> Get(CancellationToken cancellationToken)
        {
            var getAllCustomersResult = await _getAllCustomersHandler.Handle(new GetAllCustomers(), cancellationToken).ConfigureAwait(false);

            if (getAllCustomersResult.IsFailed)
            {

                return Problem(getAllCustomersResult);
            }

            return Ok(getAllCustomersResult.Value.Select(MapCustomerToReadDto));
        }

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<CustomerDto>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var getCustomerByIdResult = await _getCustomerByIdHandler.Handle(new GetCustomerById(id), cancellationToken).ConfigureAwait(false);

            if (getCustomerByIdResult.IsSuccess)
            {
                return Ok(MapCustomerToReadDto(getCustomerByIdResult.Value));
            }

            if (getCustomerByIdResult.HasError<CustomerNotFound>())
            {
                return NotFound();
            }

            return Problem(getCustomerByIdResult);
        }

        [HttpPost]
        public async Task<ActionResult<CustomerDto>> Post([FromBody] CreateCustomer createCustomer, CancellationToken cancellationToken)
        {

            var createResult = await _createCustomerHandler.Handle(createCustomer, cancellationToken).ConfigureAwait(false);

            if (createResult.IsSuccess)
            {
                return CreatedAtAction(nameof(GetById), new { id = createCustomer.Id }, MapCreateDtoToReadDto(createCustomer));
            }

            if (createResult.HasError<CustomerNotFound>())
            {
                return NotFound();
            }

            return Problem(createResult);

        }

        [HttpPut("{id:Guid}")]
        public async Task<ActionResult> PutAsync(Guid id, [FromBody] UpdateCustomer updateCustomer, CancellationToken cancellationToken)
        {
            if (id != updateCustomer.Id)
            {
                return BadRequest();
            }

            var updateResult = await _updateCustomerHandler.Handle(updateCustomer, cancellationToken).ConfigureAwait(false);

            if (updateResult.IsSuccess)
            {
                return NoContent();
            }

            if (updateResult.HasError<CustomerNotFound>())
            {
                return NotFound();
            }

            return Problem(updateResult);
        }

        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var deleteResult = await _deleteCustomerHandler.Handle(new DeleteCustomer(id), cancellationToken).ConfigureAwait(false);

            if (deleteResult.IsSuccess)
            {
                return NoContent();
            }

            if (deleteResult.HasError<CustomerNotFound>())
            {
                return NotFound();
            }

            return Problem(deleteResult);
        }

        private ObjectResult Problem(ResultBase result)
        {
            return Problem(string.Join(';', result.Errors));
        }

        private CustomerDto MapCreateDtoToReadDto(CreateCustomer customer)
        {
            return new CustomerDto(customer.Id, customer.FirstName, customer.Surname, customer.Email);
        }

        private CustomerDto MapCustomerToReadDto(Customer customer)
        {
            return new CustomerDto(customer.Id, customer.Name.FirstName, customer.Name.Surname, customer.Email.Value);
        }
    }
}
