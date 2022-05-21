using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using Confluent.Kafka;

using KafkaWeb.Kafka;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.Development.json", true, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
    .AddEnvironmentVariables();

string kafkaServer = builder.Configuration
    .GetSection("dependencies:broker:producer").GetValue<string>("BootstrapServers");

string bizTopic = builder.Configuration
    .GetSection("dependencies:broker").GetValue<string>("bizTopic");

string redisServer = builder.Configuration
    .GetSection("dependencies").GetValue<string>("redis");

builder.Services.AddSingleton<KafkaClientHandle>();
builder.Services.AddSingleton<KafkaDependentProducer<long, string>>();
builder.Services.AddSingleton<DeliveryHandler>();

builder.Services.AddSingleton<IConnectionMultiplexer>(
    ConnectionMultiplexer.Connect(redisServer)
);

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/", async (
    [FromServices] IConnectionMultiplexer _connmux,
    [FromServices] KafkaDependentProducer<long, string> _producer,
    [FromServices] DeliveryHandler _delivery
) =>
{
    var redis = _connmux.GetDatabase();

    long id = await redis.StringIncrementAsync("bizCounter");
    string content = DateTime.Now.ToLongTimeString();

    _producer.Produce(
        bizTopic,
        new Message<long, string> { Key = id, Value = content },
        _delivery.DeliveryReportHandler
    );

    var msg = $"Kafka: {kafkaServer} ---- Redis: {redisServer} ---- Hits [GET /]: Message [{id:N0}] => [{content}]";

    // return Results.Accepted();
    return Results.Ok(msg);
});

app.Run();
