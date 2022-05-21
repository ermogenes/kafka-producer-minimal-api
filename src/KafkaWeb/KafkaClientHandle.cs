using Confluent.Kafka;

namespace KafkaWeb.Kafka;

public class KafkaClientHandle : IDisposable
{
    private IProducer<byte[], byte[]> _kafkaProducer;
    private ILogger<KafkaClientHandle> _logger;

    public KafkaClientHandle(
        IConfiguration config,
        ILogger<KafkaClientHandle> logger
    )
    {
        _logger = logger;

        var conf = new ProducerConfig();
        config.GetSection("dependencies:broker:producer").Bind(conf);
        _kafkaProducer = new ProducerBuilder<byte[], byte[]>(conf).Build();
    }

    public Handle Handle { get => _kafkaProducer.Handle; }

    public void Dispose()
    {
        _logger.LogInformation("Flushing messages...");
        _kafkaProducer.Flush();
        _logger.LogInformation("Disposing ...");
        _kafkaProducer.Dispose();
    }
}
