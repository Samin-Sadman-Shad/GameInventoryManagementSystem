using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Serializers;
using Play.Catalogue.Service.Contracts;
using Play.Catalogue.Service.Repositories;
using Play.Catalogue.Service.Services;
using Play.Catalogue.Service.Settings;
using MongoDB.Driver;
using MassTransit;
using Play.Common.MongoDb;
using Play.Common.MassTransit;

var builder = WebApplication.CreateBuilder(args);

//tell the serializer to display the corresponding fields as following Bson Type
//BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
//BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

//var serviceSettings = builder.Configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
//builder.Services.AddSingleton(serviceProvider =>
//{
//    var mongodbSettings = builder.Configuration.GetSection(MongoDbOptions.MongoDbSettings).Get<MongoDbOptions>();
//    var mongoClient = new MongoClient(mongodbSettings?.ConnectionString);
//    var database = mongoClient.GetDatabase(serviceSettings?.ServiceName);
//    return database;
//});

builder.Services.AddMongoServices()
    .AddMassTransitWithRabbitMq();

builder.Services.AddScoped<IItemService, ItemService>();
//builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepositoryMongoDB<>));
builder.Services.AddScoped<IItemRepository, ItemRepositoryMongoDB>();

//builder.Services.AddMassTransit(x =>
//{
//    x.UsingRabbitMq((context, configurator) =>
//    {
//        var rabbitMqSettings = builder.Configuration.GetSection(RabbitMQSettingsOption.RabbitMQSettings)
//                                .Get<RabbitMQSettingsOption>();
//        configurator.Host(rabbitMqSettings!.Host);

//        //define or modify how queues are created in rabbitMq
//        configurator.ConfigureEndpoints(context, new KebabCaseEndpointNameFormatter(new ServiceSettings().ServiceName, false));
//    });
//});

//start mass transit hosted service
//this is the service that actually startes the Rabbit MQ bus
//so that messages can be published to different queuses and exchanges in rabbit mq
// builder.Services.AddMassTransitHostedService();
builder.Services.Configure<MassTransitHostOptions>(option =>
{

});

//necessary for the swagger to display the operations/api
builder.Services.AddControllers(options =>
{
    //runtime will not remove any async from the formatters
    options.SuppressAsyncSuffixInActionNames = true;
});
// Add services to the container.
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

/*var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();*/
//swagger will not have the access to run the endpoints without it
app.MapControllers();
app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
