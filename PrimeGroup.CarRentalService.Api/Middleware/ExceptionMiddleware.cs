namespace PrimeGroup.CarRentalService.Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // Pass control to the next middleware
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var response = new
            {
                IsSuccessful = false,
                Message = "An unexpected error occurred. Please try again later.",
                Details = exception.Message // Hide this in production
            };
            // Log the exception details
            var jsonResponse = System.Text.Json.JsonSerializer.Serialize(response);

            return context.Response.WriteAsync(jsonResponse);
        }
    }
}
