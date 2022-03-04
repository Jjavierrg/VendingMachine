namespace VendingMachine.Core.Commands.Validators
{
    using FluentValidation;

    public class AddProductCommandValidator : AbstractValidator<AddProductCommand>
    {
        public AddProductCommandValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty().MaximumLength(200);
        }
    }
}
