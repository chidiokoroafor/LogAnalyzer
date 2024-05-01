using LogAnalyzerApi.Contracts;
using LogAnalyzerApi.Services;

namespace LogAnalyzerApi.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
        }

        public static void ConfigureRequiredService(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerService, LoggerService>();
            services.AddScoped<ILogAnalyzerService, LoggAnalyzerService>();
        }
    }
}
