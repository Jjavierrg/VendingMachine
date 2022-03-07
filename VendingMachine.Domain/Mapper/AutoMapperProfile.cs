namespace VendingMachine.Domain.Mapper
{
    using AutoMapper;
    using VendingMachine.Domain.Models;
    using VendingMachine.Entities;

    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Slot, ProductSlotDto>()
                .ForMember(d => d.Name, opt => opt.MapFrom((src, dest) => src.Product?.Name));

            CreateMap<CustomerWalletCoin, CoinWithQuantityDto>()
                .ForMember(d => d.Quantity, opt => opt.MapFrom((src, dest) => src.NumberOfCoins))
                .ForMember(d => d.CoinValue, opt => opt.MapFrom((src, dest) => src.Coin?.Value));

            CreateMap<MachineWalletCoin, CoinWithQuantityDto>()
                .ForMember(d => d.Quantity, opt => opt.MapFrom((src, dest) => src.NumberOfCoins))
                .ForMember(d => d.CoinValue, opt => opt.MapFrom((src, dest) => src.Coin?.Value));

            CreateMap<Coin, CoinWithQuantityDto>()
                .ForMember(d => d.CoinValue, opt => opt.MapFrom((src, dest) => src.Value));
        }
    }
}
