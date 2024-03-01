﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Application.Abstractions.Services
{
    public interface IMailService
    {
        Task SendMessageAsync(string subject, string body, bool isBodyHtml = true,params  string[] to);

    }
}