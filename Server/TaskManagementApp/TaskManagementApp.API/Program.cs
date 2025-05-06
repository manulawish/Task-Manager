using TaskManagementApp.Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using TaskManagementApp.Application.Task.Commands.CreateTask;
using TaskManagementApp.Infrastructure.Persistence;
using TaskManagementApp.Application.Tasks.Commands.CreateTask;
using FluentValidation.AspNetCore;
using FluentValidation;
using TaskManagementApp.Infrastructure.Services;
using TaskManagementApp.Infrastructure.Repositories;
using System.Reflection;
using TaskManagementApp.Application.Interfaces;
using TaskManagementApp.DApplicationomain.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddCustomAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(CreateTaskCommand).Assembly); // Application
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());    // WebAPI project
});

builder.Services.AddValidatorsFromAssembly(typeof(CreateTaskCommandValidator).Assembly);
builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApiDocument();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi();
}

app.UseHttpsRedirection();

app.UseCors("AllowAngularApp");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
