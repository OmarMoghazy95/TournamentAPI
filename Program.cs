using Tournament.Api.Infrastructure.Extensions;
using Tournament.Api.Ioc;
using Tournament.Api.Presentation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.RegisterServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("ClientApp");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.InitializeDatabase();

app.MapEmployeeEndpoints();

await app.RunAsync();
