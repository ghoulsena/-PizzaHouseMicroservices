using UserService.Domain.Entities;

namespace UserService.Application.Interfaces;

public interface IKafkaProducer
{
    Task ProduceAsync(CancellationToken cancellationToken, User user);
}
