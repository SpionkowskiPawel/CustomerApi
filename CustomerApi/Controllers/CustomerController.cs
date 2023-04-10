using Customer.Bl.Commands.AddCustomer;
using Customer.Bl.Commands.RemoveCustomer;
using Customer.Bl.Queries.GetCustomers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Customer.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly IMediator mediator;

        public CustomerController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Get all Customers.
        /// </summary>
        [HttpGet("ListAllCustomers")]
        [ProducesResponseType(typeof(GetCustomersQueryResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> ListAllCustomers()
        {
            GetCustomersQueryResponse response = await mediator.Send(new GetCustomersQueryRequest());

            return Ok(response);
        }

        /// <summary>
        /// Add Customer.
        /// </summary>
        /// <param name="addCustomerCommandRequest">Customer required data.</param>
        [HttpPost("AddCustomer")]
        [ProducesResponseType(typeof(AddCustomerCommandResponse), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> AddCustomer(AddCustomerCommandRequest addCustomerCommandRequest)
        {
            AddCustomerCommandResponse response = await mediator.Send(addCustomerCommandRequest);

            return !response.Success ? NotFound() : Created(nameof(AddCustomer), response);
        }


        /// <summary>
        /// Remove Customer.
        /// </summary>
        /// <param name="removeCustomerCommandRequest">Customer identifier.</param>
        [HttpDelete("RemoveCustomer")]
        [ProducesResponseType(typeof(RemoveCustomerCommandRequest), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> RemoveCustomer(RemoveCustomerCommandRequest removeCustomerCommandRequest)
        {
            RemoveCustomerCommandResponse response = await mediator.Send(removeCustomerCommandRequest);

            return !response.Success ? NotFound() : Ok(response);
        }
    }
}