using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace CloudSalesAPI.Middleware
{
    public class LoggerMiddleware :IMiddleware
    {
        private readonly ILogger<LoggerMiddleware> _logger;
        
        public LoggerMiddleware(ILogger<LoggerMiddleware> logger)
        {
            _logger = logger;
        }


        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                ProblemDetails problem = new() { 
                Status = (int)HttpStatusCode.InternalServerError,
                Type = "Server Error",
                Title = "Server Error",
                Detail = "An internal server error has occured." + ex.Message
                };

                string json = JsonSerializer.Serialize(problem);

                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(json);
            }


        }
    }
}
