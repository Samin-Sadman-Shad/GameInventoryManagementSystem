using Play.Common.MassTransit;
using Play.Common.MongoDb;
using Play.Inventory.Service.Clients;
using Play.Inventory.Service.Contracts;
using Play.Inventory.Service.Repositories;
using Play.Inventory.Service.Services;
using Polly;
using Polly.Timeout;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddMongoServices().AddMassTransitWithRabbitMq();
builder.Services.AddScoped<ICatalogItemRepository, CatalogItemRepository>();

Random jitterer = new Random();

//specify base address of the other microservcie
//when catalog client is instantiated, it will automatically receive an instance of a http client
builder.Services.AddHttpClient<ICatalogClient, CatalogClient>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7163");
})
.AddTransientHttpErrorPolicy(builder =>
        builder.Or<TimeoutRejectedException>().WaitAndRetryAsync(
            retryCount: 5,

            sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
            + TimeSpan.FromMilliseconds(jitterer.Next(0, 1000)),

            onRetry: (outcome, timespan, retryAttempt) =>
            {
                Console.WriteLine($"Delaying for {timespan.TotalSeconds} then making retry {retryAttempt} times");
            }
))
.AddTransientHttpErrorPolicy(builder => 
    builder.Or<TimeoutRejectedException>().CircuitBreakerAsync(
        handledEventsAllowedBeforeBreaking: 5,
        durationOfBreak: TimeSpan.FromSeconds(30),
        onBreak: (outcome, timespan) =>
        {

        },
        onReset: () => { }
    )
)
.AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(2));
builder.Services.AddScoped<IInventoryItemRepository, InventoryItemRepository>();
builder.Services.AddScoped<IInventoryItemService, InventoryItemService>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
