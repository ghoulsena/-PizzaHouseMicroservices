using MediatR;
using UserService.Domain.ValueObjects;
using UserService.Domain.Entities;
using UserService.Domain.Event;

namespace UserService.Domain.Entities
{
    public class User
    {
        public Guid Id { get; private set; }
        public string UserName { get; private set; } = string.Empty;

        public string EmailValue { get; private set; } = string.Empty;
        public string PasswordValue { get; private set; } = string.Empty;

        public Email Email => new Email(EmailValue);
        public Password Password => new Password(PasswordValue);

        private readonly List<INotification> _domainEvents = new();
        public IReadOnlyCollection<INotification> DomainEvents => _domainEvents.AsReadOnly();

        public List<Role> Roles { get; private set; } = new();

        private User() { }

        public static User Create(string userName, Email email, Password password)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                UserName = userName,
                EmailValue = email.Value,
                PasswordValue = password.Value
            };

            user.AddDomainEvent(new UserRegisteredDomainEvent(user));

            return user;
        }

        public void AddRole(Role role)
        {
            if (Roles.Any(r => r.Name == role.Name))
            {
                return;
            }

            Roles.Add(role);
        }



        public bool CheckPassword(string password)
        {
            return Password.Value == password;
        }

        private void AddDomainEvent(INotification eventItem)
        {
            _domainEvents.Add(eventItem);
        }
        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }
    }

}
