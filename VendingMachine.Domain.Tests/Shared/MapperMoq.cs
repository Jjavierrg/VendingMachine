namespace VendingMachine.Domain.Tests.Shared
{
    using AutoMapper;
    using VendingMachine.Domain.Mapper;

    public class MapperMock
    {
        private static IMapper? mapper;

        public static IMapper Mapper => mapper ??= new MapperConfiguration(cfg => { cfg.AddProfile<AutoMapperProfile>(); }).CreateMapper();
    }
}
