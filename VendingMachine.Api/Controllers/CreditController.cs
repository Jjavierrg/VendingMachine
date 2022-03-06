namespace VendingMachine.Api.Controllers
{
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.SignalR;
    using System.Net;
    using VendingMachine.Api.Hubs;
    using VendingMachine.Domain.Commands;
    using VendingMachine.Domain.Models;
    using VendingMachine.Domain.Querys;

    [ApiController]
    [Route("api/[controller]")]
    public class CreditController : ControllerBase
    {
        private readonly IMediator _mediator;
        private IHubContext<DisplayHub> _hub;

        public CreditController(IMediator mediator, IHubContext<DisplayHub> hub)
        {
            _mediator = mediator;
            _hub = hub;
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
            await _hub.Clients.All.SendAsync("display", "test");
            return Ok(response);
        }
    }
}
