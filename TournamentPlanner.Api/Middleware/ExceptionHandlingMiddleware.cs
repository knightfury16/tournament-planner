using System.Net;
using System.Text.Json;
using TournamentPlanner.Domain.Exceptions;

namespace TournamentPlanner.Api.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError; // Default to 500
            var message = exception.Message;


            switch (exception)
            {
                case NotFoundException:
                    code = HttpStatusCode.NotFound;
                    break;
                case BadRequestException:
                case ValidationException:
                    code = HttpStatusCode.BadRequest;
                    break;
                case UnauthorizedException:
                    code = HttpStatusCode.Unauthorized;
                    break;
                case ForbiddenException:
                    code = HttpStatusCode.Forbidden;
                    break;
                case ConflictException:
                    code = HttpStatusCode.Conflict;
                    break;
                case ServiceUnavailableException:
                    code = HttpStatusCode.ServiceUnavailable;
                    break;
                case DependencyException:
                    code = HttpStatusCode.FailedDependency;
                    break;
                    // InternalServerErrorException is already covered by the default case
            }


            //keep this for test. will remove it after
            if (exception.Message.ToLower().Contains("not found")) code = HttpStatusCode.NotFound;
            if(exception.Message.ToLower().Contains("not complete"))code = HttpStatusCode.BadRequest;
            // else if (exception is BadRequestException) code = HttpStatusCode.BadRequest;

            var result = JsonSerializer.Serialize(new { error = message });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}