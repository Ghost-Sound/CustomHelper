using CustomHelper.SMTP.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomHelper.SMTP.Classes;
using MimeKit;
using MailKit.Net.Smtp;
using System.Net;
using MailKit.Security;

namespace CustomHelper.SMTP
{
    public class SmtpEmailSender : EmailSender
    {
        private readonly ILogger<SmtpEmailSender> _logger;
        private readonly SmtpOptions _smtpOptions;

        public SmtpEmailSender(ILogger<SmtpEmailSender> logger, IOptions<SmtpOptions> smtpOptions)
        {
            _logger = logger;
            _smtpOptions = smtpOptions.Value;
        }

        public override async Task SendEmailAsync(string to, string from, string subject, string body, bool htmlBody)
        {
            try
            {
                if (from == null)
                {
                    from = _smtpOptions.Username;
                }
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(from, _smtpOptions.Username));
                message.To.Add(new MailboxAddress("", to));
                message.Subject = subject;

                var bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = body;
                bodyBuilder.TextBody = body;
                message.Body = bodyBuilder.ToMessageBody();

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(_smtpOptions.Host, _smtpOptions.Port, SecureSocketOptions.StartTls); // Using _smtpOptions.Host and _smtpOptions.Port
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    await client.AuthenticateAsync(new NetworkCredential(_smtpOptions.Username, _smtpOptions.Password)); // Using _smtpOptions.Username and _smtpOptions.Password

                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }

                _logger.LogWarning("Sending email to {to} from {from} with subject {subject}.", to, from, subject);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }
    }
}
