namespace VendingMachine.Core.Behaviors
{
    using FluentValidation;
    using FluentValidation.Results;
    using MediatR;

    public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : class, IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators) => _validators = validators;
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (!_validators.Any())
                return await next();

            var context = new ValidationContext<TRequest>(request);
            var errors = new List<ValidationFailure>();

            foreach (var validator in _validators)
            {
                var validationResult = await validator.ValidateAsync(context);
                if (!validationResult.IsValid)
                    errors.AddRange(validationResult.Errors);
            }

            if (errors.Any())
                throw new ValidationException(errors);

            return await next();
        }
    }
}
