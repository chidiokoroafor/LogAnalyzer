using LogAnalyzerApi.Contracts;
using LogAnalyzerApi.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace LogAnalyzerApi.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExeptionHandler(this IApplicationBuilder app, ILoggerService logger)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        logger.LogError($"Something went wrong {contextFeature.Error}");

                        await context.Response.WriteAsync(new ErrorDetails()
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = "Internal server Error"
                        }.ToString());

                    }
                });
            });
        }
    }
}
