using Dsw2025Tpi.Application.Exceptions;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using ApplicationException = Dsw2025Tpi.Application.Exceptions.ApplicationException;
using ArgumentException = Dsw2025Tpi.Application.Exceptions.ArgumentException;
using InvalidOperationException = Dsw2025Tpi.Application.Exceptions.InvalidOperationException;
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
            int internalCode = 1000;

            switch (e)
            {
                case ValidationException ve:
                    statusCode = HttpStatusCode.BadRequest;
                    title = "Validation Failed";
                    detail = ve.Message;
                    errors = ve.Errors.Select(e => new { e.Message, e.Code });
                    internalCode = 1001;
                    break;

                case EntityNotFoundException enEx:
                    statusCode = HttpStatusCode.NotFound;
                    title = "Entity Not Found";
                    detail = enEx.Message;
                    internalCode = 1002;
                    break;

                case NoContentException nocEx:
                    statusCode = HttpStatusCode.NoContent;
                    title = "No Content"; 
                    detail = nocEx.Message;
                    internalCode = 1003;
                    break;

                case DuplicatedEntityException duplicatedEx:
                    statusCode = HttpStatusCode.BadRequest;
                    title = "Entity Already Exists";
                    detail = duplicatedEx.Message;
                    internalCode = 1004;
                    break;

                case BadRequestException badRequestEx:
                    statusCode = HttpStatusCode.BadRequest;
                    title = "Bad Request";
                    detail = badRequestEx.Message;
                    internalCode = 1005;
                    break;

                case ArgumentException argEx:
                    statusCode = HttpStatusCode.BadRequest;
                    title = "Invalid Argument";
                    detail = argEx.Message;
                    internalCode = 1006;
                    break;

                case InvalidOperationException inEx:
                    statusCode = HttpStatusCode.BadRequest;
                    title = "Invalid Operation";
                    detail = inEx.Message;
                    internalCode = 1007;
                    break;

                case UnauthorizedException unEx:
                    statusCode = HttpStatusCode.Unauthorized;
                    title = "Unauthorized Access";
                    detail = unEx.Message;
                    internalCode = 1008;
                    break;

                case PreconditionException prEx:
                    statusCode = HttpStatusCode.PreconditionFailed;
                    title = "Precondition Failed";
                    detail = prEx.Message;
                    internalCode = 1009;
                    break;

                case ApplicationException appEx:
                    statusCode = HttpStatusCode.BadRequest;
                    title = "Application Error";
                    detail = appEx.Message;
                    internalCode = 1010;
                    break;
            }

            var errorResponse = errors != null
            ? (object)new { status = (int)statusCode, title = title, detail = detail, errors = errors, code = internalCode }
            : new { status = (int)statusCode, title = title, detail = detail, code = internalCode };

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