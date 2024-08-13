using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Api.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next 
                ?? throw new ArgumentNullException(nameof(next));
            _logger = logger 
                ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context is null) { throw new ArgumentNullException(nameof(context)); }

            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Exception occured: {Message}", exception.Message);

                var problemDetails = new
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    ErrorMessage = LoggingConstant.GenericErrorMessage,
                    RequestId = context.TraceIdentifier
                };

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                await context.Response.WriteAsJsonAsync(problemDetails);
            }
        }
    }
}
