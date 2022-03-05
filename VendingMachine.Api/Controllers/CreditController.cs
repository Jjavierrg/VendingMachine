namespace VendingMachine.Api.Controllers
{
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using System.Net;
    using VendingMachine.Core.Commands;
    using VendingMachine.Core.Models;
    using VendingMachine.Core.Querys;

    [ApiController]
    [Route("api/[controller]")]
    public class CreditController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CreditController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(UserCreditDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetUserCredit()
        {
            var query = new GetCreditQuery();
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(typeof(UserCreditDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AddCredit([FromBody] CoinWithQuantityDto[] coins)
        {
            var command = new InsertCoinsCommand(coins);
            var response = await _mediator.Send(command);
            return Ok(response);
        }
    }
}
