using BlueCustomer.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlueCustomer.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<CustomerDto>> Get(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
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
