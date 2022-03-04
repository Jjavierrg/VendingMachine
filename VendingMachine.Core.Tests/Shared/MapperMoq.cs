namespace VendingMachine.Core.Tests.Shared
{
    using AutoMapper;
    using VendingMachine.Core.Mapper;

    public class MapperMock
    {
        private static IMapper mapper;

        public static IMapper Mapper => mapper ??= new MapperConfiguration(cfg => { cfg.AddProfile<AutoMapperProfile>(); }).CreateMapper();
    }
}
