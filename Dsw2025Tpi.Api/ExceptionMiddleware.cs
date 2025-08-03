using Dsw2025Tpi.Application.Exceptions;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using ApplicationException = Dsw2025Tpi.Application.Exceptions.ApplicationException;

// ExceptionMiddleware.cs
//public class ExceptionMiddleware : IMiddleware
//{
//    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
//    {
//        try
//        {
//            await next(context);
//        }
//        catch (Exception e)
//        {
//            context.Response.ContentType = "application/json";

//                object errorResponse;

//            if (e is System.Text.Json.JsonException)
//            {
//                errorResponse = new
//                {
//                    Status = (int)HttpStatusCode.BadRequest,
//                    Title = "Invalid Request Body",
//                    Detail = "The request body could not be parsed as valid JSON."
//                };
//            }
//            else if (e is ValidationException ve)
//            {
//                var errors = new Dictionary<string, string[]>();
//                // Populate errors dictionary based on ValidationException details
//                errorResponse = new
//                {
//                    Status = (int)HttpStatusCode.BadRequest,
//                    Title = "Validation Failed",
//                    Detail = "One or more validation errors occurred.",
//                    Errors = errors
//                };
//            }
//            else
//            {
//                context.Response.StatusCode = (int)(e switch
//                {
//                    EntityNotFoundException => HttpStatusCode.NotFound,
//                    NoContentException => HttpStatusCode.NoContent,
//                    DuplicatedEntityException => HttpStatusCode.BadRequest,
//                    BadRequestException => HttpStatusCode.BadRequest,
//                    ApplicationException => HttpStatusCode.BadRequest,
//                    ArgumentException => HttpStatusCode.BadRequest,
//                    InvalidOperationException => HttpStatusCode.BadRequest,
//                    UnauthorizedException => HttpStatusCode.Unauthorized,
//                    _ => HttpStatusCode.InternalServerError
//                });

//                errorResponse = new
//                {
//                    Status = context.Response.StatusCode,
//                    Title = context.Response.StatusCode.ToString(),
//                    Detail = e.Message
//                };
//            }

//            var json = JsonSerializer.Serialize(errorResponse);
//            await context.Response.WriteAsync(json);
//        }
//    }
//}
//public class ExceptionMiddleware : IMiddleware
//{
//    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
//    {
//        try
//        {
//            await next(context);
//        }
//        catch (Exception e)
//        {
//            context.Response.ContentType = "application/json";

//            var statusCode = e switch
//            {
//                EntityNotFoundException => HttpStatusCode.NotFound,
//                NoContentException => HttpStatusCode.NoContent,
//                DuplicatedEntityException => HttpStatusCode.BadRequest,
//                BadRequestException => HttpStatusCode.BadRequest,
//                ApplicationException => HttpStatusCode.BadRequest,
//                ArgumentException => HttpStatusCode.BadRequest,
//                InvalidOperationException => HttpStatusCode.BadRequest,
//                UnauthorizedException => HttpStatusCode.Unauthorized,
//                JsonException => HttpStatusCode.BadRequest,
//                _ => HttpStatusCode.InternalServerError
//            };

//            context.Response.StatusCode = (int)statusCode;

//            var errorResponse = new
//            {
//                status = (int)statusCode,
//                title = statusCode.ToString(),
//                // Usa un mensaje más amigable para el error de JSON
//                detail = e is JsonException
//                         ? "Invalid request body format."
//                         : e.Message
//            };

//            var json = JsonSerializer.Serialize(errorResponse);
//            await context.Response.WriteAsync(json);
//        }
//    }
//}

public class ExceptionMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            context.Response.ContentType = "application/json";

            var statusCode = e switch
            {
                EntityNotFoundException => HttpStatusCode.NotFound,
                NoContentException => HttpStatusCode.NoContent,
                DuplicatedEntityException => HttpStatusCode.BadRequest,
                BadRequestException => HttpStatusCode.BadRequest,
                ApplicationException => HttpStatusCode.BadRequest,
                ArgumentException => HttpStatusCode.BadRequest,
                InvalidOperationException => HttpStatusCode.BadRequest,
                UnauthorizedException => HttpStatusCode.Unauthorized,
                _ => HttpStatusCode.InternalServerError

            };

            context.Response.StatusCode = (int)statusCode;

            var errorResponse = new
            {
                status = (int)statusCode,
                title = statusCode.ToString(),
                detail = e.Message
            };

            var json = JsonSerializer.Serialize(errorResponse);
            await context.Response.WriteAsync(json);
        }
    }
}