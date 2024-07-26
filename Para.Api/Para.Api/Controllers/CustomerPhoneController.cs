using MediatR;
using Microsoft.AspNetCore.Mvc;
using Para.Base.Response;
using Para.Bussiness.Cqrs;
using Para.Schema;

namespace Para.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerPhoneController : ControllerBase
    {
        private readonly IMediator mediator;
        
        public CustomerPhoneController(IMediator mediator)
        {
            this.mediator = mediator;
        }


        [HttpGet]
        public async Task<ApiResponse<List<CustomerPhoneResponse>>> Get()
        {
            var operation = new GetAllCustomerPhoneQuery();
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpGet("{customerPhoneId}")]
        public async Task<ApiResponse<CustomerPhoneResponse>> Get([FromRoute]long customerPhoneId)
        {
            var operation = new GetCustomerPhoneByIdQuery(customerPhoneId);
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpPost]
        public async Task<ApiResponse<CustomerPhoneResponse>> Post([FromBody] CustomerPhoneRequest value)
        {
            var operation = new CreateCustomerPhoneCommand(value);
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpPut("{customerPhoneId}")]
        public async Task<ApiResponse> Put(long customerPhoneId, [FromBody] CustomerPhoneRequest value)
        {
            var operation = new UpdateCustomerPhoneCommand(customerPhoneId,value);
            var result = await mediator.Send(operation);
            return result;
        }

        [HttpDelete("{customerPhoneId}")]
        public async Task<ApiResponse> Delete(long customerPhoneId)
        {
            var operation = new DeleteCustomerPhoneCommand(customerPhoneId);
            var result = await mediator.Send(operation);
            return result;
        }
    }
}