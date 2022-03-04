namespace VendingMachine.Core.Mapper
{
    using AutoMapper;
    using VendingMachine.Core.Models;
    using VendingMachine.Entities;

    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Slot, ProductDto>()
                .ForMember(d => d.Name, opt => opt.MapFrom((src, dest) => src.Product?.Name));
        }
    }
}
