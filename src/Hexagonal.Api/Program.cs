using System.Reflection;
using Hexagonal.Api.Controllers.Dtos;
using Hexagonal.Api.Extensions;
using Hexagonal.Domain.Entities.Products;
using Hexagonal.EntityFramework;
using Hexagonal.EntityFramework.Repositories;
using Hexagonal.ProductService;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // 👈 your frontend URL
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials(); // optional, only if you're using cookies or auth headers
    });
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IProductService, ProductServices>();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Api Hexagonal", Version = "v1",
        Contact = new OpenApiContact
        {
            Name = "Developer Name",
            Email = "dev@example.com"
        } });
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
    throw new Exception(nameof(connectionString));

builder.Services.AddDbContext<HexagonalDbContext>(
    dbContextOptions => dbContextOptions
        .UseMySql(connectionString,new MySqlServerVersion("5.7"))
        // The following three options help with debugging, but should
        // be changed or removed for production.
        .LogTo(Console.WriteLine, LogLevel.Information)
        .EnableSensitiveDataLogging()
        .EnableDetailedErrors()
);

var app = builder.Build();

// Configure the HTTP request pipeline.
// Use CORS
app.UseCors("AllowFrontend");

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();
app.MapControllers();
app.UseRouting();

app.UseMiddleware<ExceptionMiddleware>();


app.Run();
