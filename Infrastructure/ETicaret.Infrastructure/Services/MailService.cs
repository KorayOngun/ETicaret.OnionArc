using ETicaret.Application.Abstractions.Services;
using ETicaret.Application.SettingObject;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
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
        readonly IConfiguration _configuration;
        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;
            _settings = configuration.GetSection(MailSettings.SettingName).Get<MailSettings>() ?? throw new Exception("e-mail settings are not set");
        }

        public IConfiguration Configuration => _configuration;

        public async Task SendCompletedOrderMailAsync(string to, string orderCode, DateTime orderDate, string userNameSurname)
        {
            string mail = $"Sayın {userNameSurname}  Merhaba<br> {orderDate} tarihinde vermiş olduğunuz {orderCode} kodlu siparişiniz tamamlanmış ve kargo firmasına verilmiştir.<br> Hayrını görünüz efendim...";
            await SendMailAsync(subject: $"{orderCode} numaralı siparişiniz tamamlandı", body: mail, to:to);
        }

        public async Task SendMailAsync(string subject, string body, bool isBodyHtml = true,params string[] to)
        {
            MailMessage mail = new MailMessage();

            mail.IsBodyHtml = isBodyHtml;

            foreach (var _to in to)
                mail.To.Add(_to);

            mail.Subject = subject;
            mail.Body = body;
            mail.From = new(_settings.Mail, "mini E-ticaret",System.Text.Encoding.UTF8);

            SmtpClient smtp = new SmtpClient();
            
            smtp.Credentials = new NetworkCredential(_settings.Mail,_settings.Password);
            smtp.EnableSsl = _settings.EnableSsl;
            smtp.Port = _settings.Port;
            smtp.Host = _settings.Host;

             await smtp.SendMailAsync(mail);
        }

        public async Task SendPasswordResetMailAsync(string to, string userId, string resetToken)
        {
            StringBuilder mail = new();
            mail.AppendLine("Merhaba<br>Eğer yeni şifre talabinde bulunduysanız aşağıdaki linkten şifrenizi yenileyebilirsiniz.<br><strong><a target=\"_blank\" href=\"");
            mail.Append(_configuration["AngularClientUrl"]);
            mail.Append("/update-password/");
            mail.Append(userId);
            mail.Append("/");
            mail.Append(resetToken);
            mail.Append("\">Yeni şifre talebi için tıklayınız...</a></strong><br><br><span style=\"font-size:12px;\">NOT : Eğer ki bu talep tarafınızca gerçekleştirilmemişse lütfen bu maili ciddiye almayınız. </span><br> Saygılarımızla... <br><br><br>mini e-ticaret");
            var m = mail.ToString();
            await SendMailAsync(to:to,subject:"Şifre Yenileme Talebi",body:mail.ToString(),isBodyHtml:true);
        }
    }
}
