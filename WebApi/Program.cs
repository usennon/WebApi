using WebApi.Extensions;
using NLog;
using WebApi;
using WebApi.Presentation.ActionFilters;

var builder = WebApplication.CreateBuilder(args);

LogManager.Setup().LoadConfigurationFromFile(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));


builder.Services.ConfigureCors();

builder.Services.ConfigureLoggerService();

builder.Services.ConfigureServiceManager();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddScoped<ValidateEvenPositiveNumberFilter>();

builder.Services.ConfigureServiceMemoryStorage();

builder.Services.ConfigureSwagger();


builder.Services.AddControllers().AddApplicationPart(typeof(WebApi.Presentation.AssemblyReference).Assembly);

var app = builder.Build();

app.UseExceptionHandler(opt => { });

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI(s =>
{
    s.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApi v1");
});

app.Run();
