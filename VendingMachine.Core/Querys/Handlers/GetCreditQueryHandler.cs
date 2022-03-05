namespace VendingMachine.Api.Controllers
{
    using AutoMapper;
    using MediatR;
    using VendingMachine.Core.Models;
    using VendingMachine.Core.Querys;
    using VendingMachine.Core.Services;

    public class GetCreditQueryHandler : IRequestHandler<GetCreditQuery, UserCreditDto>
    {
        private readonly IWalletService _walletService;

        public GetCreditQueryHandler(IWalletService walletService)
        {
            _walletService = walletService ?? throw new ArgumentNullException(nameof(walletService));
        }

        public Task<UserCreditDto> Handle(GetCreditQuery request, CancellationToken cancellationToken) => _walletService.GetCustomerCreditAsync();
    }
}
