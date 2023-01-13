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

        public async Task SendMailAsync(string to, string subject, string body, bool isBodyHtml = true)
        {
            await SendMailAsync(new[] { to }, subject, body, isBodyHtml);
        }

        public async Task SendMailAsync(string[] tos, string subject, string body, bool isBodyHtml = true)
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

        public async Task SendPasswordResetMailAsync(string to, string userId, string resetToken)
        {
            StringBuilder mail = new();
            mail.AppendLine("Merhaba<br>Eğer yeni şifre talebinde bulunduysanız aşağıdaki linkten şifrenizi yenileyebilirsiniz.<br><a target=\"_blank\" href=\".../");
            mail.AppendLine(userId);
            mail.AppendLine("/");
            mail.AppendLine(resetToken);
            mail.AppendLine("\">Yeni şifre talebi için tıklayınız...</a></strong><br><br><span style=\"font-size:12px;\">NOT: Eğer li bu talper tarafınızca gerçekleştirilmemişse lütfen bu maili ciddiye almayınız.</span><br>Saygılarımızla...<br><br><br> Gndgd");
            await SendMailAsync(to, "Şifre Yenileme", mail.ToString());
        }
    }
}
