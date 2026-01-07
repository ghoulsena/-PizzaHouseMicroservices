using MediatR;
using UserService.Domain.Entities;

namespace UserService.Domain.Event
{
    public class UserRegisteredDomainEvent : INotification
    {
        public User user;
        public UserRegisteredDomainEvent(User user)
        {
            this.user = user;
        }
    }
}

