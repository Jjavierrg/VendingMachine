namespace VendingMachine.Api.Controllers
{
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using System.ComponentModel.DataAnnotations;
    using System.Net;
    using VendingMachine.Api.Models;
    using VendingMachine.Core.Commands;
    using VendingMachine.Core.Models;
    using VendingMachine.Core.Querys;

    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProductSlotDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            var query = new GetProductsQuery();
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ProductSlotDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get(int id)
        {
            var query = new GetProductQuery(id);
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpPost("order")]
        [ProducesResponseType(typeof(SellDto), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> OrderProduct([FromBody] SlotOrderDto order)
        {
            var command = new SellProductCommand(order?.Quantity ?? 0, order?.SlotNumber ?? 0);
            var response = await _mediator.Send(command);
            return Ok(response);
        }
    }
}
