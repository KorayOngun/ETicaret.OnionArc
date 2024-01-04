﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Infrastructure.Operations
{
    public static class NameOperations
    {
        public static string CharacterRegulatory(string name)
        {
            return name.Replace("\"", "")
                     .Replace("!", "")
                     .Replace("'", "")
                     .Replace("^", "")
                     .Replace("+", "")
                     .Replace("%", "")
                     .Replace("&", "")
                     .Replace("/", "")
                     .Replace("(", "")
                     .Replace(")", "")
                     .Replace("=", "")
                     .Replace("?", "")
                     .Replace("_", "")
                     .Replace(" ", "-")
                     .Replace("@", "")
                     .Replace("€", "")
                     .Replace("¨", "")
                     .Replace("~", "")
                     .Replace(",", "")
                     .Replace(";", "")
                     .Replace(":", "")
                     .Replace(".", "-")
                     .Replace("Ö", "o")
                     .Replace("ö", "o")
                     .Replace("Ü", "u")
                     .Replace("ü", "u")
                     .Replace("ı", "i")
                     .Replace("İ", "i")
                     .Replace("ğ", "g")
                     .Replace("Ğ", "g")
                     .Replace("æ", "")
                     .Replace("ß", "")
                     .Replace("â", "a")
                     .Replace("î", "i")
                     .Replace("ş", "s")
                     .Replace("Ş", "s")
                     .Replace("Ç", "c")
                     .Replace("ç", "c")
                     .Replace("<", "")
                     .Replace(">", "")
                     .Replace("|", "");
        }
    }
}
