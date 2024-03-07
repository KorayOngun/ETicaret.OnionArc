using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Application.Abstractions.Services
{
    public interface IMailService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="subject">mail konusu</param>
        /// <param name="body">mail içeriği</param>
        /// <param name="isBodyHtml">mail içeriği html mi?</param>
        /// <param name="to">kime/kimlere</param>
        /// <returns></returns>
        Task SendMailAsync(string subject, string body, bool isBodyHtml = true,params  string[] to);

        Task SendPasswordResetMailAsync(string to, string userId, string resetToken);

        Task SendCompletedOrderMailAsync(string to, string orderCode, DateTime orderDate,string userNameSurname);
    }
}
