using ETicaretAPI.Application.Abstractions.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Infrastructure.Services
{
    public class MailService : IMailService
    {
        readonly IConfiguration configuration;

        public MailService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task SendMessageAsync(string to, string subject, string body, bool isBodyHtml = true)
        {
            await SendMessageAsync(new[] { to }, subject, body, isBodyHtml);
        }

        public async Task SendMessageAsync(string[] tos, string subject, string body, bool isBodyHtml = true)
        {
            MailMessage mailMessage = new();
            mailMessage.IsBodyHtml = isBodyHtml;
            foreach (var to in tos)
                mailMessage.To.Add(to);
            mailMessage.Subject = subject;
            mailMessage.Body = body;
            mailMessage.From = new(configuration["Mail:Username"], "NG E-Ticaret", Encoding.UTF8);
            SmtpClient smtp = new();
            smtp.Credentials = new NetworkCredential(configuration["Mail:Username"], configuration["Mail:Password"]);
            smtp.Port = Convert.ToInt32(configuration["Mail:Port"]);
            smtp.EnableSsl = true;
            smtp.Host = configuration["Mail:Host"];
            await smtp.SendMailAsync(mailMessage);
        }
    }
}
