using Dsw2025Tpi.Application.Exceptions;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using ApplicationException = Dsw2025Tpi.Application.Exceptions.ApplicationException;
using ValidationException= Dsw2025Tpi.Application.Exceptions.ValidationException;


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

            var statusCode = HttpStatusCode.InternalServerError;
            object errors = null!;
            string title = "Internal Server Error";
            string detail = e.Message;

            switch (e)
            {
                case ValidationException ve:
                    statusCode = HttpStatusCode.BadRequest;
                    title = "Bad Request";
                    detail = ve.Message;
                    errors = ve.Errors; // Extrae la lista de errores
                    break;

                case EntityNotFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    title = "Not Found";
                    break;
                case NoContentException:
                    statusCode = HttpStatusCode.NoContent;
                    title = "No Content";
                    break;
                case DuplicatedEntityException:
                case BadRequestException:
                case ApplicationException:
                case ArgumentException:
                case InvalidOperationException:
                    statusCode = HttpStatusCode.BadRequest;
                    title = "Bad Request";
                    break;
                case UnauthorizedException:
                    statusCode = HttpStatusCode.Unauthorized;
                    title = "Unauthorized";
                    break;
            }

            var errorResponse = errors != null
            ? (object)new { status = (int)statusCode, title = title, detail = detail, errors = errors }
            : new { status = (int)statusCode, title = title, detail = detail };

            //var errorResponse = new
            //{
            //    status = (int)statusCode,
            //    title = title,
            //    detail = detail,
            //    errors = errors
            //};

            context.Response.StatusCode = (int)statusCode;

            var json = JsonSerializer.Serialize(errorResponse);
            await context.Response.WriteAsync(json);
        }
    }
}













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
//                ValidationException => HttpStatusCode.BadRequest,
//                _ => HttpStatusCode.InternalServerError

//            };

//            context.Response.StatusCode = (int)statusCode;

//            var errorResponse = new
//            {
//                status = (int)statusCode,
//                title = statusCode.ToString(),
//                detail = e.Message
//            };

//            var json = JsonSerializer.Serialize(errorResponse);
//            await context.Response.WriteAsync(json);
//        }
//    }
//}