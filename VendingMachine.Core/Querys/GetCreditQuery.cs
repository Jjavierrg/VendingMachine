namespace VendingMachine.Core.Querys
{
    using MediatR;
    using VendingMachine.Core.Models;

    public class GetCreditQuery : IRequest<UserCreditDto>
    {
    }
}
