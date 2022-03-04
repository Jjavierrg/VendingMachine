namespace VendingMachine.Api.Controllers
{
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using System.ComponentModel.DataAnnotations;
    using System.Net;
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
        [ProducesResponseType(typeof(IEnumerable<ProductDto>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            var query = new GetProductsQuery();
            var response = await _mediator.Send(query);
            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ProductDto), (int)HttpStatusCode.Created)]
        [ProducesErrorResponseType(typeof(ValidationException))]
        public async Task<IActionResult> Post([FromBody] AddProductCommand productCommand)
        {
            var newProduct = await _mediator.Send(productCommand);
            return CreatedAtAction(nameof(Get), newProduct);
        }
    }
}
