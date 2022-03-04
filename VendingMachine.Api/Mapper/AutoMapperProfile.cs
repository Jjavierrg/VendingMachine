namespace VendingMachine.Api.Mapper
{
    using AutoMapper;
    using VendingMachine.Api.Controllers;
    using VendingMachine.Entities;

    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Product, ProductDto>();
        }
    }
}
