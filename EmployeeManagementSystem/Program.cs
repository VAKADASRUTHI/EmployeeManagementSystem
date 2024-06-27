using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AutoMapper;
using EmployeeManagementSystem.CosmosDB;
using EmployeeManagementSystem.Services;


var builder = WebApplication.CreateBuilder(args);
var entity = new EmployeeManagementSystem.Entities.PersonalDetailsEntity();
var dto = new EmployeeManagementSystem.DTOs.PersonalDetailsDTO();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddSingleton<CosmosClient>(new CosmosClient(builder.Configuration["CosmosDb:Account"], builder.Configuration["CosmosDb:Key"]));
builder.Services.AddSingleton<ICosmosDBService, CosmosDBService>();


builder.Services.AddScoped<IEmployeeService, EmployeeService>();

builder.Services.AddHttpClient();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
