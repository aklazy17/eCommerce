using AutoMapper;
using eCommerce.ProductDetail.API;
using eCommerce.ProductDetail.API.Data;
using eCommerce.ProductDetail.API.Handlers;
using eCommerce.ProductDetail.API.Repositories;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Steeltoe.Discovery.Client;
using Steeltoe.Discovery.Eureka;

var builder = WebApplication.CreateBuilder(args);

// Add Global Exception Handler
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Automapper
IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(option =>
{
    string databaseName = builder.Configuration.GetConnectionString("ProductDetailDbConnection") ?? throw new InvalidOperationException("ProductDetailDbConnection is not set");
    option.UseSqlServer(databaseName);
});

builder.Services.AddScoped<IProductDetailRepository, ProductDetailRepository>();

builder.Services.AddControllers();

// Register Service in Eureka Server
builder.Services.AddServiceDiscovery(options => options.UseEureka());
builder.Services.AddDiscoveryClient(builder.Configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var serviceName = builder.Configuration["ServiceInfo:Name"] ?? throw new InvalidOperationException("Service name is not set");
var serviceVersion = builder.Configuration["ServiceInfo:Version"] ?? throw new InvalidOperationException("Service version is not set");

builder.Services.AddOpenTelemetry()
    .WithTracing(t =>
    {
        t.AddConsoleExporter()
         .AddSource(serviceName)
         .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName, serviceVersion: serviceVersion))
         .AddAspNetCoreInstrumentation()
         .AddHttpClientInstrumentation();
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Migrate db
app.MigrateDatabase();

app.UseAuthorization();

app.MapControllers();

app.UseExceptionHandler();

app.Run();
