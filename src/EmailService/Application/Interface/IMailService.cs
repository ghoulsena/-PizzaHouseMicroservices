using EmailService.Domain.Entity;

namespace EmailService.Application.Interface
{
    public interface IMailService
    {
        bool SendMail(MailData Mail_Data);
   
    }
}
