using ToDoListService.Api.Extensions;
using ToDoListService.Framework.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureDependencies(builder.Configuration);

builder.Services.AddControllers();

builder.Services.ConfigurePersistence(builder.Configuration);

builder.Services.AddAuthenticationWithJwtBearerConfiguration(builder.Configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ControllerAdviceMiddleware>();

app.MapControllers();

await app.RunAsync();

public partial class Program {}
