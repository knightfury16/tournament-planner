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
            var code = HttpStatusCode.BadRequest; // Default to 400
            var message = exception.Message;


            switch (exception)
            {
                case NotFoundException:
                    code = HttpStatusCode.NotFound;
                    break;
                case BadRequestException:
                case ArgumentNullException:
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
                case InternalServerErrorException:
                    code = HttpStatusCode.InternalServerError;
                    break;
            }


            var result = JsonSerializer.Serialize(new { error = message });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}