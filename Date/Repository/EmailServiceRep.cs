using MailKit.Net.Smtp;
using MimeKit;
using MVP.Date.Interfaces;
using MVP.Date.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVP.Date.Repository
{
    public class EmailServiceRep : IEmailService
    {
        private readonly EmailConfig _emailConfig;
        public EmailServiceRep(EmailConfig emailConfig) => _emailConfig = emailConfig;
        public void SendEmail(Message message)
        {
            var emailMessage = CreateEmailMassage(message);
            Send(emailMessage);
        }
        private MimeMessage CreateEmailMassage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("email", _emailConfig.From));// name
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content};
            return emailMessage;
        }
        private void Send(MimeMessage mailMessage)
        {
            using var client = new SmtpClient();
            try
            {
                client.Connect(_emailConfig.SmtpServer, _emailConfig.Port,true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(_emailConfig.UserName, _emailConfig.Password);

                client.Send(mailMessage);
            }
            catch
            {
                throw;
            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();
            }
        }
    }

}
