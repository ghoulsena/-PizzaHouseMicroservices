using Confluent.Kafka;
using EmailService.Application.Dtos;
using EmailService.Application.Interface;
using EmailService.Domain.Entity;
using System.Text.Json;

namespace EmailService.Application.Service
{
    public class Consumer : BackgroundService
    {
        private readonly IMailService _mailService;
        private readonly ILogger<Consumer> _logger;

        public Consumer(ILogger<Consumer> logger, IMailService mailService)
        {
            _logger = logger;
            _mailService = mailService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "test",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = true
            };

            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();

            consumer.Subscribe("registeruser"); 

            _logger.LogInformation("Kafka consumer started...");

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        var result = consumer.Consume(stoppingToken);

                        if (result == null)
                        {
                            _logger.LogInformation("No message received in this poll.");
                            continue;
                        }

                        _logger.LogInformation($"RAW message received: {result.Message.Value} | Offset: {result.Offset}");

                        UserRegisteredEvent registerEvent = null;
                        try
                        {
                            registerEvent = JsonSerializer.Deserialize<UserRegisteredEvent>(result.Message.Value);
                            if (registerEvent == null)
                            {
                                _logger.LogWarning("Deserialized productEvent is null!");
                                continue;
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError($"Error deserializing message: {ex.Message}");
                            continue;
                        }

                        try
                        {
                            var mailData = new MailData
                            {
                                EmailToId = registerEvent.EmailValue,
                                EmailToName = registerEvent.UserName,
                                EmailSubject = "Добро пожаловать!",
                                EmailBody = $"Привет, {registerEvent.UserName}! Ты успешно зарегистрировался 🎉"
                            };

                            var success = _mailService.SendMail(mailData);

                            if (success)
                                _logger.LogInformation($"Welcome email sent to {registerEvent.EmailValue}");
                            else
                                _logger.LogWarning($"Failed to send welcome email to {registerEvent.EmailValue}");
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError($"Error sending email: {ex.Message}");
                        }
                    }
                    catch (ConsumeException ex)
                    {
                        _logger.LogError($"Kafka consume error: {ex.Error.Reason}");
                    }
                    catch (OperationCanceledException)
                    {
                        break;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Unexpected error in consumer loop: {ex.Message}");
                    }
                }
            }
            finally
            {
                consumer.Close();
                _logger.LogInformation("Kafka consumer closed.");
            }
        }
    }
}
