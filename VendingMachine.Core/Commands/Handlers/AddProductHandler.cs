namespace VendingMachine.Core.Commands.Handlers
{
    using AutoMapper;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using VendingMachine.Core.Models;
    using VendingMachine.Entities;
    using VendingMachine.Infrastructure.Core;

    public class AddProductCommandHandler : IRequestHandler<AddProductCommand, ProductDto>
    {
        private readonly IRepository<Product> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<AddProductCommandHandler> _logger;

        public AddProductCommandHandler(ILogger<AddProductCommandHandler> logger, IRepository<Product> repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ProductDto> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                Name = request.Name
            };

            _repository.Add(product);
            await _repository.SaveChangesAsync();
            _logger.LogInformation($"New Product Created");

            return _mapper.Map<ProductDto>(product);
        }
    }
}
