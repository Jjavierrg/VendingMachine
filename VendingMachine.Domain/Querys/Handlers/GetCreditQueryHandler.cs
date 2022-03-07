namespace VendingMachine.Api.Controllers
{
    using AutoMapper;
    using MediatR;
    using VendingMachine.Domain.Models;
    using VendingMachine.Domain.Querys;
    using VendingMachine.Domain.Services;

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
