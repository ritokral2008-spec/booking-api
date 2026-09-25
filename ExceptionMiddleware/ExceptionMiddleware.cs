using BookingApi.DTOs;
using BookingApi.Exceptions;
using System.Net;
using System.Text.Json;

namespace BookingApi.ExceptionMiddleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionMiddleware(
            RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }

        }
        public async Task HandleExceptionAsync(
            HttpContext context,
            Exception exception)
        {
            context.Response.ContentType = "application/json";

            var statusCode = exception switch
            {
                UserNotFoundException => StatusCodes.Status404NotFound,
                RoomNotFoundException => StatusCodes.Status404NotFound,
                BookingNotFoundException => StatusCodes.Status404NotFound,

                RoomAlreadyBookedException => StatusCodes.Status409Conflict,
                EmailAlreadyExistsException => StatusCodes.Status409Conflict,
                UsernameAlreadyExistsException => StatusCodes.Status409Conflict,

                InvalidBookingDatesException => StatusCodes.Status400BadRequest,
                InvalidBookingStatusException => StatusCodes.Status400BadRequest,

                UnauthorizedAccessException => StatusCodes.Status401Unauthorized,

                _ => StatusCodes.Status500InternalServerError
            };

            context.Response.StatusCode = (int)statusCode;

            var response = new ErrorResponse
            {
                StatusCode = context.Response.StatusCode,
                Message = exception.Message,
                Errors = []
            };  

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response));
        }
    }
}
