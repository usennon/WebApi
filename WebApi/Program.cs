using WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureCors();

// Add services to the container.

builder.Services.AddControllers().AddApplicationPart(typeof(WebApi.Presentation.AssemblyReference).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
