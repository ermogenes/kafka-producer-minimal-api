using Confluent.Kafka;
using StackExchange.Redis;

namespace KafkaWeb.Kafka;

public class DeliveryHandler
{
    private ILogger<DeliveryHandler> _logger;
    private IDatabase _redis;

    public DeliveryHandler(
        ILogger<DeliveryHandler> logger,
        IConnectionMultiplexer connmux
    )
    {
        _logger = logger;
        _redis = connmux.GetDatabase();
    }

    public async void DeliveryReportHandler(
        DeliveryReport<long, string> deliveryReport
    )
    {
        var key = deliveryReport.Message.Key;
        var value = deliveryReport.Message.Value;

        switch (deliveryReport.Status)
        {
            case PersistenceStatus.NotPersisted:
                await _redis.StringIncrementAsync("np");
                break;
            case PersistenceStatus.PossiblyPersisted:
                await _redis.StringIncrementAsync("pp");
                break;
            case PersistenceStatus.Persisted:
                await _redis.StringIncrementAsync("p");
                break;
        }
    }
}
