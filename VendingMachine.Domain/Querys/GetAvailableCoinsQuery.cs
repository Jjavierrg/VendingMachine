namespace VendingMachine.Domain.Querys
{
    using MediatR;
    using VendingMachine.Domain.Models;

    public class GetCreditQuery : IRequest<UserCreditDto>
    {
    }
}
