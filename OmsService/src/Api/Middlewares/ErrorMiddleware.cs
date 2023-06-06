using System.Net;
using Domain.Exceptions;
using Exception = System.Exception;

namespace Api.Middlewares;

internal sealed class ErrorMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next.Invoke(context);
        }
        catch (OrderNotFoundException e)
        {
            Console.WriteLine(e);
            context.Response.StatusCode = (int) HttpStatusCode.NotFound;
            await context.Response.WriteAsJsonAsync(new
            {
                Message = e.Message
            });
        }
        catch (DishNotFoundException e)
        {
            Console.WriteLine(e);
            context.Response.StatusCode = (int) HttpStatusCode.NotFound;
            await context.Response.WriteAsJsonAsync(new
            {
                Message = e.Message
            });
        }
        catch (DomainException e)
        {
            Console.WriteLine(e);
            context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
            await context.Response.WriteAsJsonAsync(new
            {
                Message = e.Message
            });
        }
        catch (Exception e)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            Console.WriteLine(e);
            await context.Response.WriteAsJsonAsync(new
            {
                Message = e.Message
            });
        }
    }
}