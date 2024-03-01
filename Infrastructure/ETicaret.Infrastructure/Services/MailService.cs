using ETicaret.Application.Abstractions.Services;
using ETicaret.Application.SettingObject;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Infrastructure.Services
{
    public class MailService : IMailService
    {
        readonly MailSettings _settings;
        public MailService(IConfiguration configuration)
        {
            _settings = configuration.GetSection(MailSettings.SettingName).Get<MailSettings>() ?? throw new Exception("e-mail settings are not set");
        }

        public async Task SendMessageAsync(string subject, string body, bool isBodyHtml = true,params string[] to)
        {
            MailMessage mail = new MailMessage();

            mail.IsBodyHtml = isBodyHtml;

            foreach (var _to in to)
                mail.To.Add(_to);

            mail.Subject = subject;
            mail.Body = body;
            mail.From = new(_settings.Mail, "botgg E-ticaret",System.Text.Encoding.UTF8);

            SmtpClient smtp = new SmtpClient();

            smtp.Credentials = new NetworkCredential(_settings.Mail,_settings.Password);
            smtp.EnableSsl = _settings.EnableSsl;
            smtp.Port = _settings.Port;
            smtp.Host = _settings.Host;

             await smtp.SendMailAsync(mail);
        }
    }
}
