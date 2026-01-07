using Confluent.Kafka;
using MediatR;
using System.Text.Json;
using UserService.Application.Interfaces;
using UserService.Domain.Entities;

namespace UserService.Application.Services
{
    public class KafkaProducer:IKafkaProducer
    {
        private readonly ILogger<KafkaProducer> _logger;
        private readonly IProducer<Null, string> _producer;
        private const int DefaultTimeoutMs = 5000;

        public KafkaProducer(ILogger<KafkaProducer> logger)
        {
            _logger = logger;
            var config = new ProducerConfig
            {
                BootstrapServers = "localhost:9092",
                Acks = Acks.All,
                MessageTimeoutMs = DefaultTimeoutMs,
                RequestTimeoutMs = DefaultTimeoutMs,
                SocketTimeoutMs = DefaultTimeoutMs
            };

            _producer = new ProducerBuilder<Null, string>(config).Build();
        }
        public async Task ProduceAsync(CancellationToken cancellationToken, User user)
        {
            var userDto = new
            {
                user.Id,
                user.UserName,
                user.EmailValue
            };

            string message = JsonSerializer.Serialize(userDto);

            try
            {
      
                var dr = await _producer.ProduceAsync(
                    "registeruser",
                    new Message<Null, string> { Value = message },
                    cancellationToken);

                _logger.LogInformation("Доставленно '{Value}' в '{Tpo}'", dr.Value, dr.TopicPartitionOffset);
            }
            catch (ProduceException<Null, string> e)
            {
                _logger.LogError(e, "Доставка из Kafka не удалась: {Reason}", e.Error?.Reason);
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("Произведение Kafka отменено из-за токена или таймаута.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Непредвиденная ошибка при создании сообщения Kafka.");
            }
        }
    }
}
