using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaret.Application.Exceptions
{
    public class PasswordChangeFailedException : Exception
    {
        public PasswordChangeFailedException() : base("şifre güncellenirken bir sorun oluştu")
        {
            
        }
        public PasswordChangeFailedException(string message):base(message)
        {
            
        }
    }
}
