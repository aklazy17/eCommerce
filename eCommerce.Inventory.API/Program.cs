using AutoMapper;
using eCommerce.Inventory.API;
using eCommerce.Inventory.API.Data;
using eCommerce.Inventory.API.Handlers;
using eCommerce.Inventory.API.Repositories;
using eCommerce.RabbitMq;
using eCommerce.RabbitMq.Connection;
using eCommerce.RabbitMq.Producers;
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
    string databaseName = builder.Configuration.GetConnectionString("InventoryDbConnection") ?? throw new InvalidOperationException("InventoryDbConnection is not set");
    option.UseSqlServer(databaseName);
});

builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();

var rabbitMqConfig = builder.Configuration.GetSection("RabbitMq").Get<RabbitMqConfig>() ?? throw new InvalidOperationException("RabbitMq is not set");
builder.Services.AddSingleton<IRabbitMqConnection>(new RabbitMqConnection(rabbitMqConfig));

builder.Services.AddScoped<IMessageProducer, RabbitMqProducer>();

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