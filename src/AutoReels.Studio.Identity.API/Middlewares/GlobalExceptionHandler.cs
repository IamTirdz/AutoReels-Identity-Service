using AutoReels.Studio.Identity.Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;

namespace AutoReels.Studio.Identity.API.Middlewares
{
    public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
        {
            context.Response.ContentType = "application/json";
            context.Response.Headers["OperationId"] = Activity.Current!.RootId;

            if (exception is BaseException baseException)
                await HandleExceptionAsync(context, baseException);
            else
                await HandleExceptionAsync(context, exception);

            return true;
        }

        private async Task HandleExceptionAsync(HttpContext context, BaseException ex)
        {
            var statusCode = ex switch
            {
                ForbiddenException => HttpStatusCode.Forbidden,
                NotFoundException => HttpStatusCode.NotFound,
                UnauthorizedException => HttpStatusCode.Unauthorized,
                BadRequestException => HttpStatusCode.BadRequest,
                InputValidationException => HttpStatusCode.UnprocessableEntity,
                _ => HttpStatusCode.InternalServerError
            };

            var result = string.Empty;
            if (!string.IsNullOrEmpty(ex.ErrorResponse.Message))
            {
                //ex.ErrorResponse.Code = (int)statusCode;
                //ex.ErrorResponse.CodeInfo = statusCode.ToString();
                result = JsonConvert.SerializeObject(ex.ErrorResponse);
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            await context.Response.WriteAsync(result);
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var statusCode = HttpStatusCode.InternalServerError;
            var referrenceKey = Activity.Current!.RootId;
            const string message = "An unhandled exception has occurred.";

            var response = new
            {
                Message = message,
                ReferenceKey = referrenceKey,
                Code = statusCode,
                CodeInfo = statusCode.ToString()
            };

            var result = JsonConvert.SerializeObject(response);

            logger.LogError(ex, "{Message}--{ReferenceKey}", message, referrenceKey);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            await context.Response.WriteAsync(result);
        }
    }
}
