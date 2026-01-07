using MediatR;
using System.Text.Json;
using UserService.Application.Interfaces;
using UserService.Domain.Event;

namespace UserService.Application.DomainEventHandlers
{
    public class UserRegisteredDomainEventHandler : INotificationHandler<UserRegisteredDomainEvent>
    {
        private readonly IKafkaProducer _producer;
        public UserRegisteredDomainEventHandler(IKafkaProducer producer)
        {
            _producer = producer;
        }


        public async Task Handle(UserRegisteredDomainEvent notification, CancellationToken cancellationToken)
        {
            await _producer.ProduceAsync(cancellationToken, notification.user);

        }
    }
}
