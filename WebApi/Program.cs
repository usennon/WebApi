using WebApi.Extensions;
using NLog;

var builder = WebApplication.CreateBuilder(args);

LogManager.Setup().LoadConfigurationFromFile(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));


builder.Services.ConfigureCors();

builder.Services.ConfigureLoggerService();

builder.Services.AddControllers().AddApplicationPart(typeof(WebApi.Presentation.AssemblyReference).Assembly);

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
