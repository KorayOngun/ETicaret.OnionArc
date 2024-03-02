using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Application.SettingObject
{
    public class MailSettings
    {
        public const string SettingName = "MailSettings";


        public string Mail { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public bool EnableSsl { get; set; }
        public string Host { get; set; }
    }
}
