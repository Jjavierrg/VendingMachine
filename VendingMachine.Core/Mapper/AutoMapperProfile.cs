namespace VendingMachine.Core.Mapper
{
    using AutoMapper;
    using VendingMachine.Core.Models;
    using VendingMachine.Entities;

    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Product, ProductDto>();
        }
    }
}
