namespace VendingMachine.Core.Tests
{
    using AutoMapper;
    using MediatR;
    using Moq;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using VendingMachine.Api.Controllers;
    using VendingMachine.Core.Mapper;
    using VendingMachine.Core.Querys;
    using VendingMachine.Core.Tests.Shared;
    using VendingMachine.Entities;
    using Xunit;

    public class Queries
    {
        private readonly IEnumerable<Product> _products;
        private readonly RepositoryMoq<Product> _repository;

        public Queries()
        {
            _products = Enumerable.Range(1, 10).Select(i => new Product { Id = i, Name = $"Product ${i}" });
            _repository = new RepositoryMoq<Product>(_products);
        }

        [Fact]
        public async Task GetAllProductsQuery()
        {
            // Arrange
            var query = new GetProductsQuery();
            var handler = new GetProductsQueryHandler(_repository, MapperMock.Mapper);

            // Act
            var result = await handler.Handle(query, new CancellationToken());

            // Assert
            Assert.Equal(_products.Count(), result.Count());
            Assert.True(result.All(x => _products.Any(y => x.Id == y.Id)));
        }
    }
}