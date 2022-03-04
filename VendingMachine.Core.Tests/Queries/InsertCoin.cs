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
        public Queries()
        {
        }

        [Fact]
        public async Task GetAllProductsAsync()
        {
            // Arrange
            var query = new GetProductsQuery();
            var products = Enumerable.Range(1, 10).Select(i => new Product { Id = i, Name = $"Product ${i}" });
            var repository = new RepositoryMoq<Product>(products);
            var handler = new GetProductsQueryHandler(repository, MapperMock.Mapper);

            // Act
            var result = await handler.Handle(query, new CancellationToken());

            // Assert
            Assert.All(result, x => products.Any(y => x.Id == y.Id));
            Assert.All(products, x => result.Any(y => x.Id == y.Id));
        }
    }
}