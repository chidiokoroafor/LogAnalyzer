using LogAnalyzerApi.Contracts;
using LogAnalyzerApi.Extensions;
using NLog;

var builder = WebApplication.CreateBuilder(args);

LogManager.Setup().LoadConfigurationFromFile(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

// Add services to the container.
builder.Services.ConfigureCors();
builder.Services.ConfigureRequiredService();
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseHsts();
}

var logger = app.Services.GetRequiredService<ILoggerService>();
app.ConfigureExeptionHandler(logger);
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("CorsPolicy");
app.UseAuthorization();

app.MapControllers();

app.Run();
