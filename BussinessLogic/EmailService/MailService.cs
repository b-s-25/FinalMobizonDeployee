using DomainLayer.EmailService;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogic
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;
        public MailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task SendEmailAsync(Email email)
        {
            var emails = new MimeMessage();
            emails.Sender = MailboxAddress.Parse(_mailSettings.mail);
            emails.To.Add(MailboxAddress.Parse(email.toEmail));
            emails.Subject = email.subject;
            var builder = new BodyBuilder();     
            builder.HtmlBody = email.body;
            emails.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.host, _mailSettings.port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.mail, _mailSettings.password);
            await smtp.SendAsync(emails);
            smtp.Disconnect(true);
        }
    }
}
