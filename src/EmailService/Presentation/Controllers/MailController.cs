using Confluent.Kafka;
using EmailService.Application.Dtos;
using EmailService.Application.Interface;
using EmailService.Domain.Entity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace EmailService.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class MailController : ControllerBase
{
    private readonly IMailService _mailService;
   

    public MailController(IMailService mailService)
    {
        _mailService = mailService;

       
    }

    [HttpPost("send")]
    public   bool SendMail(MailData data)
    {
        return _mailService.SendMail(data);
    }

    
    }


