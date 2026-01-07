namespace EmailService.Application.Dtos
{
    public class UserRegisteredEvent
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = default!;
        public string EmailValue { get; set; } = default!;
    }


}
