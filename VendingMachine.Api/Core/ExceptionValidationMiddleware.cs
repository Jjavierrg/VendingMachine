namespace VendingMachine.Api.Core
{
    using FluentValidation;
    using FluentValidation.Results;
    using System.Text.Json;

    internal sealed class ExceptionValidationMiddleware : IMiddleware
    {
        private readonly ILogger<ExceptionValidationMiddleware> _logger;

        public ExceptionValidationMiddleware(ILogger<ExceptionValidationMiddleware> logger) => _logger = logger;

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                await HandleExceptionAsync(context, e);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
        {
            var statusCode = GetStatusCode(exception);

            var response = new
            {
                status = statusCode,
                detail = exception.Message,
                errors = GetErrors(exception)
            };

            httpContext.Response.ContentType = "application/json";

            httpContext.Response.StatusCode = statusCode;

            await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
        }

        private static int GetStatusCode(Exception exception) =>
            exception switch
            {
                ValidationException => StatusCodes.Status422UnprocessableEntity,
                _ => StatusCodes.Status500InternalServerError
            };

        private static IEnumerable<ValidationFailure> GetErrors(Exception exception)
        {
            return exception is ValidationException validationException ? validationException.Errors : Array.Empty<ValidationFailure>();
        }
    }
}
