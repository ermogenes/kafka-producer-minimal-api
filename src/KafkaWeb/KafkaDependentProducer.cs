using Confluent.Kafka;

namespace KafkaWeb.Kafka;

public class KafkaDependentProducer<K, V>
{
    private IProducer<K, V> _handle;

    public KafkaDependentProducer(KafkaClientHandle handle)
    {
        _handle = new DependentProducerBuilder<K, V>(handle.Handle).Build();
    }

    public Task ProduceAsync(string topic, Message<K, V> message)
    {
        return _handle.ProduceAsync(topic, message);
    }

    public void Produce(
        string topic,
        Message<K, V> message,
        Action<DeliveryReport<K, V>> deliveryHandler = null
    )
    {
        _handle.Produce(topic, message, deliveryHandler);
        return;
    }

    public void Flush(TimeSpan timeout)
    {
        _handle.Flush(timeout);
    }
}
